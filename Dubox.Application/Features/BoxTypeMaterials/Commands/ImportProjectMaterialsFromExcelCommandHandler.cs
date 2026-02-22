using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public class ImportProjectMaterialsFromExcelCommandHandler 
    : IRequestHandler<ImportProjectMaterialsFromExcelCommand, Result<BoxTypeMaterialImportResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
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

    public ImportProjectMaterialsFromExcelCommandHandler(
        IUnitOfWork unitOfWork, 
        IDbContext context,
        ICurrentUserService currentUserService,
        IBoxMaterialDeliveryService boxMaterialDeliveryService)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _currentUserService = currentUserService;
        _boxMaterialDeliveryService = boxMaterialDeliveryService;
    }

    public async Task<Result<BoxTypeMaterialImportResultDto>> Handle(
        ImportProjectMaterialsFromExcelCommand request, 
        CancellationToken cancellationToken)
    {
        if (request.FileStream == null)
            return Result.Failure<BoxTypeMaterialImportResultDto>("No file stream provided");

        var fileExtension = Path.GetExtension(request.FileName).ToLower();
        if (fileExtension != ".xlsx" && fileExtension != ".xls")
            return Result.Failure<BoxTypeMaterialImportResultDto>("Invalid file format. Please upload an Excel file (.xlsx or .xls)");

        var errors = new List<string>();
        var warnings = new List<string>();
        var successCount = 0;
        var failureCount = 0;

        try
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            var now = DateTime.UtcNow;

            using var package = new ExcelPackage(request.FileStream);

            if (package.Workbook.Worksheets.Count == 0)
                return Result.Failure<BoxTypeMaterialImportResultDto>("No worksheets found in the Excel file");

            // Get all box types for this project
            var projectBoxTypes = await _context.ProjectBoxTypes
                .Where(pbt => pbt.ProjectId == request.ProjectId && pbt.IsActive)
                .ToDictionaryAsync(pbt => pbt.TypeName, pbt => pbt, cancellationToken);

            // Get all materials in the system for lookup
            var allMaterials = await _context.Materials
                .ToDictionaryAsync(m => m.MaterialCode, m => m, cancellationToken);

            // Process each worksheet (each represents a box type)
            foreach (var worksheet in package.Workbook.Worksheets)
            {
                var sheetName = worksheet.Name;
                
                // Find the box type for this sheet
                var boxType = projectBoxTypes.Values.FirstOrDefault(bt => 
                    SanitizeSheetName(bt.TypeName) == sheetName);

                if (boxType == null)
                {
                    warnings.Add($"Sheet '{sheetName}': Box type not found in project. Skipping sheet.");
                    continue;
                }

                // Get box count for this box type
                var boxCount = await _context.Boxes
                    .Where(b => b.ProjectBoxTypeId == boxType.Id 
                        && b.ProjectId == request.ProjectId
                        && b.IsActive)
                    .CountAsync(cancellationToken);

                // Read data from worksheet (starting from row 3, skipping header and instructions)
                var startRow = 3;
                var endRow = worksheet.Dimension?.End.Row ?? 0;

                for (int row = startRow; row <= endRow; row++)
                {
                    // Skip empty rows
                    var materialCode = worksheet.Cells[row, 1].GetValue<string>()?.Trim();
                    if (string.IsNullOrWhiteSpace(materialCode))
                        continue;

                    // Skip summary rows (Box Type Summary, Note, etc.)
                    if (materialCode.StartsWith("Box Type Summary", StringComparison.OrdinalIgnoreCase) ||
                        materialCode.StartsWith("Note:", StringComparison.OrdinalIgnoreCase) ||
                        materialCode.StartsWith("Note", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    try
                    {
                        // Read row data (based on new column order)
                        var materialName = worksheet.Cells[row, 2].GetValue<string>()?.Trim() ?? string.Empty;
                        var category = worksheet.Cells[row, 3].GetValue<string>()?.Trim() ?? string.Empty;
                        var unit = worksheet.Cells[row, 4].GetValue<string>()?.Trim() ?? string.Empty;
                        var quantityPerBox = GetIntValueFromCell(worksheet.Cells[row, 5]);
                        var totalQuantity = GetDecimalValueFromCell(worksheet.Cells[row, 6]);
                        var status = worksheet.Cells[row, 7].GetValue<string>()?.Trim() ?? string.Empty;
                        var deliveredQuantity = GetNullableDecimalValueFromCell(worksheet.Cells[row, 8]);
                        var requiredBeforeDays = worksheet.Cells[row, 9].GetValue<string>()?.Trim() ?? string.Empty;
                        var notes = worksheet.Cells[row, 10].GetValue<string>()?.Trim() ?? string.Empty;

                        // Validate required fields
                        if (string.IsNullOrWhiteSpace(materialCode))
                        {
                            errors.Add($"Sheet '{sheetName}', Row {row}: Material Code is required");
                            failureCount++;
                            continue;
                        }

                        // Find the material
                        if (!allMaterials.TryGetValue(materialCode, out var material))
                        {
                            errors.Add($"Sheet '{sheetName}', Row {row}: Material with code '{materialCode}' not found in the system");
                            failureCount++;
                            continue;
                        }

                        // Validate QuantityPerBox
                        if (quantityPerBox <= 0)
                        {
                            errors.Add($"Sheet '{sheetName}', Row {row}: Quantity Per Box must be greater than 0");
                            failureCount++;
                            continue;
                        }

                        // Calculate total quantity based on box count and quantity per box
                        var calculatedTotalQuantity = boxCount * quantityPerBox;

                        // Validate DeliveredQuantity doesn't exceed TotalQuantity
                        if (deliveredQuantity.HasValue && deliveredQuantity.Value > calculatedTotalQuantity)
                        {
                            errors.Add($"Sheet '{sheetName}', Row {row}: Delivered Quantity ({deliveredQuantity.Value}) cannot exceed Total Quantity ({calculatedTotalQuantity})");
                            failureCount++;
                            continue;
                        }

                        // Check if warning about total quantity mismatch (if user manually edited the formula)
                        if (totalQuantity > 0 && Math.Abs(totalQuantity - calculatedTotalQuantity) > 0.01m)
                        {
                            warnings.Add($"Sheet '{sheetName}', Row {row}: Total Quantity in Excel ({totalQuantity}) differs from calculated value ({calculatedTotalQuantity} = {boxCount} boxes × {quantityPerBox} per box). Using calculated value.");
                        }

                        // Find existing BoxTypeMaterial
                        var existingBoxTypeMaterial = await _context.BoxTypeMaterials
                            .FirstOrDefaultAsync(
                                btm => btm.ProjectBoxTypeId == boxType.Id 
                                    && btm.MaterialId == material.MaterialId,
                                cancellationToken);

                        if (existingBoxTypeMaterial == null)
                        {
                            errors.Add($"Sheet '{sheetName}', Row {row}: Box type material assignment not found for material '{materialCode}'. Material must be assigned to this box type first.");
                            failureCount++;
                            continue;
                        }

                        // Track old values to detect changes
                        var oldQuantityPerBox = existingBoxTypeMaterial.QuantityPerBox;
                        var quantityPerBoxChanged = oldQuantityPerBox != quantityPerBox;

                        // Update the box type material
                        existingBoxTypeMaterial.QuantityPerBox = quantityPerBox;
                        var newDeliveredQuantity = deliveredQuantity ?? 0;
                        existingBoxTypeMaterial.DeliveredQuantity = newDeliveredQuantity;
                        existingBoxTypeMaterial.Notes = notes;
                        existingBoxTypeMaterial.ModifiedDate = now;

                        // Recalculate delivery progress and arrival status based on new total quantity
                        if (calculatedTotalQuantity > 0)
                        {
                            var currentDeliveredQty = deliveredQuantity ?? existingBoxTypeMaterial.DeliveredQuantity;
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
                        errors.Add($"Sheet '{sheetName}', Row {row}: {ex.Message}");
                        failureCount++;
                    }
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

    private string SanitizeSheetName(string name)
    {
        // Remove invalid characters for Excel sheet names
        var sanitized = name.Replace("/", "_")
                           .Replace("\\", "_")
                           .Replace("?", "_")
                           .Replace("*", "_")
                           .Replace("[", "_")
                           .Replace("]", "_");
        
        // Limit to 31 characters (Excel limit)
        if (sanitized.Length > 31)
            sanitized = sanitized.Substring(0, 31);
        
        return sanitized;
    }

    /// <summary>
    /// Safely get an integer value from an Excel cell, handling "-" and empty values
    /// </summary>
    private int GetIntValueFromCell(ExcelRange cell)
    {
        var value = cell.Value;
        if (value == null)
            return 0;

        var stringValue = value.ToString()?.Trim();
        if (string.IsNullOrEmpty(stringValue) || stringValue == "-")
            return 0;

        if (int.TryParse(stringValue, out var intValue))
            return intValue;

        return 0;
    }

    /// <summary>
    /// Safely get a decimal value from an Excel cell, handling "-" and empty values
    /// </summary>
    private decimal GetDecimalValueFromCell(ExcelRange cell)
    {
        var value = cell.Value;
        if (value == null)
            return 0;

        var stringValue = value.ToString()?.Trim();
        if (string.IsNullOrEmpty(stringValue) || stringValue == "-")
            return 0;

        if (decimal.TryParse(stringValue, out var decimalValue))
            return decimalValue;

        return 0;
    }

    /// <summary>
    /// Safely get a nullable decimal value from an Excel cell, handling "-" and empty values
    /// </summary>
    private decimal? GetNullableDecimalValueFromCell(ExcelRange cell)
    {
        var value = cell.Value;
        if (value == null)
            return null;

        var stringValue = value.ToString()?.Trim();
        if (string.IsNullOrEmpty(stringValue) || stringValue == "-")
            return null;

        if (decimal.TryParse(stringValue, out var decimalValue))
            return decimalValue;

        return null;
    }
}
