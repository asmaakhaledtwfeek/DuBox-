using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetBoxesByFactoryIdSpecification : Specification<Box>
{
    public GetBoxesByFactoryIdSpecification(Guid factoryId)
    {
        AddCriteria(b => b.IsActive);
        AddCriteria(b => b.FactoryId == factoryId);
        // Include InProgress, Completed, and Dispatched boxes
        AddCriteria(b => b.Status == BoxStatusEnum.InProgress || 
                        b.Status == BoxStatusEnum.Completed || 
                        b.Status == BoxStatusEnum.Dispatched);
        AddInclude(nameof(Box.Project));
        // Note: BoxType and BoxSubType navigation properties are ignored
        // BoxTypeId/BoxSubTypeId now reference ProjectBoxTypes/ProjectBoxSubTypes
        AddInclude(nameof(Box.Factory));
        AddInclude(nameof(Box.CurrentLocation));

        // Enable split query to avoid Cartesian explosion with BoxActivities collection
        EnableSplitQuery();
    }
}

