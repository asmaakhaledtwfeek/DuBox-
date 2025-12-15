using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetWIRsWithChecklistByBoxIdSpecification : Specification<WIRCheckpoint>
{
    public GetWIRsWithChecklistByBoxIdSpecification(Guid boxId)
    {
        AddCriteria(w => w.BoxId == boxId);
        AddInclude(nameof(WIRCheckpoint.ChecklistItems));
        AddOrderBy(w => w.WIRNumber);
        
        // Enable split query to avoid Cartesian explosion
        EnableSplitQuery();
    }
}
