using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Commands;

public class ImportBoxesFromExcelCommandHandler : IRequestHandler<ImportBoxesFromExcelCommand, Result<BoxImportResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelService _excelService;
    private readonly IDbContext _dbContext;
    private readonly IQRCodeService _qrCodeService;
    private readonly IBoxActivityService _boxActivityService;
    private static readonly string[] RequiredHeaders = new[]
    {
        "Box Tag",
        "Box Type",
        "Floor",
    };

    public ImportBoxesFromExcelCommandHandler(IUnitOfWork unitOfWork, IExcelService excelService, IDbContext dbContext, IQRCodeService qrCodeService, IBoxActivityService boxActivityService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
        _dbContext = dbContext;
        _qrCodeService = qrCodeService;
        _boxActivityService = boxActivityService;
    }

    public async Task<Result<BoxImportResultDto>> Handle(ImportBoxesFromExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileStream == null)
            return Result.Failure<BoxImportResultDto>("No file stream provided");

        var fileExtension = Path.GetExtension(request.FileName).ToLower();
        if (fileExtension != ".xlsx" && fileExtension != ".xls")
            return Result.Failure<BoxImportResultDto>("Invalid file format. Please upload an Excel file (.xlsx or .xls)");

        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            return Result.Failure<BoxImportResultDto>("Project not found");

        var errors = new List<string>();
        var importedBoxes = new List<BoxDto>();
        var successCount = 0;
        var failureCount = 0;

        try
        {
            var stream = request.FileStream;

            stream.Position = 0;
            var (isValid, validationErrors) = await _excelService.ValidateExcelStructureAsync(stream, RequiredHeaders);
            if (!isValid)
            {
                return Result.Failure<BoxImportResultDto>($"Excel validation failed: {string.Join(", ", validationErrors)}");
            }

            // Read boxes from Excel
            stream.Position = 0;
            var boxes = await _excelService.ReadFromExcelAsync<ImportBoxFromExcelDto>(stream, MapRowToBox);

            if (boxes == null || boxes.Count == 0)
            {
                return Result.Failure<BoxImportResultDto>("No valid data found in the Excel file");
            }

            var boxRepository = _unitOfWork.Repository<Box>();

            // Get existing box tags for this project to check duplicates
            var existingBoxTags = await _dbContext.Boxes
                .Where(b => b.ProjectId == request.ProjectId)
                .Select(b => b.BoxTag.ToLower())
                .ToListAsync(cancellationToken);

            // Process each box
            for (int i = 0; i < boxes.Count; i++)
            {
                var boxDto = boxes[i];
                var rowNumber = i + 2; // +2 because of header and 0-based index

                try
                {
                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(boxDto.BoxTag))
                    {
                        errors.Add($"Row {rowNumber}: BoxTag is required");
                        failureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(boxDto.BoxType))
                    {
                        errors.Add($"Row {rowNumber}: BoxType (Type) is required");
                        failureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(boxDto.Floor))
                    {
                        errors.Add($"Row {rowNumber}: Floor (Level) is required");
                        failureCount++;
                        continue;
                    }

                    // Check if box already exists in this project
                    if (existingBoxTags.Contains(boxDto.BoxTag.ToLower()))
                    {
                        errors.Add($"Row {rowNumber}: Box with tag '{boxDto.BoxTag}' already exists in this project");
                        failureCount++;
                        continue;
                    }

                    // Check for duplicates within the import file
                    var duplicateInFile = boxes.Take(i)
                        .Any(b => b.BoxTag.Equals(boxDto.BoxTag, StringComparison.OrdinalIgnoreCase));

                    if (duplicateInFile)
                    {
                        errors.Add($"Row {rowNumber}: Duplicate BoxTag '{boxDto.BoxTag}' found in the import file");
                        failureCount++;
                        continue;
                    }

                    // Create new box
                    var box = new Box
                    {
                        ProjectId = request.ProjectId,
                        BoxTag = boxDto.BoxTag.Trim(),
                        BoxName = boxDto.BoxName?.Trim(),
                        BoxType = boxDto.BoxType.Trim(),
                        Floor = boxDto.Floor.Trim(),
                        Building = boxDto.Building?.Trim(),
                        Zone = boxDto.Zone?.Trim(),
                        Length = boxDto.Length,
                        Width = boxDto.Width,
                        Height = boxDto.Height,
                        UnitOfMeasure = UnitOfMeasureEnum.m,
                        BIMModelReference = boxDto.BIMModelReference?.Trim(),
                        Notes = boxDto.Notes?.Trim(),
                        QRCodeString = $"{project.ProjectCode}_{boxDto.BoxTag.Trim()}",
                        Status = BoxStatusEnum.NotStarted,
                        ProgressPercentage = 0,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };

                    await boxRepository.AddAsync(box, cancellationToken);

                    // Create associated activities based on BoxType
                    await _boxActivityService.CopyActivitiesToBox(box, cancellationToken);

                    // Add to existing tags to prevent duplicates within the same import
                    existingBoxTags.Add(boxDto.BoxTag.ToLower());

                    var createdBoxDto = box.Adapt<BoxDto>() with
                    {
                        ProjectCode = project.ProjectCode,
                        QRCodeImage = _qrCodeService.GenerateQRCodeBase64(box.QRCodeString)
                    };

                    importedBoxes.Add(createdBoxDto);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {rowNumber}: {ex.Message}");
                    failureCount++;
                }
            }

            // Save all changes if any boxes were successfully imported
            if (successCount > 0)
            {
                await _unitOfWork.CompleteAsync(cancellationToken);

                // Update project's total boxes count
                project.TotalBoxes += successCount;
                _unitOfWork.Repository<Project>().Update(project);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            var result = new BoxImportResultDto
            {
                SuccessCount = successCount,
                FailureCount = failureCount,
                Errors = errors,
                ImportedBoxes = importedBoxes
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<BoxImportResultDto>($"Error processing Excel file: {ex.Message}");
        }
    }

    private ImportBoxFromExcelDto MapRowToBox(Dictionary<string, object?> row)
    {
        return new ImportBoxFromExcelDto
        {
            BoxTag = GetStringValue(row, "Box Tag"),
            BoxName = GetStringValue(row, "Box Name"),
            BoxType = GetStringValue(row, "Box Type"),
            Floor = GetStringValue(row, "Floor"),
            Building = GetStringValue(row, "Building"),
            Zone = GetStringValue(row, "Zone"),
            Length = GetDecimalValue(row, "Length"),
            Width = GetDecimalValue(row, "Width"),
            Height = GetDecimalValue(row, "Height"),
            BIMModelReference = GetStringValue(row, "BIMModelReference"),
            Notes = GetStringValue(row, "Notes")
        };
    }

    private string GetStringValue(Dictionary<string, object?> row, string key)
    {
        if (row.TryGetValue(key, out var value) && value != null)
        {
            return value.ToString()?.Trim() ?? string.Empty;
        }
        return string.Empty;
    }

    private decimal? GetDecimalValue(Dictionary<string, object?> row, string key)
    {
        if (row.TryGetValue(key, out var value) && value != null)
        {
            if (decimal.TryParse(value.ToString(), out var decimalValue))
            {
                return decimalValue;
            }
        }
        return null;
    }

}

