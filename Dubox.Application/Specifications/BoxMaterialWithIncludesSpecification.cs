using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class BoxMaterialWithIncludesSpecification : Specification<BoxMaterial>
    {
        public BoxMaterialWithIncludesSpecification(Guid boxMaterialId)
        {
            AddCriteria(bm => bm.BoxMaterialId == boxMaterialId);
            AddInclude(nameof(BoxMaterial.Material));
            AddInclude(nameof(BoxMaterial.Box));

        }
    }
}
