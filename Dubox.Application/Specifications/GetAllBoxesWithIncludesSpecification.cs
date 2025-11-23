using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetAllBoxesWithIncludesSpecification : Specification<Box>
    {
        public GetAllBoxesWithIncludesSpecification()
        {
            AddInclude(nameof(Box.Project));
            AddInclude(nameof(Box.BoxAssets));
            AddInclude(nameof(Box.BoxActivities));
            AddInclude(nameof(Box.ProgressUpdates));
            AddOrderByDescending(x => x.CreatedDate);
        }
    }
}
