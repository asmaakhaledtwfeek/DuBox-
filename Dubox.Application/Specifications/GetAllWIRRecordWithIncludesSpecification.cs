using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetAllWIRRecordWithIncludesSpecification : Specification<WIRRecord>
    {
        public GetAllWIRRecordWithIncludesSpecification()
        {
            AddInclude(nameof(WIRRecord.BoxActivity));
            AddInclude($"{nameof(WIRRecord.BoxActivity)}.{nameof(BoxActivity.ActivityMaster)}");
            AddInclude($"{nameof(WIRRecord.BoxActivity)}.{nameof(BoxActivity.Box)}");
            AddInclude(nameof(WIRRecord.RequestedByUser));
            AddInclude(nameof(WIRRecord.InspectedByUser));
            AddOrderByDescending(w => w.RequestedDate);
            
            // Enable split query to avoid Cartesian explosion with multiple includes
            EnableSplitQuery();
        }
    }
}
