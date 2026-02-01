using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries;

public record ExportTeamsPerformanceReportQuery : IRequest<Result<Stream>>
{
    public Guid? ProjectId { get; init; }
    public Guid? TeamId { get; init; }
    public int? Status { get; init; }
    public string? Search { get; init; }
}

