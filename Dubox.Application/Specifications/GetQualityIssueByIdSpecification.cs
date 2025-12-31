using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetQualityIssueByIdSpecification : Specification<QualityIssue>
    {
        public GetQualityIssueByIdSpecification(Guid issueId)
        {
            AddCriteria(i => i.IssueId == issueId);
            // Filter out quality issues for inactive boxes or projects
            AddCriteria(i => i.Box.IsActive);
            AddCriteria(i => i.Box.Project.IsActive);
            AddInclude(nameof(QualityIssue.Box));
            AddInclude($"{nameof(QualityIssue.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(QualityIssue.WIRCheckpoint));
            AddInclude(nameof(QualityIssue.AssignedToTeam));
            AddInclude(nameof(QualityIssue.AssignedToUser));
            AddInclude(nameof(QualityIssue.Images));
            // NOTE: Don't include Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query

            // Enable split query to avoid Cartesian explosion
            EnableSplitQuery();
        }
    }
}
