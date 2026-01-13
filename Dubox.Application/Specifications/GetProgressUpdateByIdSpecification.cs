using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetProgressUpdateByIdSpecification : Specification<ProgressUpdate>
    {
        public GetProgressUpdateByIdSpecification(Guid progressUpdateId)
        {
            AddCriteria(pu => pu.ProgressUpdateId == progressUpdateId);
            AddInclude(nameof(ProgressUpdate.Box));
            AddInclude(nameof(ProgressUpdate.BoxActivity));
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.ActivityMaster)}");
            AddInclude(nameof(ProgressUpdate.UpdatedByUser));
            // NOTE: Don't include Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query
            
            // Enable split query to avoid Cartesian explosion
            EnableSplitQuery();
        }
    }
}

