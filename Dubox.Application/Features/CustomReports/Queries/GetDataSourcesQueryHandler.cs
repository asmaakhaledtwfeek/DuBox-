using Dubox.Application.DTOs;
using Dubox.Application.Features.CustomReports.DataSources;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

public class GetDataSourcesQueryHandler : IRequestHandler<GetDataSourcesQuery, Result<List<DataSourceDefinitionDto>>>
{
    public Task<Result<List<DataSourceDefinitionDto>>> Handle(GetDataSourcesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success(DataSourceRegistry.GetAll()));
    }
}
