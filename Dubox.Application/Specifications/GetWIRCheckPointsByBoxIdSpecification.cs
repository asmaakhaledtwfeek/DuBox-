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
            AddOrderByDescending(w => w.CreatedDate);
        }
    }
}
