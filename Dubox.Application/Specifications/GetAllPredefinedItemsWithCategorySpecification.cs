using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetAllPredefinedItemsWithCategorySpecification : Specification<PredefinedChecklistItem>
{
    public GetAllPredefinedItemsWithCategorySpecification()
    {
        AddCriteria(p => p.IsActive);
        AddOrderBy(p => p.Sequence);
    }
}
