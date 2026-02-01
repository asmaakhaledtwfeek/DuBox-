using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class DeleteBoxWithIncludesSpecification : Specification<Box>
    {
        public DeleteBoxWithIncludesSpecification(Guid boxId)
        {
            AddCriteria(box => box.BoxId == boxId);

            AddInclude(nameof(Box.BoxAssets));
            AddInclude(nameof(Box.BoxActivities));
            AddInclude(nameof(Box.ProgressUpdates));
        }
    }
}
