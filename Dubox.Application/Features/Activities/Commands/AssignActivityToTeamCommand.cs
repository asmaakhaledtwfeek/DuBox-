using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands
{
    public record AssignActivityToTeamCommand
    (
       Guid BoxActivityId,
       int TeamId,
       Guid TeamMemberId
    ) : IRequest<Result<AssignBoxActivityTeamDto>>;


}
