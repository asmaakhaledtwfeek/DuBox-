using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public record GetAllTeamsQuery : IRequest<Result<List<TeamDto>>>;

