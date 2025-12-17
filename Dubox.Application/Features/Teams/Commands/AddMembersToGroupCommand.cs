using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public record AddMembersToGroupCommand(
    Guid TeamGroupId,
    List<Guid> TeamMemberIds
) : IRequest<Result<TeamGroupDto>>;

