using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{
    public record RemoveTeamMemberCommand(
        Guid TeamId,
        Guid TeamMemberId
    ) : IRequest<Result<bool>>;
}


