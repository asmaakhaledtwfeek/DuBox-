using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{

    public record AssignedTeamMembersCommand(
    int TeamId,
    List<Guid>? UserIds
) : IRequest<Result<TeamMembersDto>>;
}
