using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetNotComplatedActivitiesByAssignedMemberIdSpecification : Specification<BoxActivity>
    {
        public GetNotComplatedActivitiesByAssignedMemberIdSpecification(Guid assignedMemberId)
        {
            AddCriteria(ac => ac.AssignedMemberId == assignedMemberId && ac.ProgressPercentage < 100 && ac.Status != Domain.Enums.BoxStatusEnum.Completed);
        }
    }
}
