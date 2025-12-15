using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetPredefinedItemsByCategorySpecification : Specification<PredefinedChecklistItem>
{
    public GetPredefinedItemsByCategorySpecification(List<Guid> predefinedItemIds)
    {
        AddCriteria(p => predefinedItemIds.Contains(p.PredefinedItemId));
        AddInclude(nameof(PredefinedChecklistItem.Category));
        AddInclude(nameof(PredefinedChecklistItem.Reference));
        AddOrderBy(p => p.Sequence);
    }
}
