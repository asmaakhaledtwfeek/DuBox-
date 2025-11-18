using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands
{
    public record UpdateProjectStatusCommand(
    Guid ProjectId,
    int Status
) : IRequest<Result<ProjectDto>>;
}
