namespace Dubox.Application.DTOs;

public record FactoryLocationDto
{
    public Guid LocationId { get; init; }
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
    public Guid LocationId { get; init; }
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
    public Guid HistoryId { get; init; }
    public Guid BoxId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public string? SerialNumber { get; init; }
    public Guid LocationId { get; init; }
    public string LocationCode { get; init; } = string.Empty;
    public string LocationName { get; init; } = string.Empty;
    public Guid? MovedFromLocationId { get; init; }
    public string? MovedFromLocationCode { get; init; }
    public string? MovedFromLocationName { get; init; }
    public DateTime MovedDate { get; init; }
    public string? Reason { get; init; }
    public Guid? MovedBy { get; init; }
    public string? MovedByUsername { get; init; }
    public string? MovedByFullName { get; init; }
}


public record LocationBoxStatusCountDto
{
    public string Status { get; init; } = string.Empty;
    public int Count { get; init; }
}

public record LocationBoxesDto
{
    public Guid LocationId { get; init; }
    public string LocationCode { get; init; } = string.Empty;
    public string LocationName { get; init; } = string.Empty;
    public List<BoxDto> Boxes { get; init; } = new();
    public List<LocationBoxStatusCountDto> StatusCounts { get; init; } = new();
    public int TotalBoxes { get; init; }
}

