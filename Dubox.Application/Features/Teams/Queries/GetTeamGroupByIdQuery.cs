using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public record GetTeamGroupByIdQuery(Guid TeamGroupId) : IRequest<Result<TeamGroupDto>>;

