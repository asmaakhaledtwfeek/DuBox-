using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetTeamGroupWithIncludesSpecification : Specification<TeamGroup>
{
    public GetTeamGroupWithIncludesSpecification(Guid teamGroupId)
    {
        AddCriteria(tg => tg.TeamGroupId == teamGroupId);

        AddInclude(nameof(TeamGroup.Team));
        AddInclude(nameof(TeamGroup.Members));
        
        // Enable split query to avoid Cartesian explosion with Members collection
        EnableSplitQuery();
    }

    public GetTeamGroupWithIncludesSpecification()
    {
        AddInclude(nameof(TeamGroup.Team));
        AddInclude(nameof(TeamGroup.Members));
        
        // Enable split query to avoid Cartesian explosion with Members collection
        EnableSplitQuery();
    }
}

