using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetProgressUpdatesByBoxSpecification : Specification<ProgressUpdate>
    {
        public GetProgressUpdatesByBoxSpecification(Guid boxId)
        {
            AddCriteria(pu => pu.BoxId == boxId);
            AddInclude(nameof(ProgressUpdate.Box));
            AddInclude(nameof(ProgressUpdate.BoxActivity));
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.ActivityMaster)}");
            AddInclude(nameof(ProgressUpdate.UpdatedByUser));
            AddOrderByDescending(pu => pu.UpdateDate);

        }
    }
}
