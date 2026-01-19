using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Dubox.Application.Features.Cost.Commands;

public class ImportHRCostsCommandHandler : IRequestHandler<ImportHRCostsCommand, Result<ImportCostCodesResult>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ImportHRCostsCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ImportCostCodesResult>> Handle(ImportHRCostsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!File.Exists(request.FilePath))
            {
                return Result.Failure<ImportCostCodesResult>(new Error("FileNotFound", $"File not found: {request.FilePath}"));
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var result = new ImportCostCodesResult
            {
                Errors = new List<string>()
            };

            if (request.ClearExisting)
            {
                var recordsToRemove = await _context.HRCostRecords.ToListAsync(cancellationToken);
                _context.HRCostRecords.RemoveRange(recordsToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }

            using var package = new ExcelPackage(new FileInfo(request.FilePath));
            var worksheet = package.Workbook.Worksheets[0];
            
            var rowCount = worksheet.Dimension?.Rows ?? 0;

            if (rowCount < 2)
            {
                return Result.Failure<ImportCostCodesResult>(new Error("InvalidFile", "File contains no data"));
            }

            var successCount = 0;
            var skippedCount = 0;
            var errorCount = 0;
            var errors = new List<string>();
            var batchSize = 5; // Save every 5 records (smaller for better performance)
            var batchCount = 0;

            // Read header row to map columns dynamically
            var headers = new Dictionary<string, int>();
            var maxColumns = worksheet.Dimension?.Columns ?? 0;
            for (int col = 1; col <= maxColumns; col++)
            {
                var header = worksheet.Cells[1, col].Text?.Trim().ToLower() ?? "";
                if (!string.IsNullOrWhiteSpace(header))
                {
                    headers[header] = col;
                }
            }

            // Load all existing HR cost records into memory once to avoid context disposal issues
            var existingRecords = await _context.HRCostRecords
                .AsNoTracking()
                .ToDictionaryAsync(h => h.Code ?? h.Name, h => h.HRCostRecordId, cancellationToken);

            // Start from row 2 (assuming row 1 is header)
            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    // Excel columns: Code | Name | Units | Type | Status
                    var code = GetCellValue(worksheet, row, headers, new[] { "code", "resource code", "employee code", "id", "hrc code" });
                    var name = GetCellValue(worksheet, row, headers, new[] { "name", "resource name", "position", "role", "designation" });
                    var units = GetCellValue(worksheet, row, headers, new[] { "units", "unit", "uom" });
                    var type = GetCellValue(worksheet, row, headers, new[] { "type", "cost type", "category", "manpower type" });
                    var statusText = GetCellValue(worksheet, row, headers, new[] { "status", "active", "is active" });
                    
                    // Try to parse additional rate columns if they exist
                    var hourlyRateText = GetCellValue(worksheet, row, headers, new[] { "hourly rate", "hour rate", "rate/hour", "hourly", "ls" });
                    var dailyRateText = GetCellValue(worksheet, row, headers, new[] { "daily rate", "day rate", "rate/day", "daily" });
                    var monthlyRateText = GetCellValue(worksheet, row, headers, new[] { "monthly rate", "month rate", "rate/month", "monthly", "salary" });
                    var overtimeRateText = GetCellValue(worksheet, row, headers, new[] { "overtime rate", "ot rate", "overtime", "ot" });
                    var trade = GetCellValue(worksheet, row, headers, new[] { "trade", "discipline", "department" });
                    var position = GetCellValue(worksheet, row, headers, new[] { "position", "role", "title" });

                    // Skip empty rows
                    if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
                    {
                        skippedCount++;
                        continue;
                    }

                    // Name is required
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        errors.Add($"Row {row}: Missing name");
                        errorCount++;
                        continue;
                    }

                    // Determine if active
                    var isActive = statusText?.ToLower() == "active" || statusText?.ToLower() == "yes" || statusText?.ToLower() == "true" || string.IsNullOrWhiteSpace(statusText);

                    // Check if record already exists in our in-memory dictionary
                    var lookupKey = code ?? name;
                    if (existingRecords.TryGetValue(lookupKey, out var existingId))
                    {
                        // Update existing - fetch from context
                        var existing = await _context.HRCostRecords.FindAsync(new object[] { existingId }, cancellationToken);
                        if (existing != null)
                        {
                            existing.Code = code;
                            existing.Name = name;
                            existing.Units = units;
                            existing.CostType = type;
                            existing.Trade = trade;
                            existing.Position = position;
                            existing.HourlyRate = ParseDecimal(hourlyRateText);
                            existing.DailyRate = ParseDecimal(dailyRateText);
                            existing.MonthlyRate = ParseDecimal(monthlyRateText);
                            existing.OvertimeRate = ParseDecimal(overtimeRateText);
                            existing.IsActive = isActive;
                            existing.ModifiedDate = DateTime.UtcNow;
                            existing.ModifiedBy = _currentUserService.UserId;
                        }
                    }
                    else
                    {
                        var newId = Guid.NewGuid();
                        var hrCost = new HRCostRecord
                        {
                            HRCostRecordId = newId,
                            Code = code,
                            Name = name,
                            Units = units,
                            CostType = type,
                            Trade = trade,
                            Position = position,
                            HourlyRate = ParseDecimal(hourlyRateText),
                            DailyRate = ParseDecimal(dailyRateText),
                            MonthlyRate = ParseDecimal(monthlyRateText),
                            OvertimeRate = ParseDecimal(overtimeRateText),
                            Currency = "SAR",
                            IsActive = isActive,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = _currentUserService.UserId
                        };

                        _context.HRCostRecords.Add(hrCost);
                        existingRecords[lookupKey] = newId; // Add to dictionary for future lookups
                    }

                    successCount++;
                    batchCount++;

                    // Save in batches of 5
                    if (batchCount >= batchSize)
                    {
                        try
                        {
                            await _context.SaveChangesAsync(cancellationToken);
                            batchCount = 0;
                        }
                        catch (Exception batchEx)
                        {
                            errors.Add($"Batch save error at row {row}: {batchEx.Message}");
                            errorCount++;
                            batchCount = 0; // Reset counter even on error
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {row}: {ex.Message}");
                    errorCount++;
                }
            }

            // Save any remaining records
            if (batchCount > 0)
            {
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception finalEx)
                {
                    errors.Add($"Final batch save error: {finalEx.Message}");
                    errorCount++;
                }
            }

            var importResult = new ImportCostCodesResult
            {
                TotalRecords = rowCount - 1,
                SuccessCount = successCount,
                SkippedCount = skippedCount,
                ErrorCount = errorCount,
                Errors = errors
            };

            return Result.Success(importResult);
        }
        catch (Exception ex)
        {
            return Result.Failure<ImportCostCodesResult>(new Error("ImportFailed", $"Import failed: {ex.Message}"));
        }
    }

    private string? GetCellValue(ExcelWorksheet worksheet, int row, Dictionary<string, int> headers, string[] possibleNames)
    {
        foreach (var name in possibleNames)
        {
            if (headers.TryGetValue(name, out var col))
            {
                return worksheet.Cells[row, col].Text?.Trim();
            }
        }
        return null;
    }

    private decimal? ParseDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        value = value.Replace(",", "").Replace("SAR", "").Trim();
        
        if (decimal.TryParse(value, out var result))
            return result;
        
        return null;
    }
}

