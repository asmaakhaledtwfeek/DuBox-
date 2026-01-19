using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    /// <summary>
    /// Gets all versions of a checkpoint (the checkpoint itself and all its related versions)
    /// </summary>
    public class GetCheckpointVersionsSpecification : Specification<WIRCheckpoint>
    {
        public GetCheckpointVersionsSpecification(Guid checkpointId)
        {
            // Find the parent ID (either the checkpoint itself if it's v1, or its parent)
            // This will be used to fetch all versions in the chain
            
            AddInclude(nameof(WIRCheckpoint.Box));
            AddInclude($"{nameof(WIRCheckpoint.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(WIRCheckpoint.ChecklistItems));
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            AddInclude(nameof(WIRCheckpoint.Images));
            AddInclude(nameof(WIRCheckpoint.ParentCheckpoint));
            AddInclude(nameof(WIRCheckpoint.ChildVersions));

            // Filter out checkpoints for inactive boxes or projects
            AddCriteria(c => c.Box.IsActive);
            AddCriteria(c => c.Box.Project.IsActive);

            // Get all checkpoints that either:
            // 1. Have this ID as parent (child versions)
            // 2. Are this checkpoint itself
            // 3. Are the parent of this checkpoint
            AddCriteria(c => c.WIRId == checkpointId 
                          || c.ParentWIRId == checkpointId 
                          || c.ChildVersions.Any(cv => cv.WIRId == checkpointId));

            // Order by version ascending (oldest to newest)
            AddOrderBy(c => c.Version);

            EnableSplitQuery();
        }

        /// <summary>
        /// Gets all versions in a version chain by parent ID
        /// </summary>
        public GetCheckpointVersionsSpecification(Guid? parentWIRId, bool includeParent = true)
        {
            AddInclude(nameof(WIRCheckpoint.Box));
            AddInclude($"{nameof(WIRCheckpoint.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(WIRCheckpoint.ChecklistItems));
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            AddInclude(nameof(WIRCheckpoint.Images));

            // Filter out checkpoints for inactive boxes or projects
            AddCriteria(c => c.Box.IsActive);
            AddCriteria(c => c.Box.Project.IsActive);

            if (parentWIRId.HasValue)
            {
                if (includeParent)
                {
                    // Get parent and all children
                    AddCriteria(c => c.WIRId == parentWIRId.Value || c.ParentWIRId == parentWIRId.Value);
                }
                else
                {
                    // Get only children
                    AddCriteria(c => c.ParentWIRId == parentWIRId.Value);
                }
            }

            // Order by version ascending (oldest to newest)
            AddOrderBy(c => c.Version);

            EnableSplitQuery();
        }
    }
}
