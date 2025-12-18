using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxTypesByCategorySpecification : Specification<BoxType>
    {
        public GetBoxTypesByCategorySpecification(int categoryId)
        {
            AddCriteria(bt => bt.CategoryId == categoryId);
            AddInclude(nameof(BoxType.BoxSubTypes));
            AddOrderBy(bt => bt.BoxTypeName);
        }
    }
}

