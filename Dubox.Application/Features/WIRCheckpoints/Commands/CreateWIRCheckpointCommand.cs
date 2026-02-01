using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public record CreateWIRCheckpointCommand(
    Guid BoxActivityId,
    string WIRNumber,
    string? WIRName,
    Guid? InspectorId,
    string? WIRDescription,
    string? AttachmentPath,
    string? Comments,
    List<IFormFile>? Files,
    List<string>? ImageUrls,
    List<string>? FileNames = null
) : IRequest<Result<CreateWIRCheckpointDto>>;
}
