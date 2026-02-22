using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

/// <summary>Returns the metadata manifest for all available data sources.</summary>
public record GetDataSourcesQuery : IRequest<Result<List<DataSourceDefinitionDto>>>;
