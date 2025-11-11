using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetTeamMemberWithIcludesSpecification : Specification<TeamMember>
    {
        public GetTeamMemberWithIcludesSpecification(Guid teamMemberId)
        {
            AddCriteria(tm => tm.TeamMemberId == teamMemberId);
            AddInclude(nameof(TeamMember.User));
            AddInclude(nameof(TeamMember.Team));

        }
    }
}
