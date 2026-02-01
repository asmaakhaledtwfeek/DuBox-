using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetPredefinedItemsByCategorySpecification : Specification<PredefinedChecklistItem>
{
    public GetPredefinedItemsByCategorySpecification(List<Guid> predefinedItemIds)
    {
        AddCriteria(p => predefinedItemIds.Contains(p.PredefinedItemId));
        AddOrderBy(p => p.Sequence);
    }
}
