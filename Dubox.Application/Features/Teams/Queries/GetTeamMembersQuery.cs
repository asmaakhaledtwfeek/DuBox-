using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries
{
    public record GetTeamMembersQuery(int TeamId) : IRequest<Result<TeamMembersDto>>;

}
