using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetAllWIRRecordWithIncludesSpecification : Specification<WIRRecord>
    {
        public GetAllWIRRecordWithIncludesSpecification()
        {
            AddInclude(nameof(WIRRecord.BoxActivity));
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.ActivityMaster)}");
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.Box)}");
            AddInclude(nameof(WIRRecord.RequestedByUser));
            AddInclude(nameof(WIRRecord.InspectedByUser));
            AddOrderByDescending(w => w.RequestedDate);
        }
    }
}
