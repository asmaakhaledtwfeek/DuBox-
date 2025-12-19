using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetAllBoxesWithIncludesSpecification : Specification<Box>
    {
        public GetAllBoxesWithIncludesSpecification(List<Guid>? accessibleProjectIds = null)
        {
            AddInclude(nameof(Box.Project));
            AddInclude(nameof(Box.BoxType));
            AddInclude(nameof(Box.BoxAssets));
            AddInclude(nameof(Box.BoxActivities));
            AddInclude(nameof(Box.ProgressUpdates));
            AddOrderByDescending(x => x.CreatedDate);
            
            // Apply visibility filtering based on accessible projects
            // null means access to all projects (SystemAdmin/Viewer)
            if (accessibleProjectIds != null)
            {
                AddCriteria(b => accessibleProjectIds.Contains(b.ProjectId));
            }
            
            // Enable split query to avoid Cartesian explosion with multiple collection includes
            EnableSplitQuery();
        }
    }
}
