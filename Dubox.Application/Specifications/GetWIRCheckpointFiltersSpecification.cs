using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetWIRCheckpointFiltersSpecification : Specification<WIRCheckpoint>
    {
        public GetWIRCheckpointFiltersSpecification(List<Guid>? accessibleProjectIds = null)
        {
            // Include related entities needed for filters
            AddInclude(nameof(WIRCheckpoint.Box));
            AddInclude($"{nameof(WIRCheckpoint.Box)}.{nameof(Box.Project)}");

            // Filter out checkpoints for inactive boxes or projects
            AddCriteria(x => x.Box.IsActive);
            AddCriteria(x => x.Box.Project.IsActive);
            
            // Filter out checkpoints for projects that are on hold, closed, or archived
            AddCriteria(x => x.Box.Project.Status != Domain.Enums.ProjectStatusEnum.OnHold);
            AddCriteria(x => x.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Closed);
            AddCriteria(x => x.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Archived);
            
            // Apply visibility filter
            if (accessibleProjectIds != null && accessibleProjectIds.Any())
            {
                AddCriteria(x => accessibleProjectIds.Contains(x.Box.ProjectId));
            }

            // No pagination - we want all records for filters
            // No ordering needed - will be ordered in memory after grouping
        }
    }
}
