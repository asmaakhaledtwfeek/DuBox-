
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class TeamMembersByUserIdsSpecification : Specification<TeamMember>
{
    public TeamMembersByUserIdsSpecification(Guid teamId , bool usersOnly)
    {
        AddCriteria(tm => tm.TeamId == teamId && tm.IsActive == true  );
        if (usersOnly)
            AddCriteria(tm => tm.UserId != null);
       // AddInclude(nameof(TeamMember.Team));
        AddInclude(nameof(TeamMember.User));
      
    }
}

