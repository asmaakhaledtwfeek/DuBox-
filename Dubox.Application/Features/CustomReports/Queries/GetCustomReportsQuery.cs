using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

public record GetCustomReportsQuery(
    int Page = 1,
    int PageSize = 50,
    string? Search = null
) : IRequest<Result<List<CustomReportListItemDto>>>;
