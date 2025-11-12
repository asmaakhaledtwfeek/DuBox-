using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxActivityByIdSpecification : Specification<BoxActivity>
    {
        public GetBoxActivityByIdSpecification(Guid boxActivityId)
        {
            AddCriteria(wr => wr.BoxActivityId == boxActivityId);
            AddInclude(nameof(BoxActivity.ActivityMaster));
            AddInclude(nameof(BoxActivity.Box));
        }
    }
}
