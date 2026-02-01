using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetPredefineChecklistItemsWithIncludesSpecification : Specification<PredefinedChecklistItem>
    {
        public GetPredefineChecklistItemsWithIncludesSpecification(List<Guid> sectionIds)
        {
            AddCriteria(p => p.ChecklistSectionId.HasValue && sectionIds.Contains(p.ChecklistSectionId.Value));
            AddInclude(nameof(PredefinedChecklistItem.ChecklistSection));
            AddInclude($"{nameof(PredefinedChecklistItem.ChecklistSection)}.{nameof(ChecklistSection.Checklist)}");
        }
    }
}
