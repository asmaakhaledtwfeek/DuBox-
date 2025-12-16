using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public record CreateTeamGroupCommand(
    Guid TeamId,
    string GroupTag,
    string GroupType
) : IRequest<Result<TeamGroupDto>>;

