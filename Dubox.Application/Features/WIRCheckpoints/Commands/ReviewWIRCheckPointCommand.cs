using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public record ReviewWIRCheckPointCommand(
      Guid WIRId,
      WIRCheckpointStatusEnum Status,
      string? Comment,
      string? InspectorRole,
      List<IFormFile>? Files,
      List<string>? ImageUrls,
      List<ChecklistItemForReview> Items,
      List<string>? FileNames = null
 ) : IRequest<Result<WIRCheckpointDto>>;

    public record ChecklistItemForReview(
     Guid ChecklistItemId,
    string? Remarks,
    CheckListItemStatusEnum Status
);
}
