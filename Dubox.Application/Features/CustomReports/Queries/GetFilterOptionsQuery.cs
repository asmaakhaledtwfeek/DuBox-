using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

/// <summary>
/// Returns distinct filterable values for a given field in a data source,
/// used to populate dropdown options in the report builder filter UI.
/// </summary>
public record GetFilterOptionsQuery(string DataSource, string Field) : IRequest<List<string>>;
