using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public record UpdateBoxTypeMaterialQuantityCommand(
    Guid BoxTypeMaterialId,
    int QuantityPerBox
) : IRequest<Result<bool>>;
