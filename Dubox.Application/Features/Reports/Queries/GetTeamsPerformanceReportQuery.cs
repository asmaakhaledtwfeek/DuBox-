using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries;

public record GetTeamsPerformanceReportQuery : IRequest<Result<PaginatedTeamsPerformanceResponseDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 25;
    public Guid? ProjectId { get; init; }
    public Guid? TeamId { get; init; }
    public int? Status { get; init; }
    public string? Search { get; init; }
}

