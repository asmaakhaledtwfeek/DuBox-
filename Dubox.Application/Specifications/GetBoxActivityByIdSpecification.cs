using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxActivityByIdSpecification : Specification<BoxActivity>
    {
        public GetBoxActivityByIdSpecification(Guid boxActivityId)
        {
            AddCriteria(ba => ba.BoxActivityId == boxActivityId);
            AddInclude(nameof(BoxActivity.ActivityMaster));
            AddInclude(nameof(BoxActivity.Box));
            AddInclude($"{nameof(BoxActivity.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(BoxActivity.Team));
            AddInclude(nameof(BoxActivity.AssignedMember));
            AddInclude($"{nameof(BoxActivity.AssignedMember)}.{nameof(TeamMember.User)}");
        }
    }
}
