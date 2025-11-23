using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxByIdWithIncludesSpecification : Specification<Box>
    {
        public GetBoxByIdWithIncludesSpecification(Guid boxId)
        {
            AddCriteria(box => box.BoxId == boxId);
            AddInclude(nameof(Box.Project));
            AddInclude(nameof(Box.BoxAssets));
            AddInclude(nameof(Box.BoxActivities));
            AddInclude(nameof(Box.ProgressUpdates));
        }
    }
}
