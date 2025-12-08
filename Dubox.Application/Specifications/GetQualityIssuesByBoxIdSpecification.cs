using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetQualityIssuesByBoxIdSpecification : Specification<QualityIssue>
    {
        public GetQualityIssuesByBoxIdSpecification(Guid boxId)
        {
            AddCriteria(q => q.BoxId == boxId);
            AddInclude(nameof(QualityIssue.Box));
            AddInclude(nameof(QualityIssue.WIRCheckpoint));
            // NOTE: Don't include Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query
            AddOrderByDescending(q => q.IssueDate);
            
            // Enable split query to avoid Cartesian explosion
            EnableSplitQuery();
        }
    }
}
