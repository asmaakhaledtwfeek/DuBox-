using MediatR;
using Dubox.Domain.Shared;

namespace Dubox.Application.Features.Cost.Queries;

public class GetDistinctCostTypesQuery : IRequest<Result<List<CostTypeDto>>>
{
}

public record CostTypeDto(
    Guid HRCostRecordId,
    string CostType
);

