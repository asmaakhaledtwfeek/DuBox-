using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetProjectByIdSpecification : Specification<Project>
    {
        public GetProjectByIdSpecification(Guid projectId)
        {
            AddCriteria(p => p.ProjectId == projectId);
            AddInclude(nameof(Project.Category));
        }
    }
}


