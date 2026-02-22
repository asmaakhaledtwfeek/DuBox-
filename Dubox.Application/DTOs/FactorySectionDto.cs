using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs;

public record FactorySectionDto
{
    public Guid SectionId { get; init; }
    public Guid FactoryId { get; init; }
    public FactorySectionTypeEnum SectionType { get; init; }
    public string SectionName { get; init; } = string.Empty;
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
    public int DisplayOrder { get; init; }
    public List<FactorySectionPartDto> Parts { get; init; } = new();
}

public record CreateFactorySectionDto(
    FactorySectionTypeEnum SectionType,
    string SectionName,
    int MinRow,
    int MaxRow,
    string MinBay,
    string MaxBay
);

