using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxesByProjectIdSpecification : Specification<Box>
    {
        public GetBoxesByProjectIdSpecification(Guid projectId)
        {
            AddCriteria(b => b.ProjectId == projectId);
            AddCriteria(b => b.IsActive);
            //AddInclude(nameof(Box.BoxActivities));
            AddInclude(nameof(Box.Project));
            // Note: BoxType and BoxSubType navigation properties are ignored
            // BoxTypeId/BoxSubTypeId now reference ProjectBoxTypes/ProjectBoxSubTypes
            AddInclude(nameof(Box.Factory));
            AddInclude(nameof(Box.CurrentLocation));

            // Enable split query to avoid Cartesian explosion with BoxActivities collection
            EnableSplitQuery();
        }
    }
}
