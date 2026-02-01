using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public record ReactivateTeamMemberCommand(
    Guid TeamId,
    Guid TeamMemberId
) : IRequest<Result<TeamMemberDto>>;

