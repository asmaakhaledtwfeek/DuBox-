
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class TeamMembersByUserIdsSpecification : Specification<TeamMember>
{
    public TeamMembersByUserIdsSpecification(Guid teamId)
    {
        AddCriteria(tm => tm.TeamId == teamId && tm.IsActive == true);
        AddInclude(nameof(TeamMember.Team));
        AddInclude(nameof(TeamMember.User));
      
    }
}

