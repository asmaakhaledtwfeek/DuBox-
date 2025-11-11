using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Materials.Commands;

public class ImportMaterialsFromExcelCommandHandler : IRequestHandler<ImportMaterialsFromExcelCommand, Result<MaterialImportResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelService _excelService;

    private static readonly string[] RequiredHeaders = new[]
    {
        "MaterialCode",
        "MaterialName",
        "MaterialCategory",
        "Unit",
        "UnitCost",
        "CurrentStock",
        "MinimumStock",
        "ReorderLevel",
        "SupplierName"
    };

    public ImportMaterialsFromExcelCommandHandler(IUnitOfWork unitOfWork, IExcelService excelService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
    }

    public async Task<Result<MaterialImportResultDto>> Handle(ImportMaterialsFromExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileStream == null)
            return Result.Failure<MaterialImportResultDto>("No file stream provided");

        // Validate file extension
        var fileExtension = Path.GetExtension(request.FileName).ToLower();
        if (fileExtension != ".xlsx" && fileExtension != ".xls")
            return Result.Failure<MaterialImportResultDto>("Invalid file format. Please upload an Excel file (.xlsx or .xls)");

        var errors = new List<string>();
        var importedMaterials = new List<MaterialDto>();
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
                return Result.Failure<MaterialImportResultDto>($"Excel validation failed: {string.Join(", ", validationErrors)}");
            }

            // Read materials from Excel
            stream.Position = 0;
            var materials = await _excelService.ReadFromExcelAsync<ImportMaterialDto>(stream, MapRowToMaterial);

            if (materials == null || materials.Count == 0)
            {
                return Result.Failure<MaterialImportResultDto>("No valid data found in the Excel file");
            }

            var materialRepository = _unitOfWork.Repository<Material>();

            // Process each material
            for (int i = 0; i < materials.Count; i++)
            {
                var materialDto = materials[i];
                var rowNumber = i + 2; // +2 because of header and 0-based index

                try
                {
                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(materialDto.MaterialCode))
                    {
                        errors.Add($"Row {rowNumber}: MaterialCode is required");
                        failureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(materialDto.MaterialName))
                    {
                        errors.Add($"Row {rowNumber}: MaterialName is required");
                        failureCount++;
                        continue;
                    }

                    // Check if material already exists
                    var existingMaterial = await materialRepository
                        .IsExistAsync(m => m.MaterialCode == materialDto.MaterialCode, cancellationToken);

                    if (existingMaterial)
                    {
                        errors.Add($"Row {rowNumber}: Material with code '{materialDto.MaterialCode}' already exists");
                        failureCount++;
                        continue;
                    }

                    // Create new material
                    var material = new Material
                    {
                        MaterialCode = materialDto.MaterialCode,
                        MaterialName = materialDto.MaterialName,
                        MaterialCategory = materialDto.MaterialCategory,
                        Unit = materialDto.Unit,
                        UnitCost = materialDto.UnitCost,
                        CurrentStock = materialDto.CurrentStock,
                        MinimumStock = materialDto.MinimumStock,
                        ReorderLevel = materialDto.ReorderLevel,
                        SupplierName = materialDto.SupplierName,
                        IsActive = true
                    };

                    await materialRepository.AddAsync(material, cancellationToken);
                    
                    var createdMaterialDto = material.Adapt<MaterialDto>() with
                    {
                        IsLowStock = material.IsLowStock,
                        NeedsReorder = material.NeedsReorder
                    };

                    importedMaterials.Add(createdMaterialDto);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {rowNumber}: {ex.Message}");
                    failureCount++;
                }
            }

            // Save all changes if any materials were successfully imported
            if (successCount > 0)
            {
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            var result = new MaterialImportResultDto
            {
                SuccessCount = successCount,
                FailureCount = failureCount,
                Errors = errors,
                ImportedMaterials = importedMaterials
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<MaterialImportResultDto>($"Error processing Excel file: {ex.Message}");
        }
    }

    private ImportMaterialDto MapRowToMaterial(Dictionary<string, object?> row)
    {
        return new ImportMaterialDto
        {
            MaterialCode = GetStringValue(row, "MaterialCode"),
            MaterialName = GetStringValue(row, "MaterialName"),
            MaterialCategory = GetStringValue(row, "MaterialCategory"),
            Unit = GetStringValue(row, "Unit"),
            UnitCost = GetDecimalValue(row, "UnitCost"),
            CurrentStock = GetDecimalValue(row, "CurrentStock"),
            MinimumStock = GetDecimalValue(row, "MinimumStock"),
            ReorderLevel = GetDecimalValue(row, "ReorderLevel"),
            SupplierName = GetStringValue(row, "SupplierName")
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

    private decimal? GetDecimalValue(Dictionary<string, object?> row, string key)
    {
        if (row.TryGetValue(key, out var value) && value != null)
        {
            if (decimal.TryParse(value.ToString(), out var decimalValue))
            {
                return decimalValue;
            }
        }
        return null;
    }
}

