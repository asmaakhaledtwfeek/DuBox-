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
        "Box Tag (Auto-Generated)",
        "Box Type",
        "Floor",
        "Building Number"
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

        // Check if project is on hold - cannot import boxes for projects on hold
        if (project.Status == ProjectStatusEnum.OnHold)
        {
            return Result.Failure<BoxImportResultDto>("Cannot import boxes. Projects on hold cannot be modified. Only project status changes are allowed.");
        }

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var errors = new List<string>();
        var successCount = 0;
        var failureCount = 0;
        var boxLogs = new List<AuditLog>();
        var boxesToCreate = new List<Box>();
        // Dictionary to store box type and sub type names for DTO mapping
        var boxTypeMap = new Dictionary<int, string>();
        var boxSubTypeMap = new Dictionary<int, string>();
        Guid? factoryId = null;
        var factory = _unitOfWork.Repository<Factory>()
        .Get()
        .Where(f => f.Location == project.Location && f.IsActive)
        .OrderBy(f => f.CurrentOccupancy) // Prefer factories with lower occupancy
        .FirstOrDefault();
        if (factory != null)
            factoryId = factory.FactoryId;
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
                int boxTypeId;
                int? boxSubTypeId = null;
                ProjectBoxType? projectBoxType = null;
                ProjectBoxSubType? projectBoxSubType = null;
                
                try
                {
                    // Validate Box Type (required)
                    if (string.IsNullOrWhiteSpace(boxDto.BoxType))
                    {
                        errors.Add($"Row {rowNumber}: Box Type is required");
                        failureCount++;
                        continue;
                    }
                    
                    // Look up box type from project configuration (ProjectBoxType)
                    projectBoxType = _unitOfWork.Repository<ProjectBoxType>()
                        .Get()
                        .FirstOrDefault(x => x.ProjectId == request.ProjectId 
                            && x.TypeName.ToUpper() == boxDto.BoxType.Trim().ToUpper() 
                            && x.IsActive);
                    
                    if (projectBoxType == null)
                    {
                        errors.Add($"Row {rowNumber}: Box Type '{boxDto.BoxType}' is not configured for this project. Please use one of the valid box types from the template.");
                        failureCount++;
                        continue;
                    }
                    boxTypeId = projectBoxType.Id;
                    
                    // Store box type name in map for later use in DTO
                    if (!boxTypeMap.ContainsKey(boxTypeId))
                    {
                        boxTypeMap[boxTypeId] = projectBoxType.TypeName;
                    }
                    
                    // Validate Box Sub Type if provided
                    if (!string.IsNullOrWhiteSpace(boxDto.BoxSubType))
                    {
                        // Check if the box type supports sub-types
                        if (!projectBoxType.HasSubTypes)
                        {
                            errors.Add($"Row {rowNumber}: Box Type '{boxDto.BoxType}' does not support sub-types. Please leave the Box Sub Type field empty.");
                            failureCount++;
                            continue;
                        }
                        
                        // Look up sub-type and verify it belongs to the selected box type
                        projectBoxSubType = _unitOfWork.Repository<ProjectBoxSubType>()
                            .Get()
                            .FirstOrDefault(st => st.ProjectBoxTypeId == boxTypeId
                                && st.SubTypeName.ToUpper() == boxDto.BoxSubType.Trim().ToUpper()
                                && st.IsActive);
                        
                        if (projectBoxSubType == null)
                        {
                            errors.Add($"Row {rowNumber}: Box Sub Type '{boxDto.BoxSubType}' is not valid for Box Type '{boxDto.BoxType}'. Please check the 'Ref: Box Sub Types' sheet.");
                            failureCount++;
                            continue;
                        }
                        
                        boxSubTypeId = projectBoxSubType.Id;
                        
                        // Store box sub type name in map for later use in DTO
                        if (!boxSubTypeMap.ContainsKey(boxSubTypeId.Value))
                        {
                            boxSubTypeMap[boxSubTypeId.Value] = projectBoxSubType.SubTypeName;
                        }
                    }
                    else if (projectBoxType.HasSubTypes)
                    {
                        // Check if sub-types exist for this box type
                        var hasSubTypes = _unitOfWork.Repository<ProjectBoxSubType>()
                            .Get()
                            .Any(st => st.ProjectBoxTypeId == boxTypeId && st.IsActive);
                        
                        if (hasSubTypes)
                        {
                            errors.Add($"Row {rowNumber}: Box Type '{boxDto.BoxType}' requires a Box Sub Type. Please select one from the 'Ref: Box Sub Types' sheet.");
                            failureCount++;
                            continue;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(boxDto.Floor))
                    {
                        errors.Add($"Row {rowNumber}: Floor (Level) is required");
                        failureCount++;
                        continue;
                    }
                    
                    // Validate Floor against project configuration
                    var validFloor = await _dbContext.ProjectLevels
                        .AnyAsync(l => l.ProjectId == request.ProjectId 
                            && l.LevelCode.ToUpper() == boxDto.Floor.Trim().ToUpper() 
                            && l.IsActive, cancellationToken);
                    
                    if (!validFloor)
                    {
                        errors.Add($"Row {rowNumber}: Floor '{boxDto.Floor}' is not configured for this project. Please use one of the valid floors from the template.");
                        failureCount++;
                        continue;
                    }
                    
                    // Validate Building Number if provided
                    if (!string.IsNullOrWhiteSpace(boxDto.BuildingNumber))
                    {
                        var validBuilding = await _dbContext.ProjectBuildings
                            .AnyAsync(b => b.ProjectId == request.ProjectId 
                                && b.BuildingCode.ToUpper() == boxDto.BuildingNumber.Trim().ToUpper() 
                                && b.IsActive, cancellationToken);
                        
                        if (!validBuilding)
                        {
                            errors.Add($"Row {rowNumber}: Building Number '{boxDto.BuildingNumber}' is not configured for this project. Please use one of the valid buildings from the template.");
                            failureCount++;
                            continue;
                        }
                    }
                    
                    // Validate Zone if provided
                    if (!string.IsNullOrWhiteSpace(boxDto.Zone))
                    {
                        var validZone = await _dbContext.ProjectZones
                            .AnyAsync(z => z.ProjectId == request.ProjectId 
                                && z.ZoneCode.ToUpper() == boxDto.Zone.Trim().ToUpper() 
                                && z.IsActive, cancellationToken);
                        
                        if (!validZone)
                        {
                            errors.Add($"Row {rowNumber}: Zone '{boxDto.Zone}' is not configured for this project. Please use one of the valid zones from the template.");
                            failureCount++;
                            continue;
                        }
                    }
                    
                    // Validate Box Function if provided
                    if (!string.IsNullOrWhiteSpace(boxDto.BoxFunction))
                    {
                        var validFunction = await _dbContext.ProjectBoxFunctions
                            .AnyAsync(f => f.ProjectId == request.ProjectId 
                                && f.FunctionName.ToUpper() == boxDto.BoxFunction.Trim().ToUpper() 
                                && f.IsActive, cancellationToken);
                        
                        if (!validFunction)
                        {
                            errors.Add($"Row {rowNumber}: Box Function '{boxDto.BoxFunction}' is not configured for this project. Please use one of the valid functions from the template.");
                            failureCount++;
                            continue;
                        }
                    }
                    
                    // Auto-generate Box Tag from: Project-Building-Floor-Type-SubType
                    var boxTagParts = new List<string>();
                    
                    // 1. Project Code
                    boxTagParts.Add(project.ProjectCode);
                    
                    // 2. Building Number (optional)
                    if (!string.IsNullOrWhiteSpace(boxDto.BuildingNumber))
                    {
                        boxTagParts.Add(boxDto.BuildingNumber.Trim());
                    }
                    
                    // 3. Floor
                    boxTagParts.Add(boxDto.Floor.Trim());
                    
                    // 4. Box Type Abbreviation
                    var typeAbbreviation = !string.IsNullOrWhiteSpace(projectBoxType.Abbreviation) 
                        ? projectBoxType.Abbreviation 
                        : projectBoxType.TypeName.Substring(0, Math.Min(2, projectBoxType.TypeName.Length)).ToUpper();
                    boxTagParts.Add(typeAbbreviation);
                    
                    // 5. Box Sub Type Abbreviation (optional)
                    if (projectBoxSubType != null)
                    {
                        var subTypeAbbreviation = !string.IsNullOrWhiteSpace(projectBoxSubType.Abbreviation) 
                            ? projectBoxSubType.Abbreviation 
                            : projectBoxSubType.SubTypeName.Substring(0, Math.Min(1, projectBoxSubType.SubTypeName.Length)).ToUpper();
                        boxTagParts.Add(subTypeAbbreviation);
                    }
                    
                    // Generate the Box Tag
                    var generatedBoxTag = string.Join("-", boxTagParts);
                    
                    // Check for duplicates
                    if (existingBoxTags.Contains(generatedBoxTag.ToLower()))
                    {
                        errors.Add($"Row {rowNumber}: Box with tag '{generatedBoxTag}' already exists in this project");
                        failureCount++;
                        continue;
                    }
                    
                    // Check for duplicates in the current import batch
                    // We need to check against previously generated tags in this batch
                    var duplicateInBatch = boxesToCreate.Any(b => b.BoxTag.Equals(generatedBoxTag, StringComparison.OrdinalIgnoreCase));
                    if (duplicateInBatch)
                    {
                        errors.Add($"Row {rowNumber}: Duplicate BoxTag '{generatedBoxTag}' found in the import file (same combination of Project-Building-Floor-Type-SubType)");
                        failureCount++;
                        continue;
                    }
                    var lastSeq = _unitOfWork.Repository<Box>().Get()
                    .Where(b => b.ProjectId == request.ProjectId)
                    .Max(b => (int?)b.SequentialNumber) ?? 0;
                   var SequentialNumber = lastSeq + 1;
                    var yearOfProject = project.CreatedDate.Year.ToString().Substring(2, 2);
                    var serialNumber = _serialNumberService.GenerateSerialNumber("X", lastSeq, yearOfProject);
                    // Try to parse Zone enum if provided
                    string? parsedZone = null;
                    if (!string.IsNullOrWhiteSpace(boxDto.Zone))
                            parsedZone = boxDto.Zone;
                     
                    
                    var newBox = new Box
                    {
                        ProjectId = request.ProjectId,
                        BoxTag = generatedBoxTag,
                        BoxName = boxDto.BoxName?.Trim(),
                        BoxTypeId = boxTypeId,
                        BoxSubTypeId = boxSubTypeId,
                        Floor = boxDto.Floor.Trim(),
                        BuildingNumber = boxDto.BuildingNumber?.Trim(),
                        BoxFunction = boxDto.BoxFunction?.Trim(),
                        Zone = parsedZone,
                        Length = boxDto.Length,
                        Width = boxDto.Width,
                        Height = boxDto.Height,
                        UnitOfMeasure = UnitOfMeasureEnum.m,
                        Notes = boxDto.Notes?.Trim(),
                        SerialNumber = serialNumber,
                        SequentialNumber = SequentialNumber,
                        QRCodeString = $"ProjectCode: {project.ProjectCode}\nBoxTag: {generatedBoxTag}\nSerialNumber: {serialNumber}",
                        Status = BoxStatusEnum.NotStarted,
                        ProgressPercentage = 0,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        FactoryId=factoryId
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
                    existingBoxTags.Add(generatedBoxTag.ToLower());
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

            var importedBoxesDtos = boxesToCreate.Adapt<List<BoxDto>>().Select(dto =>
            {
                var boxTypeName = dto.BoxTypeId.HasValue && boxTypeMap.ContainsKey(dto.BoxTypeId.Value)
                    ? boxTypeMap[dto.BoxTypeId.Value]
                    : string.Empty;
                    
                var boxSubTypeName = dto.BoxSubTypeId.HasValue && boxSubTypeMap.ContainsKey(dto.BoxSubTypeId.Value)
                    ? boxSubTypeMap[dto.BoxSubTypeId.Value]
                    : null;
              
                return dto with
                {
                    ProjectCode = project.ProjectCode,
                    QRCodeImage = _qrCodeService.GenerateQRCodeBase64(dto.QRCodeString),
                    BoxType = boxTypeName,
                    BoxSubTypeName = boxSubTypeName
                };
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
            BoxTag = GetStringValue(row, "Box Tag (Auto-Generated)"), // Box Tag is auto-generated, not read from Excel
            BoxName = GetStringValue(row, "Box Name"),
            BoxType = GetStringValue(row, "Box Type"),
            BoxSubType = GetStringValue(row, "Box Sub Type"),
            Floor = GetStringValue(row, "Floor"),
            BuildingNumber = GetStringValue(row, "Building Number"),
            BoxFunction = GetStringValue(row, "Box Function"),
            Zone = GetStringValue(row, "Zone"),
            Length = GetDecimalValue(row, "Length"),
            Width = GetDecimalValue(row, "Width"),
            Height = GetDecimalValue(row, "Height"),
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

    //private BoxZone? GetBoxZoneValue(Dictionary<string, object?> row, string key)
    //{
    //    if (row.TryGetValue(key, out var value) && value != null)
    //    {
    //        var strValue = value.ToString()?.Trim();
    //        if (!string.IsNullOrEmpty(strValue) && Enum.TryParse<BoxZone>(strValue, true, out var zone))
    //        {
    //            return zone;
    //        }
    //    }
    //    return null;
    //}

}

