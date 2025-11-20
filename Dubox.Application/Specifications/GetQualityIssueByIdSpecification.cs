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

        }
    }
}
