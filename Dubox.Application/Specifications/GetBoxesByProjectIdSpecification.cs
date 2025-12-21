using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxesByProjectIdSpecification : Specification<Box>
    {
        public GetBoxesByProjectIdSpecification(Guid projectId)
        {
            AddCriteria(b => b.ProjectId == projectId);
            //AddInclude(nameof(Box.BoxActivities));
            AddInclude(nameof(Box.Project));
            AddInclude(nameof(Box.BoxType));
            AddInclude(nameof(Box.BoxSubType));
            AddInclude(nameof(Box.Factory));
            AddInclude(nameof(Box.CurrentLocation));

            // Enable split query to avoid Cartesian explosion with BoxActivities collection
            EnableSplitQuery();
        }
    }
}
