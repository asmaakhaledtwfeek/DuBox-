using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetBoxActivitiesByBoxSpecification : Specification<BoxActivity>
{
    public GetBoxActivitiesByBoxSpecification(Guid boxId)
    {
        // Filter by BoxId and active status
        AddCriteria(ba => ba.BoxId == boxId);
        
        // Include only necessary related entities
        // Order matters: include most frequently used first
        AddInclude(nameof(BoxActivity.ActivityMaster));
        AddInclude(nameof(BoxActivity.ActivityTemplateActivity));

        AddInclude(nameof(BoxActivity.Team));
        AddInclude(nameof(BoxActivity.AssignedMember));
        AddInclude($"{nameof(BoxActivity.AssignedMember)}.{nameof(TeamMember.User)}");
        
        // Only include Box.BoxTag (minimal data needed)
        AddInclude(nameof(BoxActivity.Box));
        
        // Order by sequence for consistent results
        AddOrderBy(ba => ba.Sequence);
        
        // OPTIMIZED: Disable split query for better performance with proper indexes
        // Single query with JOIN is faster when:
        // 1. Proper indexes exist (BoxId, TeamId, AssignedMemberId)
        // 2. Result set is small (activities per box is typically < 50)
        // 3. Network latency is a factor (Azure SQL)
        // Split query creates N+1 problem with multiple round trips to database
    }
}

