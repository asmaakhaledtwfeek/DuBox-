using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public record CreateWIRCheckpointCommand(
    Guid BoxActivityId,
    string WIRNumber,
    string? WIRName,
    string? WIRDescription,
    string? AttachmentPath,
    string? Comments,
    List<byte[]>? Files,
    List<string>? ImageUrls
) : IRequest<Result<CreateWIRCheckpointDto>>;
}
