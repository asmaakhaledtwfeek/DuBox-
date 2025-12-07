using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries;

/// <summary>
/// Query to export activities report to Excel - Same filters as GetActivitiesReportQuery
/// </summary>
public record ExportActivitiesReportQuery : IRequest<Result<Stream>>
{
    public Guid? ProjectId { get; init; }
    public Guid? TeamId { get; init; }
    public int? Status { get; init; }
    public DateTime? PlannedStartDateFrom { get; init; }
    public DateTime? PlannedStartDateTo { get; init; }
    public DateTime? PlannedEndDateFrom { get; init; }
    public DateTime? PlannedEndDateTo { get; init; }
    public string? Search { get; init; }
}

