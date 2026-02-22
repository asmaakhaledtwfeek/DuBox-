using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public record MarkBoxTypeMaterialArrivedCommand(
    Guid BoxTypeMaterialId,
    int DeliveryProgress = 100,
    decimal? DeliveredQuantity = null,
    string? Notes = null
) : IRequest<Result<bool>>;






