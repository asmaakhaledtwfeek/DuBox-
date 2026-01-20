using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetQualityIssuesByProjectIdSpecification : Specification<QualityIssue>
    {
        public GetQualityIssuesByProjectIdSpecification(Guid projectId)
        {
            AddCriteria(q => q.Box.ProjectId == projectId);
            // Filter out quality issues for inactive boxes or projects
            AddCriteria(q => q.Box.IsActive);
            AddCriteria(q => q.Box.Project.IsActive);
            AddInclude(nameof(QualityIssue.Box));
            AddInclude($"{nameof(QualityIssue.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(QualityIssue.WIRCheckpoint));
            AddInclude(nameof(QualityIssue.AssignedToTeam));
            AddInclude(nameof(QualityIssue.AssignedToMember));
            AddInclude($"{nameof(QualityIssue.AssignedToMember)}.{nameof(TeamMember.User)}");
            AddInclude(nameof(QualityIssue.CCUser));
            
            // NOTE: Don't include Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query
            AddOrderByDescending(q => q.IssueDate);
            
            // Enable split query to avoid Cartesian explosion
            EnableSplitQuery();
        }
    }
}


