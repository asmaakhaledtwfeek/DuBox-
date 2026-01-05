using Dubox.Application.DTOs;
using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    /// <summary>
    /// Specification for getting quality issues summary without pagination
    /// Used to calculate status counts for all issues
    /// </summary>
    public class GetQualityIssuesSummarySpecification : Specification<QualityIssue>
    {
        public GetQualityIssuesSummarySpecification(GetQualityIssuesQuery query, List<Guid>? accessibleProjectIds = null)
        {
            // Don't include heavy navigation properties for summary - just need counts
            AddInclude(nameof(QualityIssue.Box));
            AddInclude($"{nameof(QualityIssue.Box)}.{nameof(Box.Project)}");

            // DO NOT apply pagination for summary

            // Filter out quality issues for inactive boxes or projects
            AddCriteria(q => q.Box.IsActive);
            AddCriteria(q => q.Box.Project.IsActive);
            
            // Filter out quality issues for projects that are on hold, closed, or archived
            AddCriteria(q => q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.OnHold);
            AddCriteria(q => q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Closed);
            AddCriteria(q => q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Archived);

            // Apply visibility filtering based on accessible projects
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
        }
    }

}

