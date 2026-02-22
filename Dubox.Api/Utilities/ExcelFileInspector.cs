using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Dubox.Api.Utilities;

/// <summary>
/// Utility to inspect Excel file structure and preview data
/// </summary>
public static class ExcelFileInspector
{
    /// <summary>
    /// Inspects an Excel file and logs its structure and sample data
    /// </summary>
    public static void InspectExcelFile(string filePath, ILogger logger, int previewRows = 10, string? sheetName = null)
    {
        if (!File.Exists(filePath))
        {
            logger.LogError("File not found: {Path}", filePath);
            return;
        }

        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using var package = new ExcelPackage(new FileInfo(filePath));
            
            logger.LogInformation("═══════════════════════════════════════════════════════════════");
            logger.LogInformation("📊 EXCEL FILE INSPECTION REPORT");
            logger.LogInformation("═══════════════════════════════════════════════════════════════");
            logger.LogInformation("File: {FileName}", Path.GetFileName(filePath));
            logger.LogInformation("Path: {Path}", filePath);
            logger.LogInformation("Size: {Size:N0} bytes", new FileInfo(filePath).Length);
            logger.LogInformation("───────────────────────────────────────────────────────────────");
            
            logger.LogInformation("📑 WORKSHEETS ({Count} sheets):", package.Workbook.Worksheets.Count);
            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
            {
                var sheet = package.Workbook.Worksheets[i];
                var rowCount = sheet.Dimension?.Rows ?? 0;
                var colCount = sheet.Dimension?.Columns ?? 0;
                logger.LogInformation("  {Index}. '{Name}' - {Rows} rows × {Cols} columns", 
                    i + 1, sheet.Name, rowCount, colCount);
            }
            
            // Inspect the specified sheet or first sheet
            var worksheet = string.IsNullOrWhiteSpace(sheetName)
                ? package.Workbook.Worksheets.FirstOrDefault()
                : package.Workbook.Worksheets[sheetName];
                
            if (worksheet == null || worksheet.Dimension == null)
            {
                logger.LogWarning("Worksheet is empty or not found");
                return;
            }

            logger.LogInformation("───────────────────────────────────────────────────────────────");
            logger.LogInformation("📋 INSPECTING SHEET: '{SheetName}'", worksheet.Name);
            logger.LogInformation("Dimensions: {Rows} rows × {Cols} columns", 
                worksheet.Dimension.Rows, worksheet.Dimension.Columns);
            
            // Find header row
            int headerRow = FindHeaderRow(worksheet);
            logger.LogInformation("Header row detected at: Row {Row}", headerRow);
            logger.LogInformation("───────────────────────────────────────────────────────────────");
            
            // Display columns
            var colCount2 = worksheet.Dimension.End.Column;
            logger.LogInformation("📌 COLUMNS ({Count}):", colCount2);
            
            var headers = new List<string>();
            for (int col = 1; col <= colCount2; col++)
            {
                var header = worksheet.Cells[headerRow, col].Value?.ToString()?.Trim() ?? $"Column{col}";
                headers.Add(header);
                
                // Sample first few values to understand data type
                var sampleValues = new List<string>();
                for (int row = headerRow + 1; row <= Math.Min(headerRow + 3, worksheet.Dimension.End.Row); row++)
                {
                    var value = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                    if (!string.IsNullOrWhiteSpace(value) && value.Length <= 50)
                    {
                        sampleValues.Add(value);
                    }
                }
                
                var sample = sampleValues.Any() 
                    ? $" (e.g., {string.Join(", ", sampleValues.Take(2))})" 
                    : " (empty)";
                
                logger.LogInformation("  {Index,3}. [{Letter}] {Header,-40} {Sample}", 
                    col, GetExcelColumnName(col), header, sample);
            }
            
            logger.LogInformation("───────────────────────────────────────────────────────────────");
            logger.LogInformation("📊 DATA PREVIEW (First {Count} rows):", Math.Min(previewRows, worksheet.Dimension.Rows - headerRow));
            logger.LogInformation("");
            
            // Display header
            var headerLine = string.Join(" | ", headers.Select((h, i) => $"{h,-20}".Substring(0, Math.Min(20, h.Length))));
            logger.LogInformation(headerLine);
            logger.LogInformation(new string('─', Math.Min(150, headerLine.Length)));
            
            // Display data rows
            for (int row = headerRow + 1; row <= Math.Min(headerRow + previewRows, worksheet.Dimension.End.Row); row++)
            {
                var rowValues = new List<string>();
                for (int col = 1; col <= colCount2; col++)
                {
                    var value = worksheet.Cells[row, col].Value?.ToString()?.Trim() ?? "";
                    rowValues.Add($"{value,-20}".Substring(0, Math.Min(20, value.Length)));
                }
                logger.LogInformation(string.Join(" | ", rowValues));
            }
            
            logger.LogInformation("───────────────────────────────────────────────────────────────");
            logger.LogInformation("📈 STATISTICS:");
            logger.LogInformation("  Total rows: {Total}", worksheet.Dimension.Rows);
            logger.LogInformation("  Header row: {Header}", headerRow);
            logger.LogInformation("  Data rows: {Data}", worksheet.Dimension.Rows - headerRow);
            logger.LogInformation("  Total columns: {Cols}", colCount2);
            
            // Analyze specific columns relevant to schedule activities
            logger.LogInformation("───────────────────────────────────────────────────────────────");
            logger.LogInformation("🔍 RELEVANT COLUMNS FOR SCHEDULE ACTIVITY IMPORT:");
            
            var relevantColumns = new Dictionary<string, string[]>
            {
                ["Activity Code"] = new[] { "Activity ID", "Activity Code", "ActivityCode", "Task ID" },
                ["Activity Name"] = new[] { "Activity Name", "ActivityName", "Task Name", "Description" },
                ["Parent Code"] = new[] { "Parent Activity", "Parent Code", "ParentCode", "Parent ID" },
                ["Start Date"] = new[] { "BL1 Start", "Start", "Planned Start", "Start Date", "Begin" },
                ["Finish Date"] = new[] { "BL1 Finish", "Finish", "Planned Finish", "Finish Date", "End" },
                ["Duration"] = new[] { "Original Duration", "Duration", "Days" },
                ["Stage"] = new[] { "Stage", "Phase", "Milestone" },
                ["Status"] = new[] { "Status", "State" }
            };
            
            foreach (var mapping in relevantColumns)
            {
                var found = false;
                for (int col = 1; col <= colCount2; col++)
                {
                    var header = worksheet.Cells[headerRow, col].Value?.ToString()?.Trim() ?? "";
                    if (mapping.Value.Any(possible => header.Equals(possible, StringComparison.OrdinalIgnoreCase)))
                    {
                        logger.LogInformation("  ✓ {Purpose,-15} → Column {Col} '{Header}'", mapping.Key, col, header);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    logger.LogWarning("  ✗ {Purpose,-15} → NOT FOUND (looking for: {Possible})", 
                        mapping.Key, string.Join(", ", mapping.Value));
                }
            }
            
            logger.LogInformation("═══════════════════════════════════════════════════════════════");
            logger.LogInformation("✅ Inspection complete!");
            logger.LogInformation("═══════════════════════════════════════════════════════════════");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inspecting Excel file: {Message}", ex.Message);
        }
    }

    private static int FindHeaderRow(ExcelWorksheet worksheet)
    {
        // Scan first 10 rows to find the header row
        for (int row = 1; row <= Math.Min(10, worksheet.Dimension.Rows); row++)
        {
            var firstCell = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
            if (firstCell != null && (
                firstCell.Contains("Activity", StringComparison.OrdinalIgnoreCase) ||
                firstCell.Contains("Task", StringComparison.OrdinalIgnoreCase) ||
                firstCell.Contains("ID", StringComparison.OrdinalIgnoreCase) ||
                firstCell.Contains("Code", StringComparison.OrdinalIgnoreCase)))
            {
                return row;
            }
        }
        return 1; // Default to first row
    }

    private static string GetExcelColumnName(int columnNumber)
    {
        int dividend = columnNumber;
        string columnName = string.Empty;

        while (dividend > 0)
        {
            int modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo) + columnName;
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }

    /// <summary>
    /// Exports Excel structure to a markdown file for documentation
    /// </summary>
    public static void ExportStructureToMarkdown(string excelPath, string outputPath, ILogger logger)
    {
        if (!File.Exists(excelPath))
        {
            logger.LogError("File not found: {Path}", excelPath);
            return;
        }

        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var md = new System.Text.StringBuilder();
            
            using var package = new ExcelPackage(new FileInfo(excelPath));
            
            md.AppendLine($"# Excel File Structure: {Path.GetFileName(excelPath)}");
            md.AppendLine();
            md.AppendLine($"**File Path**: `{excelPath}`  ");
            md.AppendLine($"**File Size**: {new FileInfo(excelPath).Length:N0} bytes  ");
            md.AppendLine($"**Worksheets**: {package.Workbook.Worksheets.Count}  ");
            md.AppendLine();
            
            foreach (var sheet in package.Workbook.Worksheets)
            {
                if (sheet.Dimension == null) continue;
                
                md.AppendLine($"## Sheet: {sheet.Name}");
                md.AppendLine();
                md.AppendLine($"- **Dimensions**: {sheet.Dimension.Rows} rows × {sheet.Dimension.Columns} columns");
                md.AppendLine();
                
                int headerRow = FindHeaderRow(sheet);
                md.AppendLine($"### Columns (Header at row {headerRow})");
                md.AppendLine();
                md.AppendLine("| # | Column | Header | Sample Data |");
                md.AppendLine("|---|--------|--------|-------------|");
                
                for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                {
                    var header = sheet.Cells[headerRow, col].Value?.ToString()?.Trim() ?? $"Column{col}";
                    var sample = sheet.Cells[headerRow + 1, col].Value?.ToString()?.Trim() ?? "";
                    if (sample.Length > 50) sample = sample.Substring(0, 50) + "...";
                    
                    md.AppendLine($"| {col} | {GetExcelColumnName(col)} | {header} | {sample} |");
                }
                
                md.AppendLine();
            }
            
            File.WriteAllText(outputPath, md.ToString());
            logger.LogInformation("Excel structure exported to: {Path}", outputPath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error exporting structure: {Message}", ex.Message);
        }
    }
}
