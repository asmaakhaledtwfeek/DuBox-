using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public record UpdateTeamGroupCommand(
    Guid TeamGroupId,
    string GroupTag,
    string GroupType,
    bool IsActive
) : IRequest<Result<TeamGroupDto>>;

