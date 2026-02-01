using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public record GetTeamByIdQuery(Guid TeamId) : IRequest<Result<TeamDto>>;


