using Dubox.Application.DTOs;
using Dubox.Application.Features.Cost.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public class ProjectCostSpecification:Specification<ProjectCost>
    {
        public ProjectCostSpecification(GetProjectCostsByProjectIdQuery query)
        {
            AddCriteria(pc=>pc.ProjectId == query.ProjectId);
            if (!string.IsNullOrEmpty( query.CostType)) 
                AddCriteria(pc=>pc.CostType ==query.CostType);
            AddInclude(nameof(ProjectCost.Box));
            AddInclude(nameof(ProjectCost.HRCost));
            AddOrderByDescending(pc => pc.CreatedDate);
            var (page, pageSize) = new PaginatedRequest
            {
                Page = query.PageNumber,
                PageSize = query.PageSize
            }.GetNormalizedPagination();

        }
    }
}
