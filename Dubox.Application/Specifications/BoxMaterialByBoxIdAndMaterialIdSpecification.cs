using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class BoxMaterialByBoxIdAndMaterialIdSpecification : Specification<BoxMaterial>
    {
        public BoxMaterialByBoxIdAndMaterialIdSpecification(Guid boxId, Guid materialId)
        {
            AddCriteria(bm => bm.BoxId == boxId && bm.MaterialId == materialId);
            AddInclude(nameof(BoxMaterial.Material));
            AddInclude(nameof(BoxMaterial.Box));
        }
    }
}
