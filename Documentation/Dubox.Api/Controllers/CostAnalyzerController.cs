using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Dubox.Api.Controllers;

/// <summary>
/// Utility controller to analyze Excel files and understand their structure
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CostAnalyzerController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CostAnalyzerController> _logger;

    public CostAnalyzerController(
        IWebHostEnvironment environment,
        ILogger<CostAnalyzerController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    /// <summary>
    /// Analyze an Excel file structure to understand its format
    /// </summary>
    [HttpGet("analyze")]
    public IActionResult AnalyzeExcelFile([FromQuery] string fileName)
    {
        try
        {
            var filePath = Path.Combine(_environment.WebRootPath, "data", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File not found: {fileName}");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(new FileInfo(filePath));
            
            var analysis = new
            {
                fileName,
                fileSize = new FileInfo(filePath).Length,
                numberOfSheets = package.Workbook.Worksheets.Count,
                sheets = package.Workbook.Worksheets.Select((sheet, index) => new
                {
                    sheetIndex = index,
                    sheetName = sheet.Name,
                    rowCount = sheet.Dimension?.Rows ?? 0,
                    columnCount = sheet.Dimension?.Columns ?? 0,
                    hasData = sheet.Dimension != null,
                    headers = GetHeaders(sheet, 1), // Assuming row 1 is headers
                    sampleData = GetSampleRows(sheet, 2, 5) // Get 5 sample rows starting from row 2
                }).ToList()
            };

            return Ok(analysis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing Excel file: {FileName}", fileName);
            return StatusCode(500, new { message = "Error analyzing file", error = ex.Message });
        }
    }

    private List<string> GetHeaders(ExcelWorksheet sheet, int headerRow)
    {
        var headers = new List<string>();
        if (sheet.Dimension == null) return headers;

        for (int col = 1; col <= sheet.Dimension.Columns; col++)
        {
            var header = sheet.Cells[headerRow, col].Text?.Trim() ?? $"Column{col}";
            headers.Add(header);
        }

        return headers;
    }

    private List<Dictionary<string, string>> GetSampleRows(ExcelWorksheet sheet, int startRow, int count)
    {
        var rows = new List<Dictionary<string, string>>();
        if (sheet.Dimension == null) return rows;

        var headers = GetHeaders(sheet, startRow - 1);
        var endRow = Math.Min(startRow + count - 1, sheet.Dimension.Rows);

        for (int row = startRow; row <= endRow; row++)
        {
            var rowData = new Dictionary<string, string>();
            for (int col = 1; col <= sheet.Dimension.Columns; col++)
            {
                var header = col <= headers.Count ? headers[col - 1] : $"Column{col}";
                var value = sheet.Cells[row, col].Text?.Trim() ?? "";
                rowData[header] = value;
            }
            rows.Add(rowData);
        }

        return rows;
    }
}

