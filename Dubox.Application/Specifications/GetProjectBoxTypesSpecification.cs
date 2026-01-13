using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dubox.Application.Specifications
{
    public class GetProjectBoxTypesSpecification:Specification<ProjectBoxType>
    {
        public GetProjectBoxTypesSpecification(Guid projectId)
        {
            AddCriteria(pbt => pbt.ProjectId == projectId);
            AddInclude(nameof(ProjectBoxType.SubTypes));
            AddOrderBy(t => t.DisplayOrder);
        }
    }
}
