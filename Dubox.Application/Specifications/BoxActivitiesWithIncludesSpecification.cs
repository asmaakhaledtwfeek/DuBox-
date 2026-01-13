using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class BoxActivitiesWithIncludesSpecification : Specification<BoxActivity>
    {
        public BoxActivitiesWithIncludesSpecification(Guid boxActivityId, Guid boxId)
        {
            AddCriteria(ba => ba.BoxActivityId == boxActivityId && ba.BoxId == boxId);
            AddInclude(nameof(BoxActivity.ActivityMaster));
            AddInclude(nameof(BoxActivity.Box));
            AddInclude($"{nameof(BoxActivity.Box)}.{nameof(BoxActivity.Box.Project)}");

        }
        public BoxActivitiesWithIncludesSpecification(BoxActivity currentActivity)
        {
            AddCriteria(x =>
                         x.BoxId == currentActivity.BoxId &&
                         x.Sequence > currentActivity.Sequence &&
                         x.ActivityMaster.IsWIRCheckpoint);

            AddInclude(nameof(BoxActivity.ActivityMaster));
            AddOrderBy(x => x.Sequence);

        }
    }
}
