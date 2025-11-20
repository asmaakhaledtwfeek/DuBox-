using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxesByProjectIdSpecification : Specification<Box>
    {
        public GetBoxesByProjectIdSpecification(Guid projectId)
        {
            AddCriteria(b => b.ProjectId == projectId);
            AddInclude(nameof(Box.BoxActivities));
            AddInclude(nameof(Box.Project));
        }
    }
}
