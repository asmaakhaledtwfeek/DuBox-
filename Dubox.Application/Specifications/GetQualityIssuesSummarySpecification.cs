using Dubox.Application.DTOs;
using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    
    public class GetQualityIssuesSummarySpecification : Specification<QualityIssue>
    {
        public GetQualityIssuesSummarySpecification(GetQualityIssuesQuery query, List<Guid>? accessibleProjectIds = null)
        {
            AddInclude(nameof(QualityIssue.Box));
            AddInclude($"{nameof(QualityIssue.Box)}.{nameof(Box.Project)}");

            AddCriteria(q => q.Box.IsActive);
            AddCriteria(q => q.Box.Project.IsActive);
            
            AddCriteria(q => q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.OnHold);
            AddCriteria(q => q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Closed);
            AddCriteria(q => q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Archived);

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

