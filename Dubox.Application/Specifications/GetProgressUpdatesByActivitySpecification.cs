using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Specification;
using MediatR;

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
            AddOrderByDescending(pu => pu.UpdateDate);
            
            // Enable split query to avoid Cartesian explosion
            EnableSplitQuery();
        }
        public GetProgressUpdatesByActivitySpecification(Guid boxId,Guid boxActivityId , decimal ProgressPercentage,BoxStatusEnum inferredStatus)
        { 
            AddCriteria(pu => pu.BoxId == boxId &&
                                pu.BoxActivityId == boxActivityId &&
                                pu.ProgressPercentage == ProgressPercentage &&
                                pu.Status == inferredStatus);
        }
    }
}
