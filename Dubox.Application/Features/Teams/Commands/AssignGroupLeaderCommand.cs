using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public record AssignGroupLeaderCommand(
    Guid TeamGroupId,
    Guid TeamMemberId
) : IRequest<Result<TeamGroupDto>>;

