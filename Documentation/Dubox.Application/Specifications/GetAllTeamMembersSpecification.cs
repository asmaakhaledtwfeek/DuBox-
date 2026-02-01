using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public class GetAllTeamMembersSpecification:Specification<TeamMember>
    {
        public GetAllTeamMembersSpecification()
        {
            AddCriteria(tm => tm.IsActive && tm.Team != null );
            AddInclude(nameof(TeamMember.User));
            AddInclude(nameof(TeamMember.Team));
            AddOrderBy(tm => tm.Team.TeamCode);
            AddOrderBy(tm => tm.EmployeeName ?? tm.User!.FullName);
        }
    }
}
