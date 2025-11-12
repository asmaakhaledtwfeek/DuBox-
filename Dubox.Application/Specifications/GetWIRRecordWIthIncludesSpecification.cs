using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetWIRRecordWIthIncludesSpecification : Specification<WIRRecord>
    {
        public GetWIRRecordWIthIncludesSpecification(Guid wIRRecordId)
        {
            AddCriteria(wr => wr.WIRRecordId == wIRRecordId);
            AddInclude(nameof(WIRRecord.BoxActivity));
            AddInclude(nameof(WIRRecord.RequestedByUser));
            AddInclude($"{nameof(WIRRecord.BoxActivity)}.{nameof(WIRRecord.BoxActivity.Box)}");
            AddInclude($"{nameof(WIRRecord.BoxActivity)}.{nameof(WIRRecord.BoxActivity.ActivityMaster)}");


        }
    }
}
