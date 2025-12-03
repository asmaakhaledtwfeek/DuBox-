using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public record ReviewWIRCheckPointCommand(
      Guid WIRId,
      WIRCheckpointStatusEnum Status,
      string? Comment,
      string? InspectorRole,
      string? AttachmentPath,
      List<ChecklistItemForReview> Items
 ) : IRequest<Result<WIRCheckpointDto>>;

    public record ChecklistItemForReview(
     Guid ChecklistItemId,
    string? Remarks,
    CheckListItemStatusEnum Status
);
}
