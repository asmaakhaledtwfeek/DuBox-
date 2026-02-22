using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

/// <summary>
/// Executes a custom report using an inline config (for live preview, not requiring a saved report).
/// </summary>
public record ExecuteCustomReportQuery(
    CustomReportConfig Config,
    int Page = 1,
    int PageSize = 50
) : IRequest<Result<ReportExecutionResultDto>>;
