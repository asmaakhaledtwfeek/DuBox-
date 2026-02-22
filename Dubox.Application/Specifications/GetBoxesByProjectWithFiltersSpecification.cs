using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

/// <summary>
/// Specification to get boxes by project with optional server-side filters.
/// Reduces payload by filtering in the database instead of loading all boxes.
/// </summary>
public class GetBoxesByProjectWithFiltersSpecification : Specification<Box>
{
    public GetBoxesByProjectWithFiltersSpecification(
        Guid projectId,
        IReadOnlyList<int>? statuses = null,
        string? boxType = null,
        string? boxSubType = null,
        string? buildingNumber = null,
        string? floor = null,
        string? zone = null,
        string? search = null)
    {
        AddCriteria(b => b.ProjectId == projectId);
        AddCriteria(b => b.IsActive);

        if (statuses != null && statuses.Count > 0)
            AddCriteria(b => statuses.Contains((int)b.Status));

        // Filter by box type abbreviation from BoxTag (format: ProjectNumber-Building-Floor-Type-SubType)
        if (!string.IsNullOrWhiteSpace(boxType))
        {
            // Use string pattern matching that EF Core can translate to SQL
            // BoxTag format: XXX-XX-XX-TYPE-SUBTYPE
            // Match pattern: anything-anything-anything-TYPE-*
            var typePattern = $"-{boxType}-";
            AddCriteria(b => b.BoxTag != null && b.BoxTag.Contains(typePattern));
        }

        // Filter by box subtype abbreviation from BoxTag
        if (!string.IsNullOrWhiteSpace(boxSubType))
        {
            // Use string pattern matching that EF Core can translate to SQL
            // BoxTag format: XXX-XX-XX-TYPE-SUBTYPE
            // Match pattern: anything ending with -SUBTYPE
            var subTypePattern = $"-{boxSubType}";
            AddCriteria(b => b.BoxTag != null && 
                (b.BoxTag.EndsWith(subTypePattern) || b.BoxTag.Contains($"{subTypePattern}-")));
        }

        if (!string.IsNullOrWhiteSpace(buildingNumber))
            AddCriteria(b => b.BuildingNumber != null && b.BuildingNumber == buildingNumber);

        if (!string.IsNullOrWhiteSpace(floor))
            AddCriteria(b => b.Floor == floor);

        if (!string.IsNullOrWhiteSpace(zone))
            AddCriteria(b => b.Zone != null && b.Zone == zone);

        if (!string.IsNullOrWhiteSpace(search))
            AddCriteria(b => b.BoxTag != null && b.BoxTag.Contains(search));

        AddInclude(nameof(Box.Project));
        AddInclude(nameof(Box.Factory));
        AddInclude(nameof(Box.CurrentLocation));
        AddInclude(nameof(Box.BoxDrawings));
        AddInclude(nameof(Box.BoxPanels));
        // Include PanelType to avoid lazy loading N+1 queries
        AddInclude($"{nameof(Box.BoxPanels)}.{nameof(BoxPanel.PanelType)}");
        EnableSplitQuery();
    }
    public GetBoxesByProjectWithFiltersSpecification(Guid projectId , Box? currentBox, string? buildingNumber , string? floor )
    {
        AddInclude(nameof(Box.Project));
        AddCriteria(b => b.ProjectId == projectId && b.ProjectBoxTypeId==currentBox.ProjectBoxTypeId && b.Status!= Domain.Enums.BoxStatusEnum.Dispatched) ;
        AddCriteria(b => b.IsActive && b.BoxId != currentBox.BoxId);
     
        AddCriteria(b => b.Floor == floor);
       
        AddCriteria(b => b.BuildingNumber == buildingNumber);
             

    }
}
