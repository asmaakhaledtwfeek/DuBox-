using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetBoxActivitiesByBoxSpecification : Specification<BoxActivity>
{
    public GetBoxActivitiesByBoxSpecification(Guid boxId)
    {
        AddCriteria(ba => ba.BoxId == boxId);
        AddInclude(nameof(BoxActivity.ActivityMaster));
        AddInclude(nameof(BoxActivity.Box));
        AddInclude(nameof(BoxActivity.Team));
        AddInclude(nameof(BoxActivity.AssignedMember));
        AddInclude($"{nameof(BoxActivity.AssignedMember)}.{nameof(TeamMember.User)}");
        AddOrderBy(ba => ba.Sequence);
    }
}

