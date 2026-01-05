using Dubox.Application.Features.Projects.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetProjectsSpecification : Specification<Project>
    {
        public GetProjectsSpecification(GetAllProjectsQuery query, List<Guid>? accessibleProjectIds)
        {
            AddCriteria(p => p.IsActive);
          
            if (accessibleProjectIds != null)
            {
                AddCriteria(p => accessibleProjectIds.Contains(p.ProjectId));
            }

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTermLower = query.SearchTerm.ToLower().Trim();
                AddCriteria(p =>
                 (p.ProjectCode != null && p.ProjectCode.ToLower().Contains(searchTermLower)) ||
                 (p.ProjectName != null && p.ProjectName.ToLower().Contains(searchTermLower)));
            }
            if (query.StatusFilter.HasValue)
            {
                var targetStatus = (ProjectStatusEnum)query.StatusFilter.Value;
                AddCriteria(p => p.Status == targetStatus);
            }

            AddOrderByDescending(p => p.CreatedDate);

        }
    }
}
