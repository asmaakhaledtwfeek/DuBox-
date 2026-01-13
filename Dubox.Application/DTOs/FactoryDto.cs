using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs;

public record FactoryDto
{
    public Guid FactoryId { get; init; }
    public string FactoryCode { get; init; } = string.Empty;
    public string FactoryName { get; init; } = string.Empty;
    public ProjectLocationEnum Location { get; init; }
    public int? Capacity { get; init; }
    public int CurrentOccupancy { get; init; }
    public int AvailableCapacity { get; init; }
    public bool IsFull { get; init; }
    public bool IsActive { get; init; }
    public int DispatchedBoxesCount { get; init; } // Count of dispatched boxes in the factory
}

