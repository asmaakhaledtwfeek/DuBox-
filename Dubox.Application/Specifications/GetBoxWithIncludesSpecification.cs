using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxWithIncludesSpecification : Specification<Box>
    {
        public GetBoxWithIncludesSpecification(Guid boxId)
        {
            AddCriteria(box => box.BoxId == boxId);
            AddInclude(nameof(Box.Project));;
            AddInclude(nameof(Box.BoxActivities));
            AddInclude($"{nameof(Box.BoxActivities)}.{nameof(BoxActivity.ActivityMaster)}");
            AddInclude($"{nameof(Box.BoxActivities)}.{nameof(BoxActivity.ActivityTemplateActivity)}");
            AddInclude(nameof(Box.CurrentLocation));
            AddInclude(nameof(Box.BoxPanels));
            AddInclude($"{nameof(Box.BoxPanels)}.{nameof(BoxPanel.PanelType)}");
            
            AddInclude(nameof(Box.Factory));
        }

        public GetBoxWithIncludesSpecification(Guid boxId , Guid factoryId, string bay , string row , string position, Guid? factorySectionId = null)
        {
            // Only check for conflicts with boxes that are actively occupying the factory space
            // This matches the UI filtering logic in GetBoxesByFactoryIdSpecification
            AddCriteria(b => b.FactoryId == factoryId &&
                      b.IsActive &&
                      b.BoxId != boxId && 
                      b.Bay == bay &&
                      b.Row == row &&
                      b.Position == position &&
                      !string.IsNullOrWhiteSpace(b.Bay) &&
                      !string.IsNullOrWhiteSpace(b.Row) &&
                      // Only consider boxes with InProgress or Completed status (same as UI)
                      (b.Status == BoxStatusEnum.InProgress || b.Status == BoxStatusEnum.Completed) &&
                      // Exclude boxes from OnHold, Closed, or Archived projects (same as UI)
                      b.Project != null &&
                      b.Project.IsActive &&
                      b.Project.Status != ProjectStatusEnum.OnHold &&
                      b.Project.Status != ProjectStatusEnum.Closed &&
                      b.Project.Status != ProjectStatusEnum.Archived);
            
            // If factorySectionId is provided, only check for conflicts within the same section
            // This allows the same position (bay, row) in different sections
            // IMPORTANT: Orphan boxes (NULL section) will NOT block position selection
            // They need to be properly assigned to sections via database maintenance
            if (factorySectionId.HasValue)
            {
                // Strict matching: box must be assigned to the SAME section
                // Orphan boxes (NULL section) are ignored - they don't block any specific section
                AddCriteria(b => b.FactorySectionId == factorySectionId.Value);
            }
            else
            {
                // If no target section provided, only check boxes without section assignment (backwards compatibility)
                AddCriteria(b => b.FactorySectionId == null);
            }
         
        }
    }
}
