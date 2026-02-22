using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class ProjectMaterialByProjectAndMaterialIdSpecification : Specification<ProjectMaterial>
    {
        public ProjectMaterialByProjectAndMaterialIdSpecification(Guid projectId, Guid materialId)
        {
            AddCriteria(pm => pm.ProjectId == projectId && pm.MaterialId == materialId);
            AddInclude(nameof(ProjectMaterial.Material));
            AddInclude(nameof(ProjectMaterial.Project));
        }
    }
}






