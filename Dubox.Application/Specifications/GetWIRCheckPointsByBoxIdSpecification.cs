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
            AddInclude($"{nameof(WIRCheckpoint.QualityIssues)}.{nameof(Domain.Entities.QualityIssue.Images)}");
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            AddInclude(nameof(WIRCheckpoint.Images));
            AddOrderByDescending(w => w.CreatedDate);
            
            // Enable split query to avoid Cartesian explosion with multiple collection includes
            EnableSplitQuery();
        }
    }
}
