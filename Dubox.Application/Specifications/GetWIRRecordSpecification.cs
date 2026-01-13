using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetWIRRecordSpecification : Specification<WIRRecord>
    {
        public GetWIRRecordSpecification(Guid boxId)
        {

            AddInclude(nameof(WIRRecord.BoxActivity));
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.ActivityMaster)}");
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.Box)}");
            AddInclude(nameof(WIRRecord.RequestedByUser));
            AddInclude(nameof(WIRRecord.InspectedByUser));
            AddOrderByDescending(w => w.RequestedDate);
        }

        public GetWIRRecordSpecification(Guid boxId ,bool position)
        {
            AddCriteria(w=> w.BoxActivity.BoxId == boxId );
            AddCriteria(w => !string.IsNullOrWhiteSpace(w.Bay) || !string.IsNullOrWhiteSpace(w.Row) || !string.IsNullOrWhiteSpace(w.Position));
            AddInclude(nameof(WIRRecord.BoxActivity));
            AddOrderByDescending(w => w.CreatedDate);


        }
    }
}
