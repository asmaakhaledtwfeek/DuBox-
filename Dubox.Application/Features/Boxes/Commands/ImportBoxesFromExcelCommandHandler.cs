using Dubox.Application.DTOs;
using Dubox.Application.Services;
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
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectProgressService _projectProgressService;
    private readonly ISerialNumberService _serialNumberService;

    private static readonly string[] RequiredHeaders = new[]
    {
        "Box Tag",
        "Box Type",
        "Floor",
    };

    public ImportBoxesFromExcelCommandHandler(
        IUnitOfWork unitOfWork,
        IExcelService excelService,
        IDbContext dbContext,
        IQRCodeService qrCodeService,
        IBoxActivityService boxActivityService,
        ICurrentUserService currentUserService,
        IProjectProgressService projectProgressService,
        ISerialNumberService serialNumberService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
        _dbContext = dbContext;
        _qrCodeService = qrCodeService;
        _boxActivityService = boxActivityService;
        _currentUserService = currentUserService;
        _projectProgressService = projectProgressService;
        _serialNumberService = serialNumberService;
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

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var errors = new List<string>();
        var successCount = 0;
        var failureCount = 0;
        var boxLogs = new List<AuditLog>();
        var boxesToCreate = new List<Box>();

        try
        {
            var stream = request.FileStream;
            stream.Position = 0;

            var (isValid, validationErrors) = await _excelService.ValidateExcelStructureAsync(stream, RequiredHeaders);
            if (!isValid)
            {
                return Result.Failure<BoxImportResultDto>($"Excel validation failed: {string.Join(", ", validationErrors)}");
            }

            stream.Position = 0;
            var importedDtos = await _excelService.ReadFromExcelAsync<ImportBoxFromExcelDto>(stream, MapRowToBox);

            if (importedDtos == null || importedDtos.Count == 0)
            {
                return Result.Failure<BoxImportResultDto>("No valid data found in the Excel file");
            }

            var boxRepository = _unitOfWork.Repository<Box>();

            var existingBoxTags = await _dbContext.Boxes
                .Where(b => b.ProjectId == request.ProjectId)
                .Select(b => b.BoxTag.ToLower())
                .ToListAsync(cancellationToken);

            for (int i = 0; i < importedDtos.Count; i++)
            {
                var boxDto = importedDtos[i];
                var rowNumber = i + 2;

                try
                {
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
                    if (existingBoxTags.Contains(boxDto.BoxTag.ToLower()))
                    {
                        errors.Add($"Row {rowNumber}: Box with tag '{boxDto.BoxTag}' already exists in this project");
                        failureCount++;
                        continue;
                    }
                    var duplicateInFile = importedDtos.Take(i).Any(b => b.BoxTag.Equals(boxDto.BoxTag, StringComparison.OrdinalIgnoreCase));
                    if (duplicateInFile)
                    {
                        errors.Add($"Row {rowNumber}: Duplicate BoxTag '{boxDto.BoxTag}' found in the import file");
                        failureCount++;
                        continue;
                    }

                    var serialNumber = _serialNumberService.GenerateSerialNumber();
                    var newBox = new Box
                    {
                        ProjectId = request.ProjectId,
                        BoxTag = boxDto.BoxTag.Trim(),
                        BoxName = boxDto.BoxName?.Trim(),
                        BoxType = boxDto.BoxType.Trim(),
                        Floor = boxDto.Floor.Trim(),
                        Building = boxDto.Building?.Trim(),
                        Zone = boxDto.Zone ?? BoxZone.ZoneA,
                        Length = boxDto.Length,
                        Width = boxDto.Width,
                        Height = boxDto.Height,
                        UnitOfMeasure = UnitOfMeasureEnum.m,
                        BIMModelReference = boxDto.BIMModelReference?.Trim(),
                        Notes = boxDto.Notes?.Trim(),
                        SerialNumber = serialNumber,
                        QRCodeString = $"ProjectCode: {project.ProjectCode}\nBoxTag: {boxDto.BoxTag.Trim()}\nSerialNumber: {serialNumber}",
                        Status = BoxStatusEnum.NotStarted,
                        ProgressPercentage = 0,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };

                    var logEntry = new AuditLog
                    {
                        TableName = nameof(Box),
                        Action = "Creation (Import)",
                        OldValues = "N/A",
                        NewValues = $"Tag: {newBox.BoxTag}, Type: {newBox.BoxType}, Floor: {newBox.Floor}, Name: {newBox.BoxName}",
                        ChangedBy = currentUserId,
                        ChangedDate = DateTime.UtcNow,
                        Description = $"Box '{newBox.BoxTag}' created via Excel import (Row {rowNumber})."
                    };
                    boxLogs.Add(logEntry);

                    boxesToCreate.Add(newBox);
                    existingBoxTags.Add(boxDto.BoxTag.ToLower());
                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {rowNumber}: {ex.Message}");
                    failureCount++;
                }
            }

            if (successCount > 0)
            {
                await boxRepository.AddRangeAsync(boxesToCreate, cancellationToken);
                await _unitOfWork.Repository<AuditLog>().AddRangeAsync(boxLogs, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                for (int i = 0; i < boxesToCreate.Count; i++)
                {
                    boxLogs[i].RecordId = boxesToCreate[i].BoxId;
                }

                var projectLogs = new List<AuditLog>();
                var oldTotalBoxes = project.TotalBoxes;

                foreach (var box in boxesToCreate)
                {
                    await _boxActivityService.CopyActivitiesToBox(box, cancellationToken);
                }

                project.TotalBoxes += successCount;
                _unitOfWork.Repository<Project>().Update(project);

                projectLogs.Add(new AuditLog
                {
                    TableName = nameof(Project),
                    RecordId = project.ProjectId,
                    Action = "TotalBoxesUpdate",
                    OldValues = $"TotalBoxes: {oldTotalBoxes}",
                    NewValues = $"TotalBoxes: {project.TotalBoxes}",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"Total box count increased by {successCount} due to Excel import."
                });

                projectLogs.Add(new AuditLog
                {
                    TableName = nameof(Project),
                    RecordId = project.ProjectId,
                    Action = "BulkImport",
                    OldValues = $"File: {request.FileName}",
                    NewValues = $"Success: {successCount}, Failed: {failureCount}",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"Completed box import from file '{request.FileName}'. Imported {successCount} boxes."
                });

                await _unitOfWork.Repository<AuditLog>().AddRangeAsync(projectLogs, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                await _projectProgressService.UpdateProjectProgressAsync(
                    project.ProjectId,
                    currentUserId,
                    $"Project progress recalculated after importing {successCount} boxes from '{request.FileName}'.",
                    cancellationToken);
            }

            var importedBoxesDtos = boxesToCreate.Adapt<List<BoxDto>>().Select(dto => dto with
            {
                ProjectCode = project.ProjectCode,
                QRCodeImage = _qrCodeService.GenerateQRCodeBase64(dto.QRCodeString)
            }).ToList();

            var result = new BoxImportResultDto
            {
                SuccessCount = successCount,
                FailureCount = failureCount,
                Errors = errors,
                ImportedBoxes = importedBoxesDtos
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
            Zone = GetBoxZoneValue(row, "Zone"),
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

    private BoxZone? GetBoxZoneValue(Dictionary<string, object?> row, string key)
    {
        if (row.TryGetValue(key, out var value) && value != null)
        {
            var strValue = value.ToString()?.Trim();
            if (!string.IsNullOrEmpty(strValue) && Enum.TryParse<BoxZone>(strValue, true, out var zone))
            {
                return zone;
            }
        }
        return null;
    }

}

