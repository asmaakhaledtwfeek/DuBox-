using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs;

public record MaterialDto
{
    public Guid MaterialId { get; init; }
    public string MaterialCode { get; init; } = string.Empty;
    public string MaterialName { get; init; } = string.Empty;
    public string? MaterialCategory { get; init; }
    public string? Unit { get; init; }
    public decimal? UnitCost { get; init; }
    public decimal? CurrentStock { get; init; }
    public decimal? MinimumStock { get; init; }
    public decimal? ReorderLevel { get; init; }
    public string? SupplierName { get; init; }
    public bool IsActive { get; init; }
    public bool IsLowStock { get; init; }
    public bool NeedsReorder { get; init; }
}

public record CreateMaterialDto
{
    public string MaterialCode { get; init; } = string.Empty;
    public string MaterialName { get; init; } = string.Empty;
    public string? MaterialCategory { get; init; }
    public string? Unit { get; init; }
    public decimal? UnitCost { get; init; }
    public decimal? CurrentStock { get; init; }
    public decimal? MinimumStock { get; init; }
    public decimal? ReorderLevel { get; init; }
    public string? SupplierName { get; init; }
}

public record UpdateMaterialDto
{
    public int MaterialId { get; init; }
    public string MaterialCode { get; init; } = string.Empty;
    public string MaterialName { get; init; } = string.Empty;
    public string? MaterialCategory { get; init; }
    public string? Unit { get; init; }
    public decimal? UnitCost { get; init; }
    public decimal? MinimumStock { get; init; }
    public decimal? ReorderLevel { get; init; }
    public string? SupplierName { get; init; }
    public bool IsActive { get; init; }
}

public record RestockMaterialDto
{
    public Guid MaterialId { get; init; }
    public decimal RestockQuantity { get; init; }
    public decimal CurrentStock { get; init; }
    public MaterialTransactionTypeEnum TransactionType { get; init; }
    public string? Reason { get; init; }
}

public record LowStockMaterialDto
{
    public Guid MaterialId { get; init; }
    public string MaterialCode { get; init; } = string.Empty;
    public string MaterialName { get; init; } = string.Empty;
    public decimal? CurrentStock { get; init; }
    public decimal? MinimumStock { get; init; }
    public decimal? ReorderLevel { get; init; }
    public decimal Shortage { get; init; }
    public bool NeedsReorder { get; init; }
}

public record ImportMaterialDto
{
    public string MaterialCode { get; init; } = string.Empty;
    public string MaterialName { get; init; } = string.Empty;
    public string? MaterialCategory { get; init; }
    public string? Unit { get; init; }
    public decimal? UnitCost { get; init; }
    public decimal? CurrentStock { get; init; }
    public decimal? MinimumStock { get; init; }
    public decimal? ReorderLevel { get; init; }
    public string? SupplierName { get; init; }
}

public record MaterialImportResultDto
{
    public int SuccessCount { get; init; }
    public int FailureCount { get; init; }
    public List<string> Errors { get; init; } = new();
    public List<MaterialDto> ImportedMaterials { get; init; } = new();
}
public record GetAllMaterialTransactionsDto
{
    public Guid MaterialId { get; init; }
    public string MaterialName { get; init; } = string.Empty;
    public string MaterialCode { get; init; } = string.Empty;
    public decimal? CurrentStock { get; init; }
    public List<MaterialTransactionDto> Transactions { get; init; } = new();
}

