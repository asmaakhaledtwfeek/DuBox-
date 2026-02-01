using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries;

public record GetTeamsPerformanceSummaryQuery : IRequest<Result<TeamsPerformanceSummaryDto>>
{
    public Guid? ProjectId { get; init; }
    public Guid? TeamId { get; init; }
    public int? Status { get; init; }
    public string? Search { get; init; }
}

