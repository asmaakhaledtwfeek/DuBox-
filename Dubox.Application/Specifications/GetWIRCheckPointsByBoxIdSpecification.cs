using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetWIRCheckPointsByBoxIdSpecification : Specification<WIRCheckpoint>
    {
        public GetWIRCheckPointsByBoxIdSpecification(Guid boxId)
        {
            AddCriteria(w => w.BoxId == boxId);
            // Filter out checkpoints for inactive boxes or projects
            AddCriteria(w => w.Box.IsActive);
            AddCriteria(w => w.Box.Project.IsActive);
            AddInclude(nameof(WIRCheckpoint.Box));
            AddInclude($"{nameof(WIRCheckpoint.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(WIRCheckpoint.ChecklistItems));
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            AddInclude($"{nameof(WIRCheckpoint.QualityIssues)}.{nameof(QualityIssue.AssignedToTeam)}");
            AddInclude($"{nameof(WIRCheckpoint.ChecklistItems)}.{nameof(WIRChecklistItem.PredefinedChecklistItem)}");
            AddInclude($"{nameof(WIRCheckpoint.ChecklistItems)}.{nameof(WIRChecklistItem.PredefinedChecklistItem)}.{nameof(PredefinedChecklistItem.ChecklistSection)}");
            AddInclude($"{nameof(WIRCheckpoint.ChecklistItems)}.{nameof(WIRChecklistItem.PredefinedChecklistItem)}.{nameof(PredefinedChecklistItem.ChecklistSection)}.{nameof(ChecklistSection.Checklist)}");

            AddOrderByDescending(w => w.CreatedDate);


            EnableSplitQuery();
        }
    }
}
