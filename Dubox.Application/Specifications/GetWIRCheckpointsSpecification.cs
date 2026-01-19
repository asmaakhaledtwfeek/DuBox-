namespace Dubox.Application.Specifications
{
    using Dubox.Application.DTOs;
    using Dubox.Application.Features.WIRCheckpoints.Queries;
    using Dubox.Domain.Entities;
    using Dubox.Domain.Specification;

    public class GetWIRCheckpointsSpecification : Specification<WIRCheckpoint>
    {
        public GetWIRCheckpointsSpecification(GetWIRCheckpointsQuery query, List<Guid>? accessibleProjectIds = null)
        {
            AddInclude(nameof(WIRCheckpoint.Box));
            AddInclude($"{nameof(WIRCheckpoint.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(WIRCheckpoint.ChecklistItems));
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            AddInclude($"{nameof(WIRCheckpoint.QualityIssues)}.{nameof(QualityIssue.AssignedToTeam)}");
            AddInclude($"{nameof(WIRCheckpoint.QualityIssues)}.{nameof(QualityIssue.AssignedToMember)}");
            AddInclude($"{nameof(WIRCheckpoint.QualityIssues)}.{nameof(QualityIssue.CCUser)}");

            EnableSplitQuery();

            // Enable pagination
            var (page, pageSize) = new PaginatedRequest
            {
                Page = query.Page,
                PageSize = query.PageSize
            }.GetNormalizedPagination();

            ApplyPaging(pageSize, page);

            // Filter out checkpoints for inactive boxes or projects
            AddCriteria(x => x.Box.IsActive);
            AddCriteria(x => x.Box.Project.IsActive);
            
            // Filter out checkpoints for projects that are on hold, closed, or archived
            AddCriteria(x => x.Box.Project.Status != Domain.Enums.ProjectStatusEnum.OnHold);
            AddCriteria(x => x.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Closed);
            AddCriteria(x => x.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Archived);

            if (accessibleProjectIds != null)
            {
                AddCriteria(x => accessibleProjectIds.Contains(x.Box.ProjectId));
            }

            if (!string.IsNullOrWhiteSpace(query.ProjectCode))
                AddCriteria(x => x.Box.Project.ProjectCode == query.ProjectCode);

            if (!string.IsNullOrWhiteSpace(query.BoxTag))
                AddCriteria(x => x.Box.BoxTag == query.BoxTag);

            if (query.Status.HasValue)
                AddCriteria(x => x.Status == query.Status.Value);

            if (!string.IsNullOrWhiteSpace(query.WIRNumber))
            {
                var wirNumberLower = query.WIRNumber.ToLower().Trim();
                AddCriteria(x => x.WIRCode != null && x.WIRCode.ToLower().Contains(wirNumberLower));
            }
            if (query.From.HasValue)
                AddCriteria(x => x.CreatedDate >= query.From.Value);

            if (query.To.HasValue)
                AddCriteria(x => x.CreatedDate <= query.To.Value);

            // Order by created date descending, then by version descending
            // This ensures latest checkpoints appear first, with newest versions on top
            AddOrderByDescending(x => x.CreatedDate);
            AddOrderByDescending(x => x.Version);
        }
    }

}
