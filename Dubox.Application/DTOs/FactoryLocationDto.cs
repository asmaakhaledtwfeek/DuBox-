namespace Dubox.Application.DTOs;

public record FactoryLocationDto
{
    public int LocationId { get; init; }
    public string LocationCode { get; init; } = string.Empty;
    public string LocationName { get; init; } = string.Empty;
    public string? LocationType { get; init; }
    public string? Bay { get; init; }
    public string? Row { get; init; }
    public string? Position { get; init; }
    public int? Capacity { get; init; }
    public int CurrentOccupancy { get; init; }
    public int AvailableCapacity { get; init; }
    public bool IsFull { get; init; }
    public bool IsActive { get; init; }
}

public record CreateFactoryLocationDto
{
    public string LocationCode { get; init; } = string.Empty;
    public string LocationName { get; init; } = string.Empty;
    public string? LocationType { get; init; }
    public string? Bay { get; init; }
    public string? Row { get; init; }
    public string? Position { get; init; }
    public int? Capacity { get; init; }
}

public record UpdateFactoryLocationDto
{
    public int LocationId { get; init; }
    public string LocationCode { get; init; } = string.Empty;
    public string LocationName { get; init; } = string.Empty;
    public string? LocationType { get; init; }
    public string? Bay { get; init; }
    public string? Row { get; init; }
    public string? Position { get; init; }
    public int? Capacity { get; init; }
    public bool IsActive { get; init; }
}

public record BoxLocationHistoryDto
{
    public Guid BoxLocationHistoryId { get; init; }
    public Guid BoxId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public int LocationId { get; init; }
    public string LocationName { get; init; } = string.Empty;
    public int? MovedFromLocationId { get; init; }
    public string? MovedFromLocationName { get; init; }
    public DateTime MovedInDate { get; init; }
    public DateTime? MovedOutDate { get; init; }
    public string? Reason { get; init; }
    public string? MovedBy { get; init; }
}

public record MoveBoxToLocationDto
{
    public Guid BoxId { get; init; }
    public int ToLocationId { get; init; }
    public string? Reason { get; init; }
}

