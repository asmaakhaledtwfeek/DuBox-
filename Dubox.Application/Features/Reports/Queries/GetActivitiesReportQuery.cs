using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries;


public record GetActivitiesReportQuery : IRequest<Result<PaginatedActivitiesReportResponseDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
    public Guid? ProjectId { get; init; }
    public Guid? TeamId { get; init; }
    public int? Status { get; init; }
    public DateTime? PlannedStartDateFrom { get; init; }
    public DateTime? PlannedStartDateTo { get; init; }
    public DateTime? PlannedEndDateFrom { get; init; }
    public DateTime? PlannedEndDateTo { get; init; }
    public string? Search { get; init; }
}

