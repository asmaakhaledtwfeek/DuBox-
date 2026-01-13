using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetWIRCheckpointByIdSpecification : Specification<WIRCheckpoint>
    {
        public GetWIRCheckpointByIdSpecification(Guid checkPointId)
        {
            AddCriteria(c => c.WIRId == checkPointId);
            // Filter out checkpoints for inactive boxes or projects
            AddCriteria(c => c.Box.IsActive);
            AddCriteria(c => c.Box.Project.IsActive);
            AddInclude(nameof(WIRCheckpoint.ChecklistItems));
            AddInclude($"{nameof(WIRCheckpoint.ChecklistItems)}.{nameof(WIRChecklistItem.PredefinedChecklistItem)}");
            AddInclude($"{nameof(WIRCheckpoint.ChecklistItems)}.{nameof(WIRChecklistItem.PredefinedChecklistItem)}.{nameof(PredefinedChecklistItem.ChecklistSection)}");
            AddInclude($"{nameof(WIRCheckpoint.ChecklistItems)}.{nameof(WIRChecklistItem.PredefinedChecklistItem)}.{nameof(PredefinedChecklistItem.ChecklistSection)}.{nameof(ChecklistSection.Checklist)}");
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            AddInclude($"{nameof(WIRCheckpoint.QualityIssues)}.{nameof(QualityIssue.AssignedToTeam)}");
            AddInclude($"{nameof(WIRCheckpoint.QualityIssues)}.{nameof(QualityIssue.Images)}");
            AddInclude(nameof(WIRCheckpoint.Images));
            AddInclude(nameof(WIRCheckpoint.Box));
            AddInclude($"{nameof(WIRCheckpoint.Box)}.{nameof(Box.Project)}");

            // Enable split query to avoid Cartesian explosion with collection includes
            EnableSplitQuery();
        }
    }
}
