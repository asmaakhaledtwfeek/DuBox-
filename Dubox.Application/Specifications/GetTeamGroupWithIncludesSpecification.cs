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
        AddInclude($"{nameof(TeamGroup.Members)}.{nameof(TeamMember.User)}");
        AddInclude(nameof(TeamGroup.GroupLeader));
        AddInclude($"{nameof(TeamGroup.GroupLeader)}.{nameof(TeamMember.User)}");

        // Enable split query to avoid Cartesian explosion with Members collection
        EnableSplitQuery();
    }

    public GetTeamGroupWithIncludesSpecification()
    {
        AddInclude(nameof(TeamGroup.Team));
        AddInclude(nameof(TeamGroup.Members));
        AddInclude($"{nameof(TeamGroup.Members)}.{nameof(TeamMember.User)}");
        AddInclude(nameof(TeamGroup.GroupLeader));
        AddInclude($"{nameof(TeamGroup.GroupLeader)}.{nameof(TeamMember.User)}");

        // Enable split query to avoid Cartesian explosion with Members collection
        EnableSplitQuery();
    }

    public GetTeamGroupWithIncludesSpecification(string? search, Guid? teamId, bool? isActive, int pageSize, int pageNumber)
    {
        AddInclude(nameof(TeamGroup.Team));
        AddInclude(nameof(TeamGroup.Members));
        AddInclude($"{nameof(TeamGroup.Members)}.{nameof(TeamMember.User)}");
        AddInclude(nameof(TeamGroup.GroupLeader));
        AddInclude($"{nameof(TeamGroup.GroupLeader)}.{nameof(TeamMember.User)}");

        // Enable split query to avoid Cartesian explosion with Members collection
        EnableSplitQuery();

        // Apply team filter
        if (teamId.HasValue && teamId.Value != Guid.Empty)
        {
            AddCriteria(tg => tg.TeamId == teamId.Value);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim().ToLowerInvariant();
            AddCriteria(tg =>
                tg.GroupTag.ToLower().Contains(searchTerm) ||
                tg.GroupType.ToLower().Contains(searchTerm) ||
                (tg.Team != null && tg.Team.TeamName.ToLower().Contains(searchTerm)) ||
                (tg.Team != null && tg.Team.TeamCode.ToLower().Contains(searchTerm))
            );
        }

        // Apply active filter
        if (isActive.HasValue)
        {
            AddCriteria(tg => tg.IsActive == isActive.Value);
        }

        // Apply pagination
        ApplyPaging(pageSize, pageNumber);
    }
}

