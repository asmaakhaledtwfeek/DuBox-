using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

/// <summary>
/// Exports all rows of a custom report to an Excel stream.
/// </summary>
public record ExportCustomReportQuery(
    CustomReportConfig Config
) : IRequest<Result<Stream>>;
