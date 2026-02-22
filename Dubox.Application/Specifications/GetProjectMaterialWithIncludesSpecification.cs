using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetProjectMaterialWithIncludesSpecification : Specification<ProjectMaterial>
    {
        public GetProjectMaterialWithIncludesSpecification(Guid projectMaterialId)
        {
            AddCriteria(pm => pm.ProjectMaterialId == projectMaterialId);
            AddInclude(nameof(ProjectMaterial.Material));
            AddInclude(nameof(ProjectMaterial.Project));
        }
    }
}






