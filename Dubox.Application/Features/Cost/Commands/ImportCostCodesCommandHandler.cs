using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Dubox.Application.Features.Cost.Commands;

public class ImportCostCodesCommandHandler : IRequestHandler<ImportCostCodesCommand, Result<ImportCostCodesResult>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ImportCostCodesCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ImportCostCodesResult>> Handle(ImportCostCodesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!File.Exists(request.FilePath))
            {
                return Result.Failure<ImportCostCodesResult>(new Error("FileNotFound", $"File not found: {request.FilePath}"));
            }

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var result = new ImportCostCodesResult
            {
                Errors = new List<string>()
            };

            // Clear existing data if requested
            if (request.ClearExisting)
            {
                var codesToRemove = await _context.CostCodes.ToListAsync(cancellationToken);
                _context.CostCodes.RemoveRange(codesToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }

            using var package = new ExcelPackage(new FileInfo(request.FilePath));
            var worksheet = package.Workbook.Worksheets[0]; // First sheet
            
            var rowCount = worksheet.Dimension?.Rows ?? 0;
            var colCount = worksheet.Dimension?.Columns ?? 0;

            if (rowCount < 2) // At least header + 1 data row
            {
                return Result.Failure<ImportCostCodesResult>(new Error("InvalidFile", "File contains no data"));
            }

            var successCount = 0;
            var skippedCount = 0;
            var errorCount = 0;
            var errors = new List<string>();
            var batchSize = 5; // Save every 5 records (smaller for better performance)
            var batchCount = 0;

            // Load all existing cost codes into memory once to avoid context disposal issues
            var existingCodes = await _context.CostCodes
                .AsNoTracking()
                .ToDictionaryAsync(c => c.Code, c => c.CostCodeId, cancellationToken);

            // Start from row 2 (assuming row 1 is header)
            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    // Read all columns from Excel file matching the structure:
                    // Col 1: Cost Code (Main identifier)
                    // Col 2: Cost Code Level 1
                    // Col 3: Level 1 Description
                    // Col 4: Cost Code Level 2
                    // Col 5: Level 2 Description
                    // Col 6: Cost Code Level 3
                    // Col 7: Level 3 Description – as per CSI
                    // Col 8: Level 3 Description – as per CSI (abbreviation less than 50 characters)
                    // Col 9: Level 3 Description – AMANA
                    var code = worksheet.Cells[row, 1].Text?.Trim(); // Main Cost Code
                    var costCodeLevel1 = worksheet.Cells[row, 2].Text?.Trim();
                    var level1Description = worksheet.Cells[row, 3].Text?.Trim();
                    var costCodeLevel2 = worksheet.Cells[row, 4].Text?.Trim();
                    var level2Description = worksheet.Cells[row, 5].Text?.Trim();
                    var costCodeLevel3 = worksheet.Cells[row, 6].Text?.Trim();
                    var description = worksheet.Cells[row, 7].Text?.Trim(); // Level 3 Description – as per CSI
                    var level3DescriptionAbbrev = worksheet.Cells[row, 8].Text?.Trim();
                    var level3DescriptionAmana = worksheet.Cells[row, 9].Text?.Trim();

                    // Skip empty rows
                    if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(description))
                    {
                        skippedCount++;
                        continue;
                    }

                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        errors.Add($"Row {row}: Missing cost code");
                        errorCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(description))
                    {
                        errors.Add($"Row {row}: Missing description");
                        errorCount++;
                        continue;
                    }

                    // Check if code already exists in our in-memory dictionary
                    if (existingCodes.TryGetValue(code, out var existingId))
                    {
                        // Update existing - fetch from context
                        var existing = await _context.CostCodes.FindAsync(new object[] { existingId }, cancellationToken);
                        if (existing != null)
                        {
                            existing.Code = code;
                            existing.CostCodeLevel1 = costCodeLevel1;
                            existing.Level1Description = level1Description;
                            existing.CostCodeLevel2 = costCodeLevel2;
                            existing.Level2Description = level2Description;
                            existing.CostCodeLevel3 = costCodeLevel3;
                            existing.Description = description;
                            existing.Level3DescriptionAbbrev = level3DescriptionAbbrev;
                            existing.Level3DescriptionAmana = level3DescriptionAmana;
                            existing.ModifiedDate = DateTime.UtcNow;
                            existing.ModifiedBy = _currentUserService.UserId;
                        }
                    }
                    else
                    {
                        // Create new
                        var newId = Guid.NewGuid();
                        var costCode = new CostCodeMaster
                        {
                            CostCodeId = newId,
                            Code = code,
                            CostCodeLevel1 = costCodeLevel1,
                            Level1Description = level1Description,
                            CostCodeLevel2 = costCodeLevel2,
                            Level2Description = level2Description,
                            CostCodeLevel3 = costCodeLevel3,
                            Description = description,
                            Level3DescriptionAbbrev = level3DescriptionAbbrev,
                            Level3DescriptionAmana = level3DescriptionAmana,
                            Currency = "SAR",
                            IsActive = true,
                            DisplayOrder = row - 1,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = _currentUserService.UserId
                        };

                        _context.CostCodes.Add(costCode);
                        existingCodes[code] = newId; // Add to dictionary for future lookups
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
}

