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
                    // Excel columns mapping to new structure
                    var code = GetCellValue(worksheet, row, headers, new[] { "code", "cost code" });
                    var chapter = GetCellValue(worksheet, row, headers, new[] { "chapter" });
                    var subChapter = GetCellValue(worksheet, row, headers, new[] { "sub chapter", "subchapter" });
                    var classification = GetCellValue(worksheet, row, headers, new[] { "classification" });
                    var subClassification = GetCellValue(worksheet, row, headers, new[] { "sub classification", "subclassification" });
                    var name = GetCellValue(worksheet, row, headers, new[] { "name", "description" });
                    var units = GetCellValue(worksheet, row, headers, new[] { "units", "unit", "uom" });
                    var type = GetCellValue(worksheet, row, headers, new[] { "type", "cost type" });
                    var budgetLevel = GetCellValue(worksheet, row, headers, new[] { "budget level", "budgetlevel" });
                    var status = GetCellValue(worksheet, row, headers, new[] { "status" });
                    var job = GetCellValue(worksheet, row, headers, new[] { "job" });
                    var officeAccount = GetCellValue(worksheet, row, headers, new[] { "office account", "officeaccount" });
                    var jobCostAccount = GetCellValue(worksheet, row, headers, new[] { "job cost account", "jobcostaccount" });
                    var specialAccount = GetCellValue(worksheet, row, headers, new[] { "special account", "specialaccount" });
                    var idlAccount = GetCellValue(worksheet, row, headers, new[] { "idl account", "idlaccount" });

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

                    // Set default status if empty
                    if (string.IsNullOrWhiteSpace(status))
                    {
                        status = "Active";
                    }

                    // Check if record already exists in our in-memory dictionary
                    var lookupKey = code ?? name;
                    if (existingRecords.TryGetValue(lookupKey, out var existingId))
                    {
                        // Update existing - fetch from context
                        var existing = await _context.HRCostRecords.FindAsync(new object[] { existingId }, cancellationToken);
                        if (existing != null)
                        {
                            existing.Code = code;
                            existing.Chapter = chapter;
                            existing.SubChapter = subChapter;
                            existing.Classification = classification;
                            existing.SubClassification = subClassification;
                            existing.Name = name;
                            existing.Units = units;
                            existing.Type = type;
                            existing.BudgetLevel = budgetLevel;
                            existing.Status = status;
                            existing.Job = job;
                            existing.OfficeAccount = officeAccount;
                            existing.JobCostAccount = jobCostAccount;
                            existing.SpecialAccount = specialAccount;
                            existing.IDLAccount = idlAccount;
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
                            Chapter = chapter,
                            SubChapter = subChapter,
                            Classification = classification,
                            SubClassification = subClassification,
                            Name = name,
                            Units = units,
                            Type = type,
                            BudgetLevel = budgetLevel,
                            Status = status,
                            Job = job,
                            OfficeAccount = officeAccount,
                            JobCostAccount = jobCostAccount,
                            SpecialAccount = specialAccount,
                            IDLAccount = idlAccount,
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

