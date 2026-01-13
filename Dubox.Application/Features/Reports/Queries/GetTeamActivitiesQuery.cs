using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries;

public record GetTeamActivitiesQuery : IRequest<Result<TeamActivitiesResponseDto>>
{
    public Guid TeamId { get; init; }
    public Guid? ProjectId { get; init; }
    public int? Status { get; init; }
}

