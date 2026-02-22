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
            AddInclude(nameof(BoxActivity.ActivityTemplateActivity));
            AddInclude(nameof(BoxActivity.Box));
            AddInclude($"{nameof(BoxActivity.Box)}.{nameof(BoxActivity.Box.Project)}");

        }
        public BoxActivitiesWithIncludesSpecification(BoxActivity currentActivity)
        {
            // Only filter by BoxId and Sequence, WIR checkpoint filtering will be done in-memory
            // because we need to check both ActivityMaster and ActivityTemplateActivity
            AddCriteria(x =>
                         x.BoxId == currentActivity.BoxId &&
                         x.Sequence > currentActivity.Sequence &&
                         (x.ActivityMaster != null && x.ActivityMaster.IsWIRCheckpoint ||
                          x.ActivityTemplateActivity != null && x.ActivityTemplateActivity.IsWIRCheckpoint));

            AddInclude(nameof(BoxActivity.ActivityMaster));
            AddInclude(nameof(BoxActivity.ActivityTemplateActivity));
            AddOrderBy(x => x.Sequence);

        }
    }
}
