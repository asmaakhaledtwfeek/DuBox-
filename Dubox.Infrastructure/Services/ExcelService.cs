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

    public async Task<List<T>> ReadFromExcelAsync<T>(Stream fileStream, Func<Dictionary<string, object?>, T> mapper) where T : class
    {
        var result = new List<T>();

        using var package = new ExcelPackage(fileStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        if (worksheet == null || worksheet.Dimension == null)
        {
            return result;
        }

        var rowCount = worksheet.Dimension.Rows;
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

        // Read data rows (skip header row)
        for (int row = 2; row <= rowCount; row++)
        {
            var rowData = new Dictionary<string, object?>();
            bool hasData = false;

            for (int col = 1; col <= colCount; col++)
            {
                if (headers.ContainsKey(col))
                {
                    var cellValue = worksheet.Cells[row, col].Value;
                    rowData[headers[col]] = cellValue;

                    if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                    {
                        hasData = true;
                    }
                }
            }

            // Only process rows that have at least some data
            if (hasData)
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

