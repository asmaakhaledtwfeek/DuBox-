using Dubox.Application.DTOs;
using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetQualityIssuesSpecification : Specification<QualityIssue>
    {
        public GetQualityIssuesSpecification(GetQualityIssuesQuery query, List<Guid>? accessibleProjectIds = null)
        {
            AddInclude(nameof(QualityIssue.Box));
            AddInclude($"{nameof(QualityIssue.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(QualityIssue.WIRCheckpoint));
            AddInclude(nameof(QualityIssue.AssignedToTeam));
            AddInclude(nameof(QualityIssue.AssignedToUser));
            EnableSplitQuery();

            // Enable pagination
            var (page, pageSize) = new PaginatedRequest
            {
                Page = query.Page,
                PageSize = query.PageSize
            }.GetNormalizedPagination();

            ApplyPaging(pageSize, page);
            // IsTotalCountEnable = true;

            // Apply visibility filtering based on accessible projects
            // null means access to all projects (SystemAdmin/Viewer)
            if (accessibleProjectIds != null)
            {
                AddCriteria(q => accessibleProjectIds.Contains(q.Box.ProjectId));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.ToLower().Trim();
                AddCriteria(q =>
                    (q.IssueDescription != null && q.IssueDescription.ToLower().Contains(term)) ||
                    (q.ReportedBy != null && q.ReportedBy.ToLower().Contains(term)) 
                );
            }

            if (query.Status.HasValue)
                AddCriteria(q => q.Status == query.Status.Value);

            if (query.Severity.HasValue)
                AddCriteria(q => q.Severity == query.Severity.Value);

            if (query.IssueType.HasValue)
                AddCriteria(q => q.IssueType == query.IssueType.Value);
            AddOrderByDescending(q => q.IssueDate);
        }
    }

}
