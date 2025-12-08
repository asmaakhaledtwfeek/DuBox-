using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetWIRCheckPointsByBoxIdSpecification : Specification<WIRCheckpoint>
    {
        public GetWIRCheckPointsByBoxIdSpecification(Guid boxId)
        {
            AddCriteria(w => w.BoxId == boxId);
            AddInclude(nameof(WIRCheckpoint.Box));
            AddInclude(nameof(WIRCheckpoint.ChecklistItems));
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            // NOTE: Don't include Images or QualityIssues.Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query
            AddOrderByDescending(w => w.CreatedDate);
            
            // Enable split query to avoid Cartesian explosion with multiple collection includes
            
        }
    }
}
