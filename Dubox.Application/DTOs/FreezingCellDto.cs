namespace Dubox.Application.DTOs;

public record FreezingCellDto
{
    public Guid FreezingCellId { get; init; }
    public Guid FactorySectionPartId { get; init; }
    public int RowNumber { get; init; }
}

public record CreateFreezingCellDto(
    Guid FactorySectionPartId,
    int RowNumber
);
