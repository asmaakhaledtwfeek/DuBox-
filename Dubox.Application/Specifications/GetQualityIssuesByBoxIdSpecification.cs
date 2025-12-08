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
            AddInclude(nameof(QualityIssue.Images));
            AddOrderByDescending(q => q.IssueDate);
        }
    }
}
