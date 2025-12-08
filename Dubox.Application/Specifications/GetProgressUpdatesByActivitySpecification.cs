using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetProgressUpdatesByActivitySpecification : Specification<ProgressUpdate>
    {
        public GetProgressUpdatesByActivitySpecification(Guid boxActivityId)
        {
            AddCriteria(pu => pu.BoxActivityId == boxActivityId);
            AddInclude(nameof(ProgressUpdate.Box));
            AddInclude(nameof(ProgressUpdate.BoxActivity));
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.ActivityMaster)}");
            AddInclude(nameof(ProgressUpdate.UpdatedByUser));
            // NOTE: Don't include Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query
            AddOrderByDescending(pu => pu.UpdateDate);
            
            // Enable split query to avoid Cartesian explosion
            
        }
    }
}
