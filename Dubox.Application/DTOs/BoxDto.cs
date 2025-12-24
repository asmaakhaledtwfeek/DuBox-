using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs;

public record BoxDto
{
    public Guid BoxId { get; init; }
    public Guid ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string Client { get; init; } = string.Empty;
    public string BoxTag { get; init; } = string.Empty;
    public string? SerialNumber { get; init; }
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public int? BoxTypeId { get; init; }
    public int? BoxSubTypeId { get; init; }
    public string? BoxSubTypeName { get; init; }
    public string? Floor { get; init; }
    public string? BuildingNumber { get; init; }
    public string? BoxFunction { get; init; }
    public string? Zone { get; init; }
    public string QRCodeString { get; init; } = string.Empty;
    public string? QRCodeImage { get; init; } = string.Empty;
    public decimal ProgressPercentage { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public string? UnitOfMeasure { get; init; }
    public string? RevitElementId { get; init; }
    public int? Duration { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? ActualStartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public DateTime CreatedDate { get; init; }
    public int ActivitiesCount { get; init; }
    public string? Notes { get; init; }
    public Guid? CurrentLocationId { get; init; }
    public string? CurrentLocationCode { get; init; }
    public string? CurrentLocationName { get; init; }
    public Guid? FactoryId { get; init; }
    public string? FactoryCode { get; init; }
    public string? FactoryName { get; init; }
    public string? Bay { get; init; }
    public string? Row { get; init; }
    public string? Position { get; init; }
}

public record CreateBoxDto
{
    public Guid ProjectId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public string? Floor { get; init; }
    public string? BuildingNumber { get; init; }
    public string? BoxFunction { get; init; }
    public string? Zone { get; init; }
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public string? RevitElementId { get; init; }
    public Guid? FactoryId { get; init; }
    public List<CreateBoxAssetDto>? Assets { get; init; }
}

public record UpdateBoxDto
{
    public Guid BoxId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public string? Floor { get; init; }
    public string? BuildingNumber { get; init; }
    public string? BoxFunction { get; init; }
    public string? Zone { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public string? Notes { get; init; }
    public Guid? FactoryId { get; init; }
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
    public string? BoxSubType { get; init; }
    public string Floor { get; init; } = string.Empty;
    public string? BuildingNumber { get; init; }
    public string? BoxFunction { get; init; }
    public string? Zone { get; init; } // Changed to string to send zone display name instead of enum number
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public string? Notes { get; init; }
    
}

public record BoxImportResultDto
{
    public int SuccessCount { get; init; }
    public int FailureCount { get; init; }
    public List<string> Errors { get; init; } = new();
    public List<BoxDto> ImportedBoxes { get; init; } = new();
}

public record BoxLogDto
{
    public Guid Id { get; init; }
    public Guid BoxId { get; init; }
    public string Action { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? TableName { get; init; } // Table name from audit log (e.g., "Box", "QualityIssue")
    public string? EntityDisplayName { get; init; } // Display name of the entity
    public string? Field { get; init; }
    public string? OldValue { get; init; }
    public string? NewValue { get; init; }
    public string? OldValues { get; init; } // JSON string of all old values
    public string? NewValues { get; init; } // JSON string of all new values
    public string PerformedBy { get; init; } = string.Empty;
    public DateTime PerformedAt { get; init; }
}

