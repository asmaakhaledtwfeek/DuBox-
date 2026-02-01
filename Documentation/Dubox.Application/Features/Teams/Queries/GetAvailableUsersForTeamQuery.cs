using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public record GetAvailableUsersForTeamQuery(Guid TeamId) : IRequest<Result<List<UserDto>>>;

