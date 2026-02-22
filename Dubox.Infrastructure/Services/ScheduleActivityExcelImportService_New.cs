using Dubox.Domain.Entities;
using Dubox.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Dubox.Infrastructure.Services;

/// <summary>
/// Service for importing Schedule Activities from Excel files with INDENTATION-based hierarchical structure
/// </summary>
public class ScheduleActivityExcelImportService_New
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ScheduleActivityExcelImportService_New> _logger;

    public ScheduleActivityExcelImportService_New(
        ApplicationDbContext context,
        ILogger<ScheduleActivityExcelImportService_New> logger)
    {
        _context = context;
        _logger = logger;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<ScheduleActivityImportResult> ImportFromExcelAsync(string filePath, Guid? projectId = null, string? sheetName = null)
    {
        var result = new ScheduleActivityImportResult();

        try
        {
            if (!File.Exists(filePath))
            {
                result.Errors.Add($"File not found: {filePath}");
                return result;
            }

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = string.IsNullOrWhiteSpace(sheetName)
                ? package.Workbook.Worksheets.FirstOrDefault()
                : package.Workbook.Worksheets[sheetName];

            if (worksheet == null || worksheet.Dimension == null)
            {
                result.Errors.Add("The worksheet is empty or not found.");
                return result;
            }

            _logger.LogInformation("Importing from sheet: {SheetName}", worksheet.Name);

            // Parse with indentation-based hierarchy
            var activities = ParseExcelDataWithIndentation(worksheet, result);

            if (result.Errors.Any())
            {
                return result;
            }

            // Save to database
            await SaveActivitiesWithHierarchy(activities, projectId, result);

            result.TotalProcessed = activities.Count;
            _logger.LogInformation("Import completed: {Total} activities processed", result.TotalProcessed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing schedule activities");
            result.Errors.Add($"Import failed: {ex.Message}");
        }

        return result;
    }

    private List<ActivityWithHierarchy> ParseExcelDataWithIndentation(ExcelWorksheet worksheet, ScheduleActivityImportResult result)
    {
        var activities = new List<ActivityWithHierarchy>();
        var rowCount = worksheet.Dimension.End.Row;

        // Find header row (row with "Activity ID" or "Activity Name")
        int headerRow = FindHeaderRow(worksheet);
        _logger.LogInformation("Header row found at: {Row}", headerRow);

        // Build column mapping
        var columnMap = BuildColumnMapping(worksheet, headerRow);
        _logger.LogInformation("Columns: {Columns}", string.Join(", ", columnMap.Keys));

        // Track parent stack by indentation level
        var parentStack = new Stack<(int IndentLevel, ActivityWithHierarchy Activity)>();

        // Process data rows
        int processedCount = 0;
        for (int row = headerRow + 1; row <= rowCount; row++)
        {
            try
            {
                var activity = ParseRowWithIndentation(worksheet, row, columnMap, parentStack);
                if (activity != null)
                {
                    activities.Add(activity);
                    processedCount++;

                    if (processedCount % 100 == 0)
                    {
                        _logger.LogInformation("Processed {Count} rows...", processedCount);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Warnings.Add($"Row {row}: {ex.Message}");
            }
        }

        _logger.LogInformation("Parsed {Count} activities with indentation-based hierarchy", activities.Count);
        return activities;
    }

    private int FindHeaderRow(ExcelWorksheet worksheet)
    {
        for (int row = 1; row <= Math.Min(10, worksheet.Dimension.End.Row); row++)
        {
            for (int col = 1; col <= Math.Min(5, worksheet.Dimension.End.Column); col++)
            {
                var cell = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                if (cell != null && (
                    cell.Equals("Activity ID", StringComparison.OrdinalIgnoreCase) ||
                    cell.Equals("Activity Code", StringComparison.OrdinalIgnoreCase) ||
                    cell.Equals("Activity Name", StringComparison.OrdinalIgnoreCase)))
                {
                    return row;
                }
            }
        }
        return 1;
    }

    private Dictionary<string, int> BuildColumnMapping(ExcelWorksheet worksheet, int headerRow)
    {
        var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var colCount = worksheet.Dimension.End.Column;

        for (int col = 1; col <= colCount; col++)
        {
            var header = worksheet.Cells[headerRow, col].Value?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(header))
            {
                columnMap[header] = col;
            }
        }

        return columnMap;
    }

    private ActivityWithHierarchy? ParseRowWithIndentation(
        ExcelWorksheet worksheet, 
        int row, 
        Dictionary<string, int> columnMap,
        Stack<(int IndentLevel, ActivityWithHierarchy Activity)> parentStack)
    {
        // Get Activity ID (which may have indentation)
        var activityIdRaw = GetCellValueRaw(worksheet, row, columnMap, "Activity ID", "Activity Code");
        if (string.IsNullOrWhiteSpace(activityIdRaw))
        {
            return null; // Skip empty rows
        }

        // Calculate indentation level - try multiple methods
        int indentLevel = 0;
        string activityCode = activityIdRaw.Trim();
        
        // Method 1: Check for leading spaces in the text
        int leadingSpaces = activityIdRaw.TakeWhile(c => c == ' ').Count();
        if (leadingSpaces > 0)
        {
            indentLevel = leadingSpaces / 2;
        }
        else
        {
            // Method 2: Check Excel's Style.Indent property (for formatted cells)
            if (columnMap.TryGetValue("Activity ID", out int activityIdCol) || 
                columnMap.TryGetValue("Activity Code", out activityIdCol))
            {
                var cell = worksheet.Cells[row, activityIdCol];
                indentLevel = cell.Style.Indent;
            }
        }
        
        // Diagnostic logging for first 30 rows
        if (row <= 35)
        {
            _logger.LogInformation("Row {Row}: Spaces={Spaces}, StyleIndent={StyleIndent}, FinalLevel={Level}, Code='{Code}'",
                row, leadingSpaces, 
                columnMap.ContainsKey("Activity ID") ? worksheet.Cells[row, columnMap["Activity ID"]].Style.Indent : 0,
                indentLevel, activityCode);
        }

        // Skip rows that don't have a proper activity code (summary rows without IDs)
        if (string.IsNullOrWhiteSpace(activityCode) || activityCode.Length < 3)
        {
            return null;
        }

        var activityName = GetCellValue(worksheet, row, columnMap, "Activity Name") ?? activityCode;
        
        var activity = new ActivityWithHierarchy
        {
            ActivityCode = activityCode,
            ActivityName = activityName,
            IndentLevel = indentLevel,
            Description = GetCellValue(worksheet, row, columnMap, "Description"),
            OriginalDuration = GetIntValue(worksheet, row, columnMap, "Original Duration", "Duration") ?? 1,
            PlannedStartDate = GetDateValue(worksheet, row, columnMap, "BL1 Start", "Start", "Planned Start"),
            PlannedFinishDate = GetDateValue(worksheet, row, columnMap, "BL1 Finish", "Finish", "Planned Finish"),
            Status = "Planned",
            Stage = "General"
        };

        // Determine parent based on indentation
        while (parentStack.Count > 0 && parentStack.Peek().IndentLevel >= indentLevel)
        {
            parentStack.Pop();
        }

        if (parentStack.Count > 0)
        {
            activity.ParentActivity = parentStack.Peek().Activity;
        }

        // Push current activity onto stack
        parentStack.Push((indentLevel, activity));

        return activity;
    }

    private async Task SaveActivitiesWithHierarchy(
        List<ActivityWithHierarchy> activities,
        Guid? projectId,
        ScheduleActivityImportResult result)
    {
        var existingCodes = new HashSet<string>(
            await _context.ScheduleActivities.Select(a => a.ActivityCode).ToListAsync(),
            StringComparer.OrdinalIgnoreCase);

        var activityMap = new Dictionary<string, ScheduleActivity>(StringComparer.OrdinalIgnoreCase);

        // First pass: Create all activities
        foreach (var dto in activities)
        {
            if (existingCodes.Contains(dto.ActivityCode))
            {
                result.Warnings.Add($"Activity '{dto.ActivityCode}' already exists. Skipping.");
                result.Skipped++;
                continue;
            }

            var entity = new ScheduleActivity
            {
                ScheduleActivityId = Guid.NewGuid(),
                ActivityCode = dto.ActivityCode,
                ActivityName = dto.ActivityName,
                Description = dto.Description,
                Stage = dto.Stage,
                StageNumber = 0,
                SequenceInStage = 0,
                OverallSequence = 0,
                EstimatedDurationDays = dto.OriginalDuration,
                PlannedStartDate = dto.PlannedStartDate ?? DateTime.UtcNow,
                PlannedFinishDate = dto.PlannedFinishDate ?? DateTime.UtcNow.AddDays(dto.OriginalDuration),
                Status = dto.Status,
                ProjectId = projectId,
                IsCustomActivity = true,
                CreatedDate = DateTime.UtcNow
            };

            activityMap[dto.ActivityCode] = entity;
            _context.ScheduleActivities.Add(entity);
        }

        // Save to get IDs
        await _context.SaveChangesAsync();
        result.SuccessfullyImported = activityMap.Count;

        _logger.LogInformation("Saved {Count} activities, now establishing parent-child relationships...", activityMap.Count);

        // Second pass: Set parent relationships
        int relationshipsCreated = 0;
        int skippedNoParent = 0;
        int failedToFindParent = 0;
        
        foreach (var dto in activities)
        {
            if (!activityMap.TryGetValue(dto.ActivityCode, out var childEntity))
            {
                continue;
            }

            if (dto.ParentActivity != null)
            {
                if (activityMap.TryGetValue(dto.ParentActivity.ActivityCode, out var parentEntity))
                {
                    childEntity.ParentActivityId = parentEntity.ScheduleActivityId;
                    _context.ScheduleActivities.Update(childEntity); // Explicitly mark as modified
                    relationshipsCreated++;
                    
                    if (relationshipsCreated <= 20)
                    {
                        _logger.LogInformation("  → Linked '{ChildCode}' to parent '{ParentCode}'", 
                            childEntity.ActivityCode, parentEntity.ActivityCode);
                    }
                }
                else
                {
                    failedToFindParent++;
                    if (failedToFindParent <= 10)
                    {
                        _logger.LogWarning("  ✗ Could not find parent '{ParentCode}' for child '{ChildCode}'", 
                            dto.ParentActivity.ActivityCode, dto.ActivityCode);
                    }
                }
            }
            else
            {
                skippedNoParent++;
            }
        }

        _logger.LogInformation("Parent-child linking: {Created} linked, {Skipped} skipped (no parent), {Failed} failed (parent not found)", 
            relationshipsCreated, skippedNoParent, failedToFindParent);
            
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created {Count} parent-child relationships", relationshipsCreated);
    }

    // Helper methods
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
                    if (cellValue is DateTime dateTime) return dateTime;
                    if (DateTime.TryParse(cellValue.ToString(), out DateTime parsed)) return parsed;
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
                    if (int.TryParse(cellValue.ToString(), out int parsed)) return parsed;
                }
            }
        }
        return null;
    }
}

public class ScheduleActivityImportResult
{
    public int TotalProcessed { get; set; }
    public int SuccessfullyImported { get; set; }
    public int Skipped { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public bool IsSuccess => !Errors.Any();
}
