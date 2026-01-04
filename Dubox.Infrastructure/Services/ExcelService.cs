using Dubox.Domain.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Dubox.Infrastructure.Services;

public class ExcelService : IExcelService
{
    public ExcelService()
    {
        // Set the license context for EPPlus (NonCommercial or Commercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public byte[] GenerateTemplate<T>(string[] headers) where T : class
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Template");

        // Add headers
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = headers[i];
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Add some sample rows (optional)
        for (int row = 2; row <= 3; row++)
        {
            for (int col = 1; col <= headers.Length; col++)
            {
                worksheet.Cells[row, col].Value = $"Sample {headers[col - 1]}";
                worksheet.Cells[row, col].Style.Font.Italic = true;
                worksheet.Cells[row, col].Style.Font.Color.SetColor(Color.Gray);
            }
        }

        // Auto-fit columns
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }

    public byte[] GenerateTemplateWithReference<T>(string[] headers, Dictionary<string, List<string>> referenceData, Dictionary<string, List<string>> groupedReferenceData, string projectCode, string sheetName) where T : class
    {
        using var package = new ExcelPackage();
        
        // Create main data entry sheet
        var dataSheet = package.Workbook.Worksheets.Add("Data Entry");

        // Add headers with styling
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = dataSheet.Cells[1, i + 1];
            cell.Value = headers[i];
            
            // Set pattern type FIRST before any color operations
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189)); // Professional blue
            
            // Now set other styling
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.White);
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            // Add comment to Box Tag column to explain it's auto-filled
            if (headers[i].Contains("Box Tag"))
            {
                cell.AddComment("This column shows a PREVIEW of the Box Tag.\n\n" +
                               "Format: Project-Building-Floor-Type-SubType\n\n" +
                               "The actual Box Tag will be generated during import using abbreviations from your project configuration.\n\n" +
                               "Example: 169-B01-FF-S1-B",
                               "System");
                cell.Comment.AutoFit = true;
            }
        }

        // Add instruction row
        var instructionRow = 2;
        var instructionCell = dataSheet.Cells[instructionRow, 1];
        instructionCell.Value = $"⚠️ INSTRUCTIONS: Row 3 contains an example – please delete it before importing. Enter your data starting from the next row. Box Tag is AUTO-GENERATED during import using the format: {projectCode}-Building-Floor-Type-SubType. REQUIRED fields: Box Type and Floor. Use values from Reference Data sheets. Make sure to delete rows 2 and 3 before importing.";
        
        // Set pattern type FIRST before color operations on the first cell
        instructionCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        instructionCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204)); // Light yellow
        
        // Now set other styling
        instructionCell.Style.Font.Bold = true;
        instructionCell.Style.Font.Size = 10;
        instructionCell.Style.WrapText = true;
        dataSheet.Row(instructionRow).Height = 50;
        
        // Merge cells AFTER all styling is applied
        dataSheet.Cells[instructionRow, 1, instructionRow, headers.Length].Merge = true;

        // Freeze top rows
        dataSheet.View.FreezePanes(3, 1);

        // Auto-fit columns
        for (int i = 1; i <= headers.Length; i++)
        {
            dataSheet.Column(i).Width = 20;
        }
        
        // Make the last column (Box Tag) wider and add note
        dataSheet.Column(headers.Length).Width = 35;
        
        // Add example data row at row 3 to show how Box Tag will look
        var exampleRow = 3;
        
        // Column indices based on headers: BoxName=1, BoxType=2, BoxSubType=3, Floor=4, BuildingNumber=5, etc.
        dataSheet.Cells[exampleRow, 1].Value = "Kitchen Cabinet Box"; // Box Name
        dataSheet.Cells[exampleRow, 2].Value = "ElectricalPanel"; // Box Type (example)
        dataSheet.Cells[exampleRow, 3].Value = "MainPanel"; // Box Sub Type (example)
        dataSheet.Cells[exampleRow, 4].Value = "GF"; // Floor (example)
        dataSheet.Cells[exampleRow, 5].Value = "B01"; // Building Number (example)
        dataSheet.Cells[exampleRow, 6].Value = "Kitchen"; // Box Function (example)
        dataSheet.Cells[exampleRow, 7].Value = "Zone 01"; // Zone (example)
        dataSheet.Cells[exampleRow, 8].Value = 6000; // Length
        dataSheet.Cells[exampleRow, 9].Value = 3000; // Width
        dataSheet.Cells[exampleRow, 10].Value = 2800; // Height
        dataSheet.Cells[exampleRow, 11].Value = "Example box"; // Notes
        
        // Box Tag preview (manual concatenation for the example)
        var exampleBoxTag = $"{projectCode}-B01-GF-ElectricalPanel-MainPanel";
        var boxTagCell = dataSheet.Cells[exampleRow, headers.Length];
        boxTagCell.Value = exampleBoxTag;
        
        // Set pattern type FIRST before any color operations
        boxTagCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        boxTagCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 250, 205)); // Lemon chiffon
        
        // Now set font styling
        boxTagCell.Style.Font.Bold = true;
        boxTagCell.Style.Font.Color.SetColor(Color.FromArgb(184, 134, 11)); // Dark goldenrod
        
        // Style the example row
        for (int col = 1; col < headers.Length; col++)
        {
            var exampleCell = dataSheet.Cells[exampleRow, col];
            
            // Set pattern type FIRST before any color operations
            exampleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            exampleCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 248, 255)); // Alice blue
            
            // Now set font styling
            exampleCell.Style.Font.Italic = true;
            exampleCell.Style.Font.Color.SetColor(Color.FromArgb(105, 105, 105)); // Dim gray
        }
        
        // Add note to Box Tag column for subsequent rows
        for (int row = exampleRow + 1; row <= exampleRow + 5; row++)
        {
            var noteCell = dataSheet.Cells[row, headers.Length];
            noteCell.Value = "(auto-generated on import)";
            
            // Set pattern type FIRST before any color operations
            noteCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            noteCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(245, 245, 245)); // White smoke
            
            // Now set font styling
            noteCell.Style.Font.Italic = true;
            noteCell.Style.Font.Size = 9;
            noteCell.Style.Font.Color.SetColor(Color.FromArgb(169, 169, 169)); // Dark gray
            noteCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Create reference data sheets
        foreach (var reference in referenceData)
        {
            if (reference.Value == null || reference.Value.Count == 0)
                continue;

            var refSheet = package.Workbook.Worksheets.Add($"Ref: {reference.Key}");
            
            // Add header
            var refHeaderCell = refSheet.Cells[1, 1];
            refHeaderCell.Value = reference.Key;
            
            // Set pattern type FIRST before any color operations
            refHeaderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            refHeaderCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80)); // Green
            
            // Now set font styling
            refHeaderCell.Style.Font.Bold = true;
            refHeaderCell.Style.Font.Color.SetColor(Color.White);
            refHeaderCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Add description
            var descCell = refSheet.Cells[1, 2];
            descCell.Value = $"Valid values configured for this project";
            descCell.Style.Font.Italic = true;
            descCell.Style.Font.Color.SetColor(Color.Gray);

            // Add values
            for (int i = 0; i < reference.Value.Count; i++)
            {
                refSheet.Cells[i + 2, 1].Value = reference.Value[i];
                refSheet.Cells[i + 2, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            refSheet.Column(1).Width = 30;
            refSheet.Column(2).Width = 40;
        }

        // Create grouped reference data sheets (for Box Sub Types grouped by Box Type)
        if (groupedReferenceData != null && groupedReferenceData.Any())
        {
            var subTypesSheet = package.Workbook.Worksheets.Add("Ref: Box Sub Types");
            
            // Add header - style each cell individually to avoid EPPlus range styling issues
            var subTypeHeader1 = subTypesSheet.Cells[1, 1];
            subTypeHeader1.Value = "Box Type";
            subTypeHeader1.Style.Fill.PatternType = ExcelFillStyle.Solid;
            subTypeHeader1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80)); // Green
            subTypeHeader1.Style.Font.Bold = true;
            subTypeHeader1.Style.Font.Color.SetColor(Color.White);
            subTypeHeader1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            subTypeHeader1.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            var subTypeHeader2 = subTypesSheet.Cells[1, 2];
            subTypeHeader2.Value = "Box Sub Type";
            subTypeHeader2.Style.Fill.PatternType = ExcelFillStyle.Solid;
            subTypeHeader2.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80)); // Green
            subTypeHeader2.Style.Font.Bold = true;
            subTypeHeader2.Style.Font.Color.SetColor(Color.White);
            subTypeHeader2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            subTypeHeader2.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            int currentRow = 2;
            foreach (var group in groupedReferenceData.OrderBy(g => g.Key))
            {
                var boxTypeName = group.Key;
                var subTypes = group.Value;

                foreach (var subType in subTypes)
                {
                    subTypesSheet.Cells[currentRow, 1].Value = boxTypeName;
                    subTypesSheet.Cells[currentRow, 2].Value = subType;
                    subTypesSheet.Cells[currentRow, 1, currentRow, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    currentRow++;
                }
            }

            subTypesSheet.Column(1).Width = 30;
            subTypesSheet.Column(2).Width = 30;
        }

        return package.GetAsByteArray();
    }

    public async Task<List<T>> ReadFromExcelAsync<T>(Stream fileStream, Func<Dictionary<string, object?>, T> mapper) where T : class
    {
        var result = new List<T>();

        using var package = new ExcelPackage(fileStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        if (worksheet == null || worksheet.Dimension == null)
        {
            return result;
        }

        var colCount = worksheet.Dimension.Columns;

        // Read headers from first row
        var headers = new Dictionary<int, string>();
        for (int col = 1; col <= colCount; col++)
        {
            var headerValue = worksheet.Cells[1, col].Value?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(headerValue))
            {
                headers[col] = headerValue;
            }
        }

        // Find the actual last row with data (instead of using Dimension.Rows which includes empty formatted rows)
        // Start from the end row and work backwards to find the last row that actually contains meaningful data
        // Ignore rows that only have placeholder text like "(auto-generated on import)"
        int lastRowWithData = 2; // Default to row 2 (after header), will be updated if data found
        int maxRow = worksheet.Dimension.End.Row;
        string[] placeholderTexts = { "(auto-generated on import)", "auto-generated on import" };
        
        for (int row = maxRow; row >= 3; row--)
        {
            bool hasMeaningfulData = false;
            
            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value;
                if (cellValue != null)
                {
                    var cellText = cellValue.ToString()?.Trim() ?? string.Empty;
                    // Check if it's not empty and not just placeholder text
                    if (!string.IsNullOrWhiteSpace(cellText))
                    {
                        // Check if this cell contains placeholder text
                        bool isPlaceholder = placeholderTexts.Any(placeholder => 
                            cellText.Equals(placeholder, StringComparison.OrdinalIgnoreCase));
                        
                        if (!isPlaceholder)
                        {
                            hasMeaningfulData = true;
                            break; // Found meaningful data, this row should be processed
                        }
                    }
                }
            }
            
            if (hasMeaningfulData)
            {
                lastRowWithData = row;
                break;
            }
        }

        // Read data rows (skip header row and instruction row, start from row 3)
        // Ignore rows that only contain placeholder text
        
        for (int row = 2; row <= lastRowWithData; row++)
        {
            var rowData = new Dictionary<string, object?>();
            bool hasMeaningfulData = false;

            for (int col = 1; col <= colCount; col++)
            {
                if (headers.ContainsKey(col))
                {
                    var cellValue = worksheet.Cells[row, col].Value;
                    rowData[headers[col]] = cellValue;

                    if (cellValue != null)
                    {
                        var cellText = cellValue.ToString()?.Trim() ?? string.Empty;
                        // Check if it's not empty and not just placeholder text
                        if (!string.IsNullOrWhiteSpace(cellText))
                        {
                            bool isPlaceholder = placeholderTexts.Any(placeholder => 
                                cellText.Equals(placeholder, StringComparison.OrdinalIgnoreCase));
                            
                            if (!isPlaceholder)
                            {
                                hasMeaningfulData = true;
                            }
                        }
                    }
                }
            }

            // Only process rows that have meaningful data (not just placeholder text)
            if (hasMeaningfulData)
            {
                var mappedObject = mapper(rowData);
                result.Add(mappedObject);
            }
        }

        return await Task.FromResult(result);
    }

    public byte[] ExportToExcel<T>(List<T> data, string[] headers) where T : class
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Data");

        // Add headers
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = headers[i];
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
        }

        // Add data rows
        for (int row = 0; row < data.Count; row++)
        {
            var item = data[row];
            var properties = typeof(T).GetProperties();

            for (int col = 0; col < Math.Min(properties.Length, headers.Length); col++)
            {
                var value = properties[col].GetValue(item);
                worksheet.Cells[row + 2, col + 1].Value = value;
            }
        }

        // Auto-fit columns
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }

    public async Task<(bool IsValid, List<string> Errors)> ValidateExcelStructureAsync(Stream fileStream, string[] requiredHeaders)
    {
        var errors = new List<string>();

        try
        {
            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                errors.Add("No worksheet found in the Excel file.");
                return (false, errors);
            }

            if (worksheet.Dimension == null)
            {
                errors.Add("The worksheet is empty.");
                return (false, errors);
            }

            var colCount = worksheet.Dimension.Columns;
            var actualHeaders = new List<string>();

            // Read actual headers
            for (int col = 1; col <= colCount; col++)
            {
                var headerValue = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(headerValue))
                {
                    actualHeaders.Add(headerValue);
                }
            }

            // Check if all required headers are present
            foreach (var requiredHeader in requiredHeaders)
            {
                if (!actualHeaders.Contains(requiredHeader, StringComparer.OrdinalIgnoreCase))
                {
                    errors.Add($"Missing required column: {requiredHeader}");
                }
            }

            // Check if there's at least one data row
            if (worksheet.Dimension.Rows < 2)
            {
                errors.Add("The Excel file must contain at least one data row (besides the header).");
            }

            return (errors.Count == 0, errors);
        }
        catch (Exception ex)
        {
            errors.Add($"Error reading Excel file: {ex.Message}");
            return (false, errors);
        }
    }
}

