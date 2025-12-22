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

            EnableSplitQuery();

            // Enable pagination
            var (page, pageSize) = new PaginatedRequest
            {
                Page = query.Page,
                PageSize = query.PageSize
            }.GetNormalizedPagination();

            ApplyPaging(pageSize, page);

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

            AddOrderByDescending(x => x.CreatedDate);
        }
    }

}
