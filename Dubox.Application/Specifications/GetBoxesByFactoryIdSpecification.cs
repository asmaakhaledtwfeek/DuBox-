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

        if (includeDispatched)
        {
            // Include InProgress, Completed, and Dispatched boxes
            AddCriteria(b => b.Status == BoxStatusEnum.InProgress ||
                            b.Status == BoxStatusEnum.Completed ||

                            b.Status == BoxStatusEnum.Dispatched);
            AddCriteria(b => b.Project != null &&
                           b.Project.Status != ProjectStatusEnum.OnHold &&
                           b.Project.Status != ProjectStatusEnum.Closed );
            // When including dispatched, include all boxes regardless of project status
            // because dispatched boxes can be from completed/closed projects
            AddCriteria(b => b.Project != null);
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
        // Note: BoxType and BoxSubType navigation properties are ignored
        // BoxTypeId/BoxSubTypeId now reference ProjectBoxTypes/ProjectBoxSubTypes
        AddInclude(nameof(Box.Factory));
        AddInclude(nameof(Box.CurrentLocation));

        // Enable split query to avoid Cartesian explosion with BoxActivities collection
        EnableSplitQuery();
    }
}

