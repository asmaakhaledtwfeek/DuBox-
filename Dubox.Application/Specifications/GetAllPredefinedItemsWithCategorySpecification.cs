using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetAllPredefinedItemsWithCategorySpecification : Specification<PredefinedChecklistItem>
{
    public GetAllPredefinedItemsWithCategorySpecification()
    {
        AddCriteria(p => p.IsActive);
        AddInclude(nameof(PredefinedChecklistItem.Category));
        AddInclude(nameof(PredefinedChecklistItem.Reference));
        AddOrderBy(p => p.Sequence);
    }
}
