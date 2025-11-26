using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public record AddChecklistItemsCommand(
    Guid WIRId,
      List<ChecklistItemForCreate> Items
) : IRequest<Result<CreateWIRCheckpointDto>>;

    public record ChecklistItemForCreate(
    string CheckpointDescription,
    string? ReferenceDocument,
    int Sequence
);
}
