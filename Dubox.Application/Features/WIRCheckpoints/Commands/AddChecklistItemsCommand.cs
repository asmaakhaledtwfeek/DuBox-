using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public record AddChecklistItemsCommand(
        Guid WIRId,
        List<Guid> PredefinedItemIds
    ) : IRequest<Result<CreateWIRCheckpointDto>>;
}
