using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public class ImportBoxTypeMaterialsFromExcelCommandHandler 
    : IRequestHandler<ImportBoxTypeMaterialsFromExcelCommand, Result<BoxTypeMaterialImportResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelService _excelService;
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBoxMaterialDeliveryService _boxMaterialDeliveryService;

    private static readonly string[] RequiredHeaders = new[]
    {
        "Material Code",
        "Material Name",
        "Category",
        "Unit",
        "Quantity Per Box",
        "Total Quantity (Auto-calculated)",
        "Status",
        "Delivered Quantity",
        "Required Before (Days)",
        "Notes"
    };

    public ImportBoxTypeMaterialsFromExcelCommandHandler(
        IUnitOfWork unitOfWork, 
        IExcelService excelService, 
        IDbContext context,
        ICurrentUserService currentUserService,
        IBoxMaterialDeliveryService boxMaterialDeliveryService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
        _context = context;
        _currentUserService = currentUserService;
        _boxMaterialDeliveryService = boxMaterialDeliveryService;
    }

    public async Task<Result<BoxTypeMaterialImportResultDto>> Handle(
        ImportBoxTypeMaterialsFromExcelCommand request, 
        CancellationToken cancellationToken)
    {
        if (request.FileStream == null)
            return Result.Failure<BoxTypeMaterialImportResultDto>("No file stream provided");

        var fileExtension = Path.GetExtension(request.FileName).ToLower();
        if (fileExtension != ".xlsx" && fileExtension != ".xls")
            return Result.Failure<BoxTypeMaterialImportResultDto>("Invalid file format. Please upload an Excel file (.xlsx or .xls)");

        // Verify that the ProjectBoxType exists
        var projectBoxType = await _context.ProjectBoxTypes
            .FirstOrDefaultAsync(pbt => pbt.Id == request.ProjectBoxTypeId, cancellationToken);

        if (projectBoxType == null)
            return Result.Failure<BoxTypeMaterialImportResultDto>($"Box type with ID {request.ProjectBoxTypeId} not found");

        var errors = new List<string>();
        var warnings = new List<string>();
        var successCount = 0;
        var failureCount = 0;

        try
        {
            var stream = request.FileStream;

            // Validate Excel structure
            stream.Position = 0;
            var (isValid, validationErrors) = await _excelService.ValidateExcelStructureAsync(stream, RequiredHeaders);
            if (!isValid)
            {
                return Result.Failure<BoxTypeMaterialImportResultDto>(
                    $"Excel validation failed: {string.Join(", ", validationErrors)}");
            }

            // Read Excel data
            stream.Position = 0;
            var materialRows = await _excelService.ReadFromExcelAsync<ImportBoxTypeMaterialRowDto>(
                stream, MapRowToBoxTypeMaterial);

            if (materialRows == null || materialRows.Count == 0)
            {
                return Result.Failure<BoxTypeMaterialImportResultDto>("No valid data found in the Excel file");
            }

            // Filter out completely empty rows and summary rows
            materialRows = materialRows
                .Where(r => !string.IsNullOrWhiteSpace(r.MaterialCode) || 
                           !string.IsNullOrWhiteSpace(r.MaterialName) || 
                           r.QuantityPerBox > 0)
                .Where(r => !r.MaterialCode.StartsWith("Box Type Summary", StringComparison.OrdinalIgnoreCase) &&
                           !r.MaterialCode.StartsWith("Note:", StringComparison.OrdinalIgnoreCase) &&
                           !r.MaterialCode.StartsWith("Note", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (materialRows.Count == 0)
            {
                return Result.Failure<BoxTypeMaterialImportResultDto>("No valid data found in the Excel file after filtering empty rows");
            }

            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            var now = DateTime.UtcNow;

            // Get box count for this box type to calculate total quantity
            var boxCount = await _context.Boxes
                .Where(b => b.ProjectBoxTypeId == request.ProjectBoxTypeId 
                    && b.ProjectId == projectBoxType.ProjectId
                    && b.IsActive)
                .CountAsync(cancellationToken);

            // Get all materials in the system for lookup
            var allMaterialCodes = materialRows.Select(m => m.MaterialCode).Distinct().ToList();
            var materialsDict = await _context.Materials
                .Where(m => allMaterialCodes.Contains(m.MaterialCode))
                .ToDictionaryAsync(m => m.MaterialCode, m => m, cancellationToken);

            // Process each row
            for (int i = 0; i < materialRows.Count; i++)
            {
                var row = materialRows[i];
                var rowNumber = i + 2; // Excel rows start at 1, plus 1 for header row

                try
                {
                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(row.MaterialCode))
                    {
                        errors.Add($"Row {rowNumber}: Material Code is required");
                        failureCount++;
                        continue;
                    }

                    // Find the material
                    if (!materialsDict.TryGetValue(row.MaterialCode, out var material))
                    {
                        errors.Add($"Row {rowNumber}: Material with code '{row.MaterialCode}' not found in the system");
                        failureCount++;
                        continue;
                    }

                    // Validate QuantityPerBox
                    if (row.QuantityPerBox <= 0)
                    {
                        errors.Add($"Row {rowNumber}: Quantity Per Box must be greater than 0");
                        failureCount++;
                        continue;
                    }

                    // Calculate total quantity based on box count and quantity per box
                    var calculatedTotalQuantity = boxCount * row.QuantityPerBox;

                    // Validate DeliveredQuantity doesn't exceed TotalQuantity
                    if (row.DeliveredQuantity.HasValue && row.DeliveredQuantity.Value > calculatedTotalQuantity)
                    {
                        errors.Add($"Row {rowNumber}: Delivered Quantity ({row.DeliveredQuantity.Value}) cannot exceed Total Quantity ({calculatedTotalQuantity})");
                        failureCount++;
                        continue;
                    }

                    // Check if warning about total quantity mismatch
                    if (row.TotalQuantity > 0 && row.TotalQuantity != calculatedTotalQuantity)
                    {
                        warnings.Add($"Row {rowNumber}: Total Quantity in Excel ({row.TotalQuantity}) differs from calculated value ({calculatedTotalQuantity} = {boxCount} boxes × {row.QuantityPerBox} per box). Using calculated value.");
                    }

                    // Find existing BoxTypeMaterial
                    var existingBoxTypeMaterial = await _context.BoxTypeMaterials
                        .FirstOrDefaultAsync(
                            btm => btm.ProjectBoxTypeId == request.ProjectBoxTypeId 
                                && btm.MaterialId == material.MaterialId,
                            cancellationToken);

                    if (existingBoxTypeMaterial == null)
                    {
                        errors.Add($"Row {rowNumber}: Box type material assignment not found for material '{row.MaterialCode}'. Material must be assigned to this box type first.");
                        failureCount++;
                        continue;
                    }

                    // Track old values to detect changes
                    var oldQuantityPerBox = existingBoxTypeMaterial.QuantityPerBox;
                    var quantityPerBoxChanged = oldQuantityPerBox != row.QuantityPerBox;

                    // Update the box type material
                    existingBoxTypeMaterial.QuantityPerBox = row.QuantityPerBox;
                    var newDeliveredQuantity = row.DeliveredQuantity ?? 0;
                    existingBoxTypeMaterial.DeliveredQuantity = newDeliveredQuantity;
                    existingBoxTypeMaterial.Notes = row.Notes;
                    existingBoxTypeMaterial.ModifiedDate = now;

                    // Recalculate delivery progress and arrival status based on new total quantity
                    if (calculatedTotalQuantity > 0)
                    {
                        var currentDeliveredQty = row.DeliveredQuantity ?? existingBoxTypeMaterial.DeliveredQuantity;
                        var progressValue = (currentDeliveredQty / calculatedTotalQuantity);
                        var progress = (int)Math.Round((progressValue.Value * 100));
                        existingBoxTypeMaterial.DeliveryProgress = Math.Clamp(progress, 0, 100);

                        if (progress >= 100)
                        {
                            // Mark as delivered
                            existingBoxTypeMaterial.IsArrived = true;
                            existingBoxTypeMaterial.ArrivedDate = existingBoxTypeMaterial.ArrivedDate ?? now;
                            existingBoxTypeMaterial.ArrivedBy = currentUserId;
                        }
                        else
                        {
                            // Not fully delivered - update status
                            existingBoxTypeMaterial.IsArrived = false;
                            // Note: We keep ArrivedDate and ArrivedBy for historical tracking
                        }
                    }
                    else
                    {
                        // If total quantity is 0, reset delivery status
                        existingBoxTypeMaterial.DeliveryProgress = 0;
                        existingBoxTypeMaterial.IsArrived = false;
                    }

                    _unitOfWork.Repository<BoxTypeMaterial>().Update(existingBoxTypeMaterial);

                    // Update individual box material deliveries
                    // Recalculate all if quantity per box changed, otherwise just distribute new deliveries
                    await _boxMaterialDeliveryService.UpdateBoxMaterialDeliveriesAsync(
                        existingBoxTypeMaterial,
                        newDeliveredQuantity,
                        currentUserId,
                        quantityPerBoxChanged,
                        cancellationToken);

                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {rowNumber}: {ex.Message}");
                    failureCount++;
                }
            }

            // Save all changes
            if (successCount > 0)
            {
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            var result = new BoxTypeMaterialImportResultDto
            {
                SuccessCount = successCount,
                FailureCount = failureCount,
                Errors = errors,
                Warnings = warnings
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<BoxTypeMaterialImportResultDto>($"Error processing Excel file: {ex.Message}");
        }
    }

    private ImportBoxTypeMaterialRowDto MapRowToBoxTypeMaterial(Dictionary<string, object?> row)
    {
        return new ImportBoxTypeMaterialRowDto
        {
            MaterialCode = GetStringValue(row, "Material Code"),
            MaterialName = GetStringValue(row, "Material Name"),
            Category = GetStringValue(row, "Category"),
            QuantityPerBox = GetIntValue(row, "Quantity Per Box"),
            TotalQuantity = GetDecimalValue(row, "Total Quantity (Auto-calculated)") ?? 0,
            Status = GetStringValue(row, "Status"),
            DeliveredQuantity = GetDecimalValue(row, "Delivered Quantity"),
            RequiredBeforeDays = GetStringValue(row, "Required Before (Days)"),
            Notes = GetStringValue(row, "Notes")
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

    private int GetIntValue(Dictionary<string, object?> row, string key)
    {
        if (row.TryGetValue(key, out var value) && value != null)
        {
            var stringValue = value.ToString()?.Trim();
            // Handle "-" or empty values
            if (string.IsNullOrEmpty(stringValue) || stringValue == "-")
            {
                return 0;
            }
            
            if (int.TryParse(stringValue, out var intValue))
            {
                return intValue;
            }
        }
        return 0;
    }

    private decimal? GetDecimalValue(Dictionary<string, object?> row, string key)
    {
        if (row.TryGetValue(key, out var value) && value != null)
        {
            var stringValue = value.ToString()?.Trim();
            // Handle "-" or empty values
            if (string.IsNullOrEmpty(stringValue) || stringValue == "-")
            {
                return null;
            }
            
            if (decimal.TryParse(stringValue, out var decimalValue))
            {
                return decimalValue;
            }
        }
        return null;
    }
}
