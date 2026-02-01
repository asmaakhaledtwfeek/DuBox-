using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public record UpdateTeamCommand(
    Guid TeamId,
    string? TeamCode,
    string? TeamName,
    Guid? DepartmentId,
    string? Trade,
    bool? IsActive
) : IRequest<Result<TeamDto>>;


