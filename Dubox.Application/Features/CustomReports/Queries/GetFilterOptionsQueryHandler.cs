using Dubox.Application.DTOs;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

public class GetFilterOptionsQueryHandler : IRequestHandler<GetFilterOptionsQuery, List<string>>
{
    private readonly IMediator _mediator;

    public GetFilterOptionsQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<string>> Handle(GetFilterOptionsQuery request, CancellationToken cancellationToken)
    {
        // Execute the report fetching only the requested column (large page to get all distinct values)
        var config = new CustomReportConfig
        {
            DataSource = request.DataSource,
            Columns   = new List<string> { request.Field },
            Filters   = new List<ReportFilterConfig>(),
            ChartType = "table"
        };

        var result = await _mediator.Send(
            new ExecuteCustomReportQuery(config, Page: 1, PageSize: 2000),
            cancellationToken);

        var values = result.Data.Rows
            .Where(r => r.ContainsKey(request.Field) && r[request.Field] != null)
            .Select(r => r[request.Field]?.ToString() ?? string.Empty)
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
            .Take(200)
            .ToList();

        return values;
    }
}
