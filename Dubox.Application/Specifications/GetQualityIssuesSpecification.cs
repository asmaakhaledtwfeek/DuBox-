using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetQualityIssuesSpecification : Specification<QualityIssue>
    {
        public GetQualityIssuesSpecification(GetQualityIssuesQuery query)
        {
            AddInclude(nameof(QualityIssue.Box));
            AddInclude(nameof(QualityIssue.WIRCheckpoint));
            AddInclude(nameof(QualityIssue.Images));
            
            // Enable split query to avoid Cartesian explosion with Images collection
            EnableSplitQuery();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.ToLower().Trim();
                AddCriteria(q =>
                    (q.IssueDescription != null && q.IssueDescription.ToLower().Contains(term)) ||
                    (q.ReportedBy != null && q.ReportedBy.ToLower().Contains(term)) ||
                    (q.AssignedTo != null && q.AssignedTo.ToLower().Contains(term))
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
