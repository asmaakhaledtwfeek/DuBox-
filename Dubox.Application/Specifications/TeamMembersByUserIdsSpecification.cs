
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

public class TeamMembersByUserIdsSpecification : Specification<TeamMember>
{
    public TeamMembersByUserIdsSpecification(int teamId)
    {
        AddCriteria(tm => tm.TeamId == teamId);

        AddInclude(nameof(TeamMember.User));
    }
}

