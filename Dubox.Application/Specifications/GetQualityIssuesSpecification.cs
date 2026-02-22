using Dubox.Application.DTOs;
using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetQualityIssuesSpecification : Specification<QualityIssue>
    {
        public GetQualityIssuesSpecification(List<Guid>? accessibleProjectIds = null)
        {
            if (accessibleProjectIds != null && accessibleProjectIds.Any())
            {
                AddCriteria(qi => 
                    (qi.Box != null && qi.Box.ProjectId != null && accessibleProjectIds.Contains(qi.Box.ProjectId)) ||
                    (qi.ProjectId != null && accessibleProjectIds.Contains(qi.ProjectId.Value)));
            }
            
            AddInclude(nameof(QualityIssue.Box));
            AddInclude(nameof(QualityIssue.Project));
            AddInclude($"{nameof(QualityIssue.Box)}.{nameof(Box.Project)}");
        }
        public GetQualityIssuesSpecification(GetQualityIssuesQuery query, List<Guid>? accessibleProjectIds = null, Guid? currentUserId = null)
        {
            AddInclude(nameof(QualityIssue.Box));
            AddInclude(nameof(QualityIssue.Project));
            AddInclude($"{nameof(QualityIssue.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(QualityIssue.WIRCheckpoint));
            AddInclude(nameof(QualityIssue.AssignedToTeam));
            AddInclude(nameof(QualityIssue.AssignedToMember));
            AddInclude($"{nameof(QualityIssue.AssignedToMember)}.{nameof(TeamMember.User)}");
            AddInclude(nameof(QualityIssue.AssignedUser));

            AddInclude(nameof(QualityIssue.CCUser));
            EnableSplitQuery();

            // Enable pagination
            var (page, pageSize) = new PaginatedRequest
            {
                Page = query.Page,
                PageSize = query.PageSize
            }.GetNormalizedPagination();

            ApplyPaging(pageSize, page);
            // IsTotalCountEnable = true;

            // Filter out quality issues for inactive boxes or projects
            AddCriteria(q => q.Box == null || q.Box.IsActive);
            AddCriteria(q => (q.Box == null || q.Box.Project == null || q.Box.Project.IsActive) && 
                            (q.Project == null || q.Project.IsActive));
            
            // Filter out quality issues for projects that are on hold, closed, or archived
            AddCriteria(q => 
                (q.Box == null || q.Box.Project == null || q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.OnHold) &&
                (q.Project == null || q.Project.Status != Domain.Enums.ProjectStatusEnum.OnHold));
            AddCriteria(q => 
                (q.Box == null || q.Box.Project == null || q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Closed) &&
                (q.Project == null || q.Project.Status != Domain.Enums.ProjectStatusEnum.Closed));
            AddCriteria(q => 
                (q.Box == null || q.Box.Project == null || q.Box.Project.Status != Domain.Enums.ProjectStatusEnum.Archived) &&
                (q.Project == null || q.Project.Status != Domain.Enums.ProjectStatusEnum.Archived));

            // Apply visibility filtering based on accessible projects OR assigned to current user
            // null means access to all projects (SystemAdmin/Viewer)
            if (accessibleProjectIds != null && currentUserId.HasValue)
            {
                // User can see quality issues that are either:
                // 1. In accessible projects (via Box or direct Project), OR
                // 2. Assigned to them (via TeamMember)
                AddCriteria(q => 
                    (q.Box != null && accessibleProjectIds.Contains(q.Box.ProjectId)) ||
                    (q.ProjectId.HasValue && accessibleProjectIds.Contains(q.ProjectId.Value)) ||
                    (q.AssignedToMember != null && q.AssignedToMember.UserId == currentUserId.Value)
                );
            }
            else if (accessibleProjectIds != null)
            {
                // Fallback if no currentUserId (existing behavior)
                AddCriteria(q => 
                    (q.Box != null && accessibleProjectIds.Contains(q.Box.ProjectId)) ||
                    (q.ProjectId.HasValue && accessibleProjectIds.Contains(q.ProjectId.Value)));
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

            if (!string.IsNullOrWhiteSpace(query.IssueNumber))
                AddCriteria(q => q.IssueNumber.Contains(query.IssueNumber.Trim()));

            if (!string.IsNullOrWhiteSpace(query.BoxTag))
                AddCriteria(q => q.Box != null && q.Box.BoxTag == query.BoxTag.Trim());

            if (!string.IsNullOrWhiteSpace(query.ProjectCode))
                AddCriteria(q =>
                    (q.Box != null && q.Box.Project != null && q.Box.Project.ProjectCode == query.ProjectCode.Trim()) ||
                    (q.Project != null && q.Project.ProjectCode == query.ProjectCode.Trim()));

            if (!string.IsNullOrWhiteSpace(query.AssignedUser) && Guid.TryParse(query.AssignedUser, out var assignedUserId))
                AddCriteria(q => (q.AssignedToMember != null && q.AssignedToMember.UserId == assignedUserId) ||
                                 q.AssignedToMemberId == assignedUserId ||
                                 q.AssignedUserId == assignedUserId);

            AddOrderByDescending(q => q.IssueDate);
        }
        public GetQualityIssuesSpecification()
        {
            AddInclude(nameof(QualityIssue.Box));
            AddInclude(nameof(QualityIssue.Project));
            AddInclude($"{nameof(QualityIssue.Box)}.{nameof(Box.Project)}");
            AddInclude(nameof(QualityIssue.WIRCheckpoint));
            AddInclude(nameof(QualityIssue.AssignedToTeam));
            AddInclude(nameof(QualityIssue.AssignedToMember));
            AddInclude(nameof(QualityIssue.CCUser));
            EnableSplitQuery();
        }
        }

}
