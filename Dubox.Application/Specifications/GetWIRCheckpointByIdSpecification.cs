using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetWIRCheckpointByIdSpecification : Specification<WIRCheckpoint>
    {
        public GetWIRCheckpointByIdSpecification(Guid checkPointId)
        {
            AddCriteria(c => c.WIRId == checkPointId);
            AddInclude(nameof(WIRCheckpoint.ChecklistItems));
            AddInclude($"{nameof(WIRCheckpoint.QualityIssues)}.{nameof(Domain.Entities.QualityIssue.Images)}");
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            AddInclude(nameof(WIRCheckpoint.Images));
            AddInclude(nameof(WIRCheckpoint.Box));
            
            // Enable split query to avoid Cartesian explosion with collection includes
            EnableSplitQuery();
        }
    }
}
