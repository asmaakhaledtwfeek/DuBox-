using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetQualityIssueByIdSpecification : Specification<QualityIssue>
    {
        public GetQualityIssueByIdSpecification(Guid issueId)
        {
            AddCriteria(i => i.IssueId == issueId);
            AddInclude(nameof(QualityIssue.Box));
            AddInclude(nameof(QualityIssue.WIRCheckpoint));
            AddInclude(nameof(QualityIssue.Images));
            // NOTE: Don't include Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query

            // Enable split query to avoid Cartesian explosion
            EnableSplitQuery();
        }
    }
}
