using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetBoxesByFactoryIdSpecification : Specification<Box>
{
    public GetBoxesByFactoryIdSpecification(Guid factoryId, bool includeDispatched = false)
    {
        AddCriteria(b => b.IsActive);
        AddCriteria(b => b.FactoryId == factoryId);
        AddCriteria(b => b.Project.IsActive);
        AddCriteria(b => !(b.Project.Status == ProjectStatusEnum.Archived && b.Status != BoxStatusEnum.Dispatched));

        if (includeDispatched)
        {
            // Include InProgress, Completed, and Dispatched boxes
            AddCriteria(b => b.Status == BoxStatusEnum.InProgress ||
                            b.Status == BoxStatusEnum.Completed ||
                            b.Status == BoxStatusEnum.Dispatched);

            AddCriteria(b => b.Project != null &&
                           
                           b.Project.Status != ProjectStatusEnum.OnHold &&
                           b.Project.Status != ProjectStatusEnum.Closed);
        }
        else
        {
            // Only include InProgress or Completed boxes (exclude Dispatched, NotStarted, etc.)
            AddCriteria(b => b.Status == BoxStatusEnum.InProgress ||
                            b.Status == BoxStatusEnum.Completed);
            // Exclude boxes from OnHold, Closed, or Archived projects for active boxes only
            AddCriteria(b => b.Project != null &&
                            b.Project.Status != ProjectStatusEnum.OnHold &&
                            b.Project.Status != ProjectStatusEnum.Closed &&
                            b.Project.Status != ProjectStatusEnum.Archived);
        }

        AddInclude(nameof(Box.Project));
        AddInclude(nameof(Box.Factory));
        AddInclude(nameof(Box.CurrentLocation));
        AddInclude(nameof(Box.BoxPanels));

        // Add ordering required for pagination with split query
        AddOrderByDescending(x => x.CreatedDate);

        // Enable split query to avoid Cartesian explosion with BoxActivities collection
        EnableSplitQuery();
    }
}

