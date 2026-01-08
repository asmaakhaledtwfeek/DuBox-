using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public class AssignQualityIssueToTeamCommandHandler : IRequestHandler<AssignQualityIssueToTeamCommand, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public AssignQualityIssueToTeamCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _visibilityService = visibilityService;
        }

        public async Task<Result<QualityIssueDetailsDto>> Handle(AssignQualityIssueToTeamCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<QualityIssueDetailsDto>("Access denied. Viewer role has read-only access and cannot assign quality issues.");
            }

            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found.");

            // Verify user has access to the project this quality issue belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(issue.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to modify this quality issue.");
            var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(issue.Box.ProjectId, "assign quality issues", cancellationToken);

            if (!projectStatusValidation.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(projectStatusValidation.Error!);
            
            if (issue.Box.Status == BoxStatusEnum.Dispatched)
            {
                return Result.Failure<QualityIssueDetailsDto>("Cannot assign quality issue. The box is dispatched and no actions are allowed on quality issues. Only viewing is permitted.");
            }
            
            if (issue.Box.Status == BoxStatusEnum.OnHold)
            {
                return Result.Failure<QualityIssueDetailsDto>("Cannot assign quality issue. The box is on hold and no actions are allowed on quality issues. Only viewing is permitted.");
            }

            Team? team = null;
            TeamMember? teamMember = null;
            
            // If TeamId is provided, validate the team exists and user has access
            if (request.TeamId.HasValue && request.TeamId.Value != Guid.Empty)
            {
                team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.TeamId.Value, cancellationToken);
                if (team == null)
                    return Result.Failure<QualityIssueDetailsDto>("Team not found.");

                // Check if user has access to this team
                var canAccessTeam = await _visibilityService.CanAccessTeamAsync(request.TeamId.Value, cancellationToken);
                if (!canAccessTeam)
                {
                    return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to assign this team.");
                }
                
                // If TeamMemberId is provided, validate the member exists and belongs to the team
                if (request.TeamMemberId.HasValue && request.TeamMemberId.Value != Guid.Empty)
                {
                    teamMember = await _unitOfWork.Repository<TeamMember>().GetByIdAsync(request.TeamMemberId.Value, cancellationToken);
                    if (teamMember == null)
                        return Result.Failure<QualityIssueDetailsDto>("Team member not found.");
                    
                    if (teamMember.TeamId != team.TeamId || !teamMember.IsActive)
                        return Result.Failure<QualityIssueDetailsDto>("Selected member is not an active member of the selected team.");
                }
            }

            // Capture old values for audit log
            var oldTeamId = issue.AssignedToTeamId?.ToString() ?? "None";
            var oldTeamName = issue.AssignedToTeam?.TeamName ?? "None";
            var oldMemberId = issue.AssignedToMemberId?.ToString() ?? "None";
            var oldMemberName = issue.AssignedToMember != null 
                ? $"{issue.AssignedToMember.EmployeeName}".Trim() 
                : "None";

            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;

            // Update assignment
            issue.AssignedToTeamId = request.TeamId;
            issue.AssignedToMemberId = request.TeamMemberId;
            issue.UpdatedBy = currentUserId;

            _unitOfWork.Repository<QualityIssue>().Update(issue);

            // Create audit log
            var teamName = team?.TeamName ?? "None";
            var memberName = teamMember != null ? teamMember.EmployeeName ?? "Unknown" : "None";

            var auditLog = new AuditLog
            {
                TableName = nameof(QualityIssue),
                RecordId = issue.IssueId,
                Action = "UPDATE",
                OldValues = $"TeamId: {oldTeamId}, TeamName: {oldTeamName}, MemberId: {oldMemberId}, MemberName: {oldMemberName}",
                NewValues = $"TeamId: {request.TeamId?.ToString() ?? "None"}, TeamName: {teamName}, MemberId: {request.TeamMemberId?.ToString() ?? "None"}, MemberName: {memberName}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = request.TeamId.HasValue && request.TeamId.Value != Guid.Empty
                    ? $"Quality issue assigned to Team '{teamName}'" + (teamMember != null ? $" and team member '{memberName}'" : "") + $". Previous team was '{oldTeamName}'."
                    : $"Quality issue unassigned from Team '{oldTeamName}'."
            };

            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Reload issue with includes to return complete DTO
            issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));
            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found after update.");

            var dto = issue.Adapt<QualityIssueDetailsDto>();
            dto.AssignedToUserName = issue.AssignedToMember?.EmployeeName;
            return Result.Success(dto);
        }
    }
}

