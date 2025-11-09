using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public record CreateTeamCommand(
    string TeamCode,
    string TeamName,
    string? Department,
    string? Trade,
    string? TeamLeaderName,
    int? TeamSize
) : IRequest<Result<TeamDto>>;

