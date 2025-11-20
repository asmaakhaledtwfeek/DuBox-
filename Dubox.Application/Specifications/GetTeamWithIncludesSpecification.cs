using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetTeamWithIncludesSpecification : Specification<Team>
    {
        public GetTeamWithIncludesSpecification(Guid teamId)
        {
            AddCriteria(team => team.TeamId == teamId);

            AddInclude(nameof(Team.Department));
            AddInclude(nameof(Team.TeamLeader));
            AddInclude(nameof(Team.Members));
        }

    }
}
