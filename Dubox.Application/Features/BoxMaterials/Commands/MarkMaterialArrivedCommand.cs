using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxMaterials.Commands;

public record MarkMaterialArrivedCommand(
    Guid BoxId,
    Guid MaterialId,
    bool IsArrived
) : IRequest<Result<BoxMaterialDto>>;

public record BoxMaterialDto
{
    public Guid BoxMaterialId { get; init; }
    public Guid BoxId { get; init; }
    public string? BoxTag { get; init; }
    public Guid MaterialId { get; init; }
    public string MaterialCode { get; init; } = string.Empty;
    public string MaterialName { get; init; } = string.Empty;
    public string? MaterialCategory { get; init; }
    public int RequiredBeforeDays { get; init; }
    public DateTime RequiredByDate { get; init; }
    public bool IsArrived { get; init; }
    public DateTime? ArrivedDate { get; init; }
    public int DaysUntilRequired { get; init; }
    public bool IsOverdue { get; init; }
    public string Status { get; init; } = string.Empty;
    public int? DeliveredQuantity { get; init; }
    public int? QuantityPerBox { get; init; }
}






