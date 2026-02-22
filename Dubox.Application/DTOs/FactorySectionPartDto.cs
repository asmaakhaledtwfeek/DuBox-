namespace Dubox.Application.DTOs;

public record FactorySectionPartDto
{
    public Guid PartId { get; init; }
    public Guid SectionId { get; init; }
    public int PartNumber { get; init; }
    public string PartName { get; init; } = string.Empty;
    public int MinRow { get; init; }
    public int MaxRow { get; init; }
    public string MinBay { get; init; } = string.Empty;
    public string MaxBay { get; init; } = string.Empty;
    public int? Capacity { get; init; }
    public int CurrentOccupancy { get; init; }
    public int AvailableCapacity { get; init; }
    public bool IsFull { get; init; }
    public bool IsActive { get; init; }
    public int RowCount { get; init; }
    public int BayCount { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public List<FreezingCellDto> FreezingCells { get; init; } = new();
}

public record CreateFactorySectionPartDto(
    Guid SectionId,
    int PartNumber,
    string PartName,
    int MinRow,
    int MaxRow,
    string MinBay,
    string MaxBay,
    int? Capacity = null
);

public record UpdateFactorySectionPartDto
{
    public Guid PartId { get; init; }
    public int PartNumber { get; init; }
    public string PartName { get; init; } = string.Empty;
    public int MinRow { get; init; }
    public int MaxRow { get; init; }
    public string MinBay { get; init; } = string.Empty;
    public string MaxBay { get; init; } = string.Empty;
    public int? Capacity { get; init; }
    public bool IsActive { get; init; }
}

