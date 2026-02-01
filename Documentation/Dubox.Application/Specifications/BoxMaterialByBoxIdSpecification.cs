using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{

    public class BoxMaterialByBoxIdSpecification : Specification<BoxMaterial>
    {
        public BoxMaterialByBoxIdSpecification(Guid boxId)
        {
            AddCriteria(bm => bm.BoxId == boxId);
            AddInclude(nameof(BoxMaterial.Material));
            AddInclude(nameof(BoxMaterial.Box));
        }
    }
}
