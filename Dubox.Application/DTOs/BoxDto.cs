namespace Dubox.Application.DTOs;

public record BoxDto
{
    public Guid BoxId { get; init; }
    public Guid ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string BoxTag { get; init; } = string.Empty;
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public string? Floor { get; init; }
    public string? Building { get; init; }
    public string? Zone { get; init; }
    public string QRCodeString { get; init; } = string.Empty;
    public string? QRCodeImage { get; init; } = string.Empty;
    public decimal ProgressPercentage { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public string? UnitOfMeasure { get; init; }
    public string? BIMModelReference { get; init; }
    public string? RevitElementId { get; init; }
    public int? Duration { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? ActualStartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public DateTime CreatedDate { get; init; }
    public int ActivitiesCount { get; init; }
    public string? Notes { get; init; }
}

public record CreateBoxDto
{
    public Guid ProjectId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public string? Floor { get; init; }
    public string? Building { get; init; }
    public string? Zone { get; init; }
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public string? BIMModelReference { get; init; }
    public string? RevitElementId { get; init; }
    public List<CreateBoxAssetDto>? Assets { get; init; }
}

public record UpdateBoxDto
{
    public Guid BoxId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public string? Floor { get; init; }
    public string? Building { get; init; }
    public string? Zone { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public string? Notes { get; init; }
}

public record ImportBoxesDto
{
    public Guid ProjectId { get; init; }
    public List<CreateBoxDto> Boxes { get; init; } = new();
}

public record ImportBoxFromExcelDto
{
    public string BoxTag { get; init; } = string.Empty;
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public string Floor { get; init; } = string.Empty;
    public string? Building { get; init; }
    public string? Zone { get; init; }
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public string? BIMModelReference { get; init; }
    public string? Notes { get; init; }
}

public record BoxImportResultDto
{
    public int SuccessCount { get; init; }
    public int FailureCount { get; init; }
    public List<string> Errors { get; init; } = new();
    public List<BoxDto> ImportedBoxes { get; init; } = new();
}

