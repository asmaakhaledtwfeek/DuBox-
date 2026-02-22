using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Dubox.Application.Features.Schedule.Commands;

/// <summary>
/// Handler for importing schedule activities from Excel with hierarchical support
/// </summary>
public class ImportScheduleActivitiesFromExcelCommandHandler 
    : IRequestHandler<ImportScheduleActivitiesFromExcelCommand, Result<ScheduleActivityImportResultDto>>
{
    private readonly IDbContext _context;
    private readonly IExcelService _excelService;
    private readonly ILogger<ImportScheduleActivitiesFromExcelCommandHandler> _logger;

    public ImportScheduleActivitiesFromExcelCommandHandler(
        IDbContext context,
        IExcelService excelService,
        ILogger<ImportScheduleActivitiesFromExcelCommandHandler> logger)
    {
        _context = context;
        _excelService = excelService;
        _logger = logger;
    }

    public async Task<Result<ScheduleActivityImportResultDto>> Handle(
        ImportScheduleActivitiesFromExcelCommand request, 
        CancellationToken cancellationToken)
    {
        var result = new ScheduleActivityImportResultDto();

        try
        {
            if (request.FileStream == null)
            {
                return Result.Failure<ScheduleActivityImportResultDto>(
                    new Error("Import.InvalidFile", "No file stream provided"));
            }

            // Validate file extension
            var extension = Path.GetExtension(request.FileName).ToLowerInvariant();
            if (extension != ".xlsx" && extension != ".xls")
            {
                return Result.Failure<ScheduleActivityImportResultDto>(
                    new Error("Import.InvalidFileType", "Only Excel files (.xlsx, .xls) are supported."));
            }

            _logger.LogInformation(
                "Starting import of schedule activities from file: {FileName}, ProjectId: {ProjectId}",
                request.FileName, request.ProjectId);

            // Parse Excel file
            request.FileStream.Position = 0;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(request.FileStream);
            var worksheet = string.IsNullOrWhiteSpace(request.SheetName)
                ? package.Workbook.Worksheets.FirstOrDefault()
                : package.Workbook.Worksheets[request.SheetName];

            if (worksheet == null || worksheet.Dimension == null)
            {
                return Result.Failure<ScheduleActivityImportResultDto>(
                    new Error("Import.EmptySheet", "The Excel worksheet is empty or not found."));
            }

            // Parse the Excel data
            var activities = ParseExcelData(worksheet, result);

            if (result.Errors.Any())
            {
                return Result.Success(result); // Return with parsing errors
            }

            // Build hierarchy and save
            await BuildHierarchyAndSaveAsync(activities, request.ProjectId, result, cancellationToken);

            result.TotalProcessed = activities.Count;

            _logger.LogInformation(
                "Import completed: {Total} total, {Success} imported, {Skipped} skipped, {Errors} errors",
                result.TotalProcessed, result.SuccessfullyImported, result.Skipped, result.Errors.Count);

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during schedule activity import");
            result.Errors.Add($"Unexpected error: {ex.Message}");
            return Result.Success(result);
        }
    }

    private List<ScheduleActivityImportDto> ParseExcelData(ExcelWorksheet worksheet, ScheduleActivityImportResultDto result)
    {
        var activities = new List<ScheduleActivityImportDto>();
        var rowCount = worksheet.Dimension.End.Row;

        // Find header row and column mappings
        var columnMap = new Dictionary<string, int>();
        int headerRow = 1;

        // Scan first few rows to find the header row
        for (int row = 1; row <= Math.Min(5, rowCount); row++)
        {
            var firstCell = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
            if (firstCell != null && (firstCell.Contains("Activity ID", StringComparison.OrdinalIgnoreCase) ||
                                     firstCell.Contains("Activity Code", StringComparison.OrdinalIgnoreCase)))
            {
                headerRow = row;
                break;
            }
        }

        // Build column mapping
        var colCount = worksheet.Dimension.End.Column;
        for (int col = 1; col <= colCount; col++)
        {
            var header = worksheet.Cells[headerRow, col].Value?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(header))
            {
                columnMap[header] = col;
            }
        }

        _logger.LogInformation("Found {Count} columns: {Columns}", 
            columnMap.Count, string.Join(", ", columnMap.Keys));

        // Use a stack to track parent hierarchy based on indentation
        var parentStack = new Stack<(int IndentLevel, ScheduleActivityImportDto Activity)>();

        // Process data rows
        for (int row = headerRow + 1; row <= rowCount; row++)
        {
            try
            {
                var activity = ParseRowWithIndentation(worksheet, row, columnMap, parentStack);
                if (activity != null)
                {
                    activities.Add(activity);
                    
                    // Log first 30 rows for debugging
                    if (row <= headerRow + 30)
                    {
                        _logger.LogInformation("Row {Row}: Level={Level}, Code='{Code}', Parent='{Parent}'",
                            row, activity.IndentLevel, activity.ActivityCode, activity.ParentCode ?? "ROOT");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Row {row}: {ex.Message}");
                _logger.LogWarning("Error parsing row {Row}: {Message}", row, ex.Message);
            }
        }

        _logger.LogInformation("Parsed {Count} activities with hierarchy from indentation", activities.Count);
        return activities;
    }

    private ScheduleActivityImportDto? ParseRowWithIndentation(
        ExcelWorksheet worksheet, 
        int row, 
        Dictionary<string, int> columnMap,
        Stack<(int IndentLevel, ScheduleActivityImportDto Activity)> parentStack)
    {
        // Get Activity ID/Code with raw value to preserve indentation
        var activityCodeRaw = GetCellValueRaw(worksheet, row, columnMap, "Activity ID", "Activity Code");
        
        if (string.IsNullOrWhiteSpace(activityCodeRaw))
        {
            return null; // Skip empty rows
        }

        // Calculate indentation level - try multiple methods
        int indentLevel = 0;
        string activityCode = activityCodeRaw.Trim();
        
        // Method 1: Check for leading spaces in the text
        int leadingSpaces = activityCodeRaw.TakeWhile(c => c == ' ').Count();
        if (leadingSpaces > 0)
        {
            indentLevel = leadingSpaces / 2;
        }
        else
        {
            // Method 2: Check Excel's Style.Indent or OutlineLevel
            if (columnMap.TryGetValue("Activity ID", out int activityIdCol) || 
                columnMap.TryGetValue("Activity Code", out activityIdCol))
            {
                var cell = worksheet.Cells[row, activityIdCol];
                indentLevel = cell.Style.Indent;
                
                // Method 3: Try OutlineLevel as fallback
                if (indentLevel == 0)
                {
                    indentLevel = worksheet.Row(row).OutlineLevel;
                }
            }
        }

        // Skip rows without proper activity code
        if (string.IsNullOrWhiteSpace(activityCode) || activityCode.Length < 2)
        {
            return null;
        }

        var activityName = GetCellValue(worksheet, row, columnMap, "Activity Name") 
                          ?? GetCellValue(worksheet, row, columnMap, "ActivityName")
                          ?? activityCode;

        var activity = new ScheduleActivityImportDto
        {
            ActivityCode = activityCode,
            ActivityName = activityName,
            IndentLevel = indentLevel,
            PlannedStartDate = GetDateValue(worksheet, row, columnMap, "BL1 Start", "Start", "Planned Start"),
            PlannedFinishDate = GetDateValue(worksheet, row, columnMap, "BL1 Finish", "Finish", "Planned Finish"),
            OriginalDuration = GetIntValue(worksheet, row, columnMap, "Original Duration", "Duration") ?? 1,
            Description = GetCellValue(worksheet, row, columnMap, "Activity Name", "Description"),
            Stage = GetCellValue(worksheet, row, columnMap, "Stage") ?? "General",
            Status = GetCellValue(worksheet, row, columnMap, "Status") ?? "Planned"
        };

        // Determine parent based on indentation
        while (parentStack.Count > 0 && parentStack.Peek().IndentLevel >= indentLevel)
        {
            parentStack.Pop();
        }

        if (parentStack.Count > 0)
        {
            activity.ParentCode = parentStack.Peek().Activity.ActivityCode;
        }

        // Push current activity onto stack
        parentStack.Push((indentLevel, activity));

        return activity;
    }

    private string? GetCellValueRaw(ExcelWorksheet worksheet, int row, Dictionary<string, int> columnMap, params string[] possibleHeaders)
    {
        foreach (var header in possibleHeaders)
        {
            if (columnMap.TryGetValue(header, out int col))
            {
                return worksheet.Cells[row, col].Value?.ToString(); // Don't trim - preserve indentation
            }
        }
        return null;
    }

    private string? GetCellValue(ExcelWorksheet worksheet, int row, Dictionary<string, int> columnMap, params string[] possibleHeaders)
    {
        foreach (var header in possibleHeaders)
        {
            if (columnMap.TryGetValue(header, out int col))
            {
                var value = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }
        }
        return null;
    }

    private DateTime? GetDateValue(ExcelWorksheet worksheet, int row, Dictionary<string, int> columnMap, params string[] possibleHeaders)
    {
        foreach (var header in possibleHeaders)
        {
            if (columnMap.TryGetValue(header, out int col))
            {
                var cellValue = worksheet.Cells[row, col].Value;
                if (cellValue != null)
                {
                    if (cellValue is DateTime dateTime)
                    {
                        return dateTime;
                    }
                    if (DateTime.TryParse(cellValue.ToString(), out DateTime parsed))
                    {
                        return parsed;
                    }
                }
            }
        }
        return null;
    }

    private int? GetIntValue(ExcelWorksheet worksheet, int row, Dictionary<string, int> columnMap, params string[] possibleHeaders)
    {
        foreach (var header in possibleHeaders)
        {
            if (columnMap.TryGetValue(header, out int col))
            {
                var cellValue = worksheet.Cells[row, col].Value;
                if (cellValue != null)
                {
                    if (int.TryParse(cellValue.ToString(), out int parsed))
                    {
                        return parsed;
                    }
                }
            }
        }
        return null;
    }

    private async Task BuildHierarchyAndSaveAsync(
        List<ScheduleActivityImportDto> activities, 
        Guid? projectId, 
        ScheduleActivityImportResultDto result,
        CancellationToken cancellationToken)
    {
        // ✅ DELETE EXISTING ACTIVITIES FOR THIS PROJECT FIRST
        if (projectId.HasValue)
        {
            var existingActivities = await _context.ScheduleActivities
                .Where(a => a.ProjectId == projectId.Value)
                .ToListAsync(cancellationToken);
            
            if (existingActivities.Any())
            {
                _logger.LogInformation("Found {Count} existing activities for project. Deleting before import...", existingActivities.Count);
                
                // Delete related data first (materials, teams)
                var activityIds = existingActivities.Select(a => a.ScheduleActivityId).ToList();
                
                var materials = await _context.Set<ScheduleActivityMaterial>()
                    .Where(m => activityIds.Contains(m.ScheduleActivityId))
                    .ToListAsync(cancellationToken);
                
                var teams = await _context.Set<ScheduleActivityTeam>()
                    .Where(t => activityIds.Contains(t.ScheduleActivityId))
                    .ToListAsync(cancellationToken);
                
                _context.Set<ScheduleActivityMaterial>().RemoveRange(materials);
                _context.Set<ScheduleActivityTeam>().RemoveRange(teams);
                _context.ScheduleActivities.RemoveRange(existingActivities);
                
                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("Deleted {ActivityCount} activities, {MaterialCount} materials, {TeamCount} teams",
                    existingActivities.Count, materials.Count, teams.Count);
                
                result.Warnings.Add($"Replaced {existingActivities.Count} existing activities with new import.");
            }
        }
        
        var activityMap = new Dictionary<string, ScheduleActivity>(StringComparer.OrdinalIgnoreCase);
        var existingCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Empty now after deletion
        
        // Track duplicates to make unique codes
        var duplicateCounter = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        
        // Map original codes to their renamed unique codes (for parent lookup)
        var codeRenameMap = new Dictionary<int, string>(); // Index → UniqueCode

        // First pass: Create all activities without parent relationships
        for (int i = 0; i < activities.Count; i++)
        {
            var dto = activities[i];
            string originalCode = dto.ActivityCode;
            string uniqueCode = dto.ActivityCode;
            
            // Make code unique if it's a duplicate
            if (activityMap.ContainsKey(uniqueCode) || existingCodes.Contains(uniqueCode))
            {
                // Increment counter for this base code
                if (!duplicateCounter.ContainsKey(originalCode))
                {
                    duplicateCounter[originalCode] = 1;
                }
                else
                {
                    duplicateCounter[originalCode]++;
                }
                
                // Create unique code by appending counter
                uniqueCode = $"{originalCode}_{duplicateCounter[originalCode]}";
                
                _logger.LogInformation("Duplicate activity name detected: '{Original}' → renamed to '{Unique}'", 
                    originalCode, uniqueCode);
                
                result.Warnings.Add($"Duplicate activity '{originalCode}' renamed to '{uniqueCode}' to maintain uniqueness.");
            }
            
            // Store the renamed code for this activity's index
            codeRenameMap[i] = uniqueCode;
            
            // Update the DTO's ActivityCode
            dto.ActivityCode = uniqueCode;

            var activity = new ScheduleActivity
            {
                ScheduleActivityId = Guid.NewGuid(),
                ActivityCode = dto.ActivityCode,
                ActivityName = dto.ActivityName,
                Description = dto.Description,
                Stage = dto.Stage,
                StageNumber = 0,
                SequenceInStage = 0,
                OverallSequence = i + 1, // ✅ Preserve Excel file order
                EstimatedDurationDays = dto.OriginalDuration,
                PlannedStartDate = dto.PlannedStartDate ?? DateTime.UtcNow,
                PlannedFinishDate = dto.PlannedFinishDate ?? DateTime.UtcNow.AddDays(dto.OriginalDuration),
                Status = dto.Status,
                ProjectId = projectId,
                IsCustomActivity = true,
                CreatedDate = DateTime.UtcNow
            };

            activityMap[dto.ActivityCode] = activity;
            _context.ScheduleActivities.Add(activity);
        }

        // Update parent codes to match renamed activity codes
        for (int i = 0; i < activities.Count; i++)
        {
            var dto = activities[i];
            if (!string.IsNullOrWhiteSpace(dto.ParentCode))
            {
                // Find the parent's index by looking backwards for an activity with matching original code
                for (int j = i - 1; j >= 0; j--)
                {
                    var potentialParent = activities[j];
                    
                    // Check if this is the parent (by matching original code and having correct indent level)
                    if (codeRenameMap.TryGetValue(j, out var parentUniqueCode))
                    {
                        // If the parent's current code matches what we're looking for
                        if (potentialParent.ActivityCode == dto.ParentCode ||
                            potentialParent.ActivityCode.StartsWith(dto.ParentCode + "_", StringComparison.OrdinalIgnoreCase))
                        {
                            // Update parent code to the unique renamed code
                            dto.ParentCode = parentUniqueCode;
                            break;
                        }
                    }
                    else if (potentialParent.ActivityCode.Equals(dto.ParentCode, StringComparison.OrdinalIgnoreCase))
                    {
                        // Parent wasn't renamed, but verify it exists in the map
                        dto.ParentCode = potentialParent.ActivityCode;
                        break;
                    }
                }
            }
        }

        // Save first to get IDs
        await _context.SaveChangesAsync(cancellationToken);
        result.SuccessfullyImported = activityMap.Count;

        // Second pass: Set up parent-child relationships
        int relationshipsCreated = 0;
        int skippedNoParent = 0;
        int failedToFindParent = 0;
        
        _logger.LogInformation("Saved {Count} activities, now establishing parent-child relationships...", activityMap.Count);
        
        foreach (var dto in activities)
        {
            if (string.IsNullOrWhiteSpace(dto.ParentCode))
            {
                skippedNoParent++;
                continue; // Root level activity
            }

            if (!activityMap.TryGetValue(dto.ActivityCode, out var childActivity))
            {
                continue; // Activity was skipped
            }

            if (activityMap.TryGetValue(dto.ParentCode, out var parentActivity))
            {
                childActivity.ParentActivityId = parentActivity.ScheduleActivityId;
                _context.ScheduleActivities.Update(childActivity); // ✅ Explicitly mark as modified
                relationshipsCreated++;
                
                if (relationshipsCreated <= 20)
                {
                    _logger.LogInformation("  → Linked '{ChildCode}' to parent '{ParentCode}'", 
                        childActivity.ActivityCode, parentActivity.ActivityCode);
                }
            }
            else
            {
                // Parent not found in current import, try to find in existing database
                var existingParent = await _context.ScheduleActivities
                    .FirstOrDefaultAsync(a => a.ActivityCode == dto.ParentCode, cancellationToken);

                if (existingParent != null)
                {
                    childActivity.ParentActivityId = existingParent.ScheduleActivityId;
                    _context.ScheduleActivities.Update(childActivity); // ✅ Explicitly mark as modified
                    relationshipsCreated++;
                    
                    if (relationshipsCreated <= 20)
                    {
                        _logger.LogInformation("  → Linked '{ChildCode}' to existing parent '{ParentCode}'", 
                            childActivity.ActivityCode, existingParent.ActivityCode);
                    }
                }
                else
                {
                    failedToFindParent++;
                    if (failedToFindParent <= 10)
                    {
                        _logger.LogWarning("  ✗ Parent '{ParentCode}' not found for '{ChildCode}'", 
                            dto.ParentCode, dto.ActivityCode);
                    }
                    result.Warnings.Add(
                        $"Parent activity '{dto.ParentCode}' not found for '{dto.ActivityCode}'. Activity will be created as root level.");
                }
            }
        }

        _logger.LogInformation("Parent-child linking: {Created} linked, {Skipped} skipped (no parent), {Failed} failed (parent not found)", 
            relationshipsCreated, skippedNoParent, failedToFindParent);

        // Save parent-child relationships
        await _context.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// DTO for importing schedule activities from Excel
/// </summary>
internal class ScheduleActivityImportDto
{
    public string ActivityCode { get; set; } = string.Empty;
    public string ActivityName { get; set; } = string.Empty;
    public string? ParentCode { get; set; }
    public int IndentLevel { get; set; } = 0;
    public string? Description { get; set; }
    public string Stage { get; set; } = "General";
    public string Status { get; set; } = "Planned";
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedFinishDate { get; set; }
    public int OriginalDuration { get; set; } = 1;
}
