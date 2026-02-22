using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public record MarkBoxTypeMaterialPendingCommand(
    Guid BoxTypeMaterialId
) : IRequest<Result<bool>>;






