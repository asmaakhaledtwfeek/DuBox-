namespace Dubox.Application.DTOs;

public record BoxAssetDto
{
    public Guid BoxAssetId { get; init; }
    public Guid BoxId { get; init; }
    public string AssetType { get; init; } = string.Empty;
    public string? AssetCode { get; init; }
    public string? AssetName { get; init; }
    public int Quantity { get; init; }
    public string? Unit { get; init; }
    public string? Specifications { get; init; }
    public string? Notes { get; init; }
}

public record CreateBoxAssetDto
{
    public string AssetType { get; init; } = string.Empty;
    public string? AssetCode { get; init; }
    public string? AssetName { get; init; }
    public int Quantity { get; init; } = 1;
    public string? Unit { get; init; }
    public string? Specifications { get; init; }
    public string? Notes { get; init; }
}

