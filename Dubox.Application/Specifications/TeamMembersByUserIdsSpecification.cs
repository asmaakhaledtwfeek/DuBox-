
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

public class TeamMembersByUserIdsSpecification : Specification<TeamMember>
{
    public TeamMembersByUserIdsSpecification(Guid teamId)
    {
        AddCriteria(tm => tm.TeamId == teamId && tm.IsActive == true);

        AddInclude(nameof(TeamMember.User));
    }
}

