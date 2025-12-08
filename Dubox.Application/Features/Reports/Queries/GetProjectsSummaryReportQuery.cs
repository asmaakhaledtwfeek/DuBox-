using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries;

public record GetProjectsSummaryReportQuery : IRequest<Result<ProjectsSummaryReportResponseDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 25;

    public bool? IsActive { get; init; }
    public List<int>? Status { get; init; }
    public string? Search { get; init; }
}

