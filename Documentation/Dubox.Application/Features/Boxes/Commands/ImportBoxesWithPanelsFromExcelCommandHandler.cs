using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Dubox.Application.Features.Boxes.Commands;

public class ImportBoxesWithPanelsFromExcelCommandHandler : IRequestHandler<ImportBoxesWithPanelsFromExcelCommand, Result<BoxWithPanelsImportResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelService _excelService;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISerialNumberService _serialNumberService;
    private readonly IQRCodeService _qrCodeService;

    private static readonly string[] RequiredHeaders = new[]
    {
        "Box Number.",
        "STR Panel No."
    };

    public ImportBoxesWithPanelsFromExcelCommandHandler(
        IUnitOfWork unitOfWork,
        IExcelService excelService,
        IDbContext dbContext,
        ICurrentUserService currentUserService,
        ISerialNumberService serialNumberService,
        IQRCodeService qrCodeService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _serialNumberService = serialNumberService;
        _qrCodeService = qrCodeService;
    }

    public async Task<Result<BoxWithPanelsImportResultDto>> Handle(ImportBoxesWithPanelsFromExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileStream == null)
            return Result.Failure<BoxWithPanelsImportResultDto>("No file stream provided");

        var fileExtension = Path.GetExtension(request.FileName).ToLower();
        if (fileExtension != ".xlsx" && fileExtension != ".xls")
            return Result.Failure<BoxWithPanelsImportResultDto>("Invalid file format. Please upload an Excel file (.xlsx or .xls)");

        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            return Result.Failure<BoxWithPanelsImportResultDto>("Project not found");

        // Check if project is on hold
        if (project.Status == ProjectStatusEnum.OnHold)
        {
            return Result.Failure<BoxWithPanelsImportResultDto>("Cannot import boxes. Projects on hold cannot be modified. Only project status changes are allowed.");
        }

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var errors = new List<string>();
        var successCount = 0;
        var failureCount = 0;
        var totalPanelsCreated = 0;
        var panelsToCreate = new List<BoxPanel>();
        var importedBoxesDto = new List<BoxWithPanelsDto>();

        // Dictionary to track the next sequence number for each panel type
        var panelTypeSequenceCounters = new Dictionary<string, int>();

        try
        {
            var stream = request.FileStream;
            stream.Position = 0;

            var (isValid, validationErrors) = await _excelService.ValidateExcelStructureAsync(stream, RequiredHeaders);
            if (!isValid)
            {
                return Result.Failure<BoxWithPanelsImportResultDto>($"Excel validation failed: {string.Join(", ", validationErrors)}");
            }

            stream.Position = 0;
            var importedRows = await _excelService.ReadFromExcelAsync<ExcelPanelRowDto>(stream, MapRowToDto);

            if (importedRows == null || importedRows.Count == 0)
            {
                return Result.Failure<BoxWithPanelsImportResultDto>("No valid data found in the Excel file");
            }

            // Fill in missing box numbers (when box number is empty, use the last one)
            string currentBoxNumber = null;
            foreach (var row in importedRows)
            {
                if (!string.IsNullOrWhiteSpace(row.BoxNumber))
                {
                    currentBoxNumber = row.BoxNumber;
                }
                else if (!string.IsNullOrWhiteSpace(currentBoxNumber))
                {
                    row.BoxNumber = currentBoxNumber; // Assign to previous box
                }
            }

            // Group rows by box number (after removing MGMT prefix)
            var boxGroups = importedRows
                .GroupBy(row => row.BoxNumber)
                .Where(g => !string.IsNullOrWhiteSpace(g.Key))
                .ToList();

            foreach (var boxGroup in boxGroups)
            {
                try
                {
                    var rawBoxNumber = boxGroup.Key;
                    
                    // Parse box number: Remove "MGMT-" and extract level and type
                    // Example: "MGMT-GF-B4" -> Level: "GF", Type: "B4"
                    var boxParts = ParseBoxNumber(rawBoxNumber);
                    if (boxParts == null)
                    {
                        errors.Add($"Invalid box number format: {rawBoxNumber}. Expected format: MGMT-[LEVEL]-[TYPE] (e.g., MGMT-GF-B4)");
                        failureCount += boxGroup.Count();
                        continue;
                    }

                    var (level, boxType) = boxParts.Value;

                    // Look up box type from project configuration
                    var projectBoxType = _unitOfWork.Repository<ProjectBoxType>()
                        .Get()
                        .FirstOrDefault(x => x.ProjectId == request.ProjectId 
                            && x.TypeName.ToUpper() == boxType.ToUpper() 
                            && x.IsActive);

                    if (projectBoxType == null)
                    {
                        errors.Add($"Box {rawBoxNumber}: Box Type '{boxType}' is not configured for this project");
                        failureCount += boxGroup.Count();
                        continue;
                    }

                    // FIND ALL EXISTING BOXES by Project, Floor, and Type
                    var existingBoxes = await _dbContext.Boxes
                        .Where(b => b.ProjectId == request.ProjectId 
                            && b.Floor.ToUpper() == level.ToUpper()
                            && b.ProjectBoxTypeId == projectBoxType.Id
                            && b.IsActive)
                        .ToListAsync(cancellationToken);

                    if (!existingBoxes.Any())
                    {
                        errors.Add($"Box {rawBoxNumber}: No existing boxes found with Floor '{level}' and Type '{boxType}' in this project");
                        failureCount += boxGroup.Count();
                        continue;
                    }

                    // Dictionary to track panels per box for this group
                    var boxPanelsDictionary = existingBoxes.ToDictionary(
                        box => box.BoxId,
                        box => new List<PanelDto>()
                    );

                    // Process all panels from Excel for this box type
                    foreach (var row in boxGroup)
                    {
                        if (string.IsNullOrWhiteSpace(row.PanelNumber))
                            continue;

                        // Parse panel number: Split "IW-250-1" into prefix "IW-250" and sequence "1"
                        var panelParts = ParsePanelNumber(row.PanelNumber);
                        if (panelParts == null)
                        {
                            errors.Add($"Box {rawBoxNumber}, Panel {row.PanelNumber}: Invalid panel number format. Expected format: [PREFIX]-[NUMBER] (e.g., IW-250-1)");
                            continue;
                        }

                        var (panelPrefix, sequenceNumber) = panelParts.Value;

                        // Find or create PanelType for this prefix (e.g., "IW-250")
                        var panelType = await _dbContext.PanelTypes
                            .FirstOrDefaultAsync(pt => pt.ProjectId == request.ProjectId 
                                && pt.PanelTypeCode == panelPrefix 
                                && pt.IsActive, cancellationToken);

                        if (panelType == null)
                        {
                            // Create new PanelType
                            panelType = new PanelType
                            {
                                ProjectId = request.ProjectId,
                                PanelTypeName = panelPrefix,
                                PanelTypeCode = panelPrefix,
                                IsActive = true,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = currentUserId
                            };
                            await _unitOfWork.Repository<PanelType>().AddAsync(panelType, cancellationToken);
                            await _unitOfWork.CompleteAsync(cancellationToken);
                        }

                        // Initialize sequence counter for this panel type if not already done
                        if (!panelTypeSequenceCounters.ContainsKey(panelPrefix))
                        {
                            // Get the highest existing sequence number for this panel type in the project
                            var existingPanels = await _dbContext.BoxPanels
                                .Where(bp => bp.ProjectId == request.ProjectId 
                                    && bp.PanelTypeId == panelType.PanelTypeId)
                                .Select(bp => bp.PanelName)
                                .ToListAsync(cancellationToken);

                            int maxSequence = 0;
                            foreach (var existingPanelName in existingPanels)
                            {
                                var parts = ParsePanelNumber(existingPanelName);
                                if (parts.HasValue && parts.Value.Prefix == panelPrefix)
                                {
                                    maxSequence = Math.Max(maxSequence, parts.Value.Sequence);
                                }
                            }

                            panelTypeSequenceCounters[panelPrefix] = maxSequence;
                        }

                        // Add this panel to ALL boxes with this floor and type
                        foreach (var box in existingBoxes)
                        {
                            // Auto-generate sequence number for this panel type
                            panelTypeSequenceCounters[panelPrefix]++;
                            var autoSequenceNumber = panelTypeSequenceCounters[panelPrefix];

                            // Generate panel name with auto sequence: "IW-250-1", "IW-250-2", etc.
                            var generatedPanelName = $"{panelPrefix}-{autoSequenceNumber}";

                            // Generate approval token for the panel
                            var qrCodeIdentifier = $"PANEL-{project.ProjectCode}-{box.SerialNumber}-{generatedPanelName}";
                            
                            // Note: The actual approval URL will be generated when displaying the QR code
                            // Format: https://app.com/public-approve?panelId={panelId}&token={token}&type=First
                            // The token is generated dynamically based on panelId and qrCodeIdentifier
                            var qrCodeUrl = _qrCodeService.GenerateQRCodeBase64(qrCodeIdentifier);

                            var newPanel = new BoxPanel
                            {
                                BoxId = box.BoxId,
                                ProjectId = request.ProjectId,
                                PanelTypeId = panelType.PanelTypeId,
                                PanelName = generatedPanelName,
                                QRCode = qrCodeIdentifier,
                                PanelStatus = PanelStatusEnum.NotStarted,
                                FirstApprovalStatus = "Pending",
                                SecondApprovalStatus = "Pending",
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = currentUserId
                            };

                            panelsToCreate.Add(newPanel);
                            totalPanelsCreated++;

                            boxPanelsDictionary[box.BoxId].Add(new PanelDto
                            {
                                PanelId = newPanel.BoxPanelId,
                                PanelName = generatedPanelName,
                                PanelPrefix = panelPrefix,
                                SequenceNumber = autoSequenceNumber
                            });
                        }
                    }

                    // Add all boxes with their panels to the result
                    foreach (var box in existingBoxes)
                    {
                        importedBoxesDto.Add(new BoxWithPanelsDto
                        {
                            BoxId = box.BoxId,
                            BoxTag = box.BoxTag,
                            Floor = level,
                            BoxType = boxType,
                            PanelCount = boxPanelsDictionary[box.BoxId].Count,
                            Panels = boxPanelsDictionary[box.BoxId]
                        });
                    }

                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Box {boxGroup.Key}: {ex.Message}");
                    failureCount++;
                }
            }

            if (totalPanelsCreated > 0)
            {
                // Save panels to existing boxes
                await _unitOfWork.Repository<BoxPanel>().AddRangeAsync(panelsToCreate, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            var result = new BoxWithPanelsImportResultDto
            {
                SuccessCount = successCount,
                FailureCount = failureCount,
                TotalPanelsCreated = totalPanelsCreated,
                Errors = errors,
                ImportedBoxes = importedBoxesDto
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<BoxWithPanelsImportResultDto>($"Error processing Excel file: {ex.Message}");
        }
    }

    private ExcelPanelRowDto MapRowToDto(Dictionary<string, object?> row)
    {
        return new ExcelPanelRowDto
        {
            BoxNumber = GetStringValue(row, "Box Number."),
            PanelNumber = GetStringValue(row, "STR Panel No.")
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

    /// <summary>
    /// Parse box number to extract level and type
    /// Example: "MGMT-GF-B4" -> ("GF", "B4")
    /// </summary>
    private (string Level, string Type)? ParseBoxNumber(string boxNumber)
    {
        if (string.IsNullOrWhiteSpace(boxNumber))
            return null;

        // Remove "MGMT-" prefix if present
        var cleaned = boxNumber.Trim();
        if (cleaned.StartsWith("MGMT-", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned.Substring(5); // Remove "MGMT-"
        }

        // Split by dash: "GF-B4" -> ["GF", "B4"]
        var parts = cleaned.Split('-');
        if (parts.Length >= 2)
        {
            return (parts[0].Trim(), parts[1].Trim());
        }

        return null;
    }

    /// <summary>
    /// Parse panel number to extract prefix and sequence number
    /// Example: "IW-250-1" -> ("IW-250", 1)
    /// </summary>
    private (string Prefix, int Sequence)? ParsePanelNumber(string panelNumber)
    {
        if (string.IsNullOrWhiteSpace(panelNumber))
            return null;

        // Find the last dash and split there
        var lastDashIndex = panelNumber.LastIndexOf('-');
        if (lastDashIndex > 0 && lastDashIndex < panelNumber.Length - 1)
        {
            var prefix = panelNumber.Substring(0, lastDashIndex).Trim();
            var sequenceStr = panelNumber.Substring(lastDashIndex + 1).Trim();

            if (int.TryParse(sequenceStr, out int sequence))
            {
                return (prefix, sequence);
            }
        }

        return null;
    }
}

// DTO for reading Excel rows
internal class ExcelPanelRowDto
{
    public string BoxNumber { get; set; } = string.Empty;
    public string PanelNumber { get; set; } = string.Empty;
}

