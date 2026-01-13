using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public class GetInActiveTeamMembersWithIncludeSpecification:Specification<TeamMember>
    {
        public GetInActiveTeamMembersWithIncludeSpecification( Guid teamId, bool isActive)
        {
            AddCriteria(tm => tm.TeamId == teamId && tm.IsActive == isActive);
            AddInclude(nameof(TeamMember.User));
        }
    }
}
