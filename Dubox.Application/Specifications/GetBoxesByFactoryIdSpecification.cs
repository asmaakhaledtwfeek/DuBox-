using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetBoxesByFactoryIdSpecification : Specification<Box>
{
    public GetBoxesByFactoryIdSpecification(Guid factoryId)
    {
        AddCriteria(b => b.FactoryId == factoryId);
        AddInclude(nameof(Box.Project));
        AddInclude(nameof(Box.BoxType));
        AddInclude(nameof(Box.BoxSubType));
        AddInclude(nameof(Box.Factory));
        AddInclude(nameof(Box.CurrentLocation));

        // Enable split query to avoid Cartesian explosion with BoxActivities collection
        EnableSplitQuery();
    }
}

