namespace Dubox.Application.DTOs;

/// <summary>
/// Read-only DTO for public box view (accessed via QR code scan)
/// Contains only essential information for public viewing
/// </summary>
public record PublicBoxDto
{
    public Guid BoxId { get; init; }
    public Guid? ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string ClientName { get; init; } = string.Empty;
    public string BoxTag { get; init; } = string.Empty;
    public string? SerialNumber { get; init; }
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public string? BoxSubTypeName { get; init; }
    public string? Floor { get; init; }
    public string? BuildingNumber { get; init; }
    public string? BoxFunction { get; init; }
    public string? Zone { get; init; }
    public decimal ProgressPercentage { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public string? UnitOfMeasure { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? ActualStartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public int ActivitiesCount { get; init; }
    public string? CurrentLocationName { get; init; }
    public string? FactoryName { get; init; }
    public string? Bay { get; init; }
    public string? Row { get; init; }
    public string? Position { get; init; }
}

