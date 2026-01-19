using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using System.Diagnostics;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public class AssignQualityIssueToTeamCommandHandler : IRequestHandler<AssignQualityIssueToTeamCommand, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly ITeamAssignmentService _teamAssignmentService;

        public AssignQualityIssueToTeamCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IProjectTeamVisibilityService visibilityService,
            ITeamAssignmentService teamAssignmentService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _visibilityService = visibilityService;
            _teamAssignmentService = teamAssignmentService;
        }

        public async Task<Result<QualityIssueDetailsDto>> Handle(AssignQualityIssueToTeamCommand request, CancellationToken cancellationToken)
        {
            var module= PermissionModuleEnum.QualityIssues;
            var action = PermissionActionEnum.Edit;
            var canModify = await _visibilityService.CanPerformAsync(module, action,cancellationToken);
            if (!canModify)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to assign quality issues.");

            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found.");

            var canAccessProject = await _visibilityService.CanAccessProjectAsync(issue.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to modify this quality issue.");
          
            var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(issue.Box.ProjectId, "assign quality issues", cancellationToken);
            if (!projectStatusValidation.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(projectStatusValidation.Error!);

            var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(issue.Box.BoxId, "assign quality issues", cancellationToken);
            if (!boxStatusValidation.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(boxStatusValidation.Error!);
           
            var teamValidationResult = await _teamAssignmentService.ValidateAssignmentAsync(request.TeamId, request.TeamMemberId, cancellationToken);
            if (!teamValidationResult.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(teamValidationResult.Error);

            var team = teamValidationResult.Data.Team;
            var teamMember = teamValidationResult.Data.Member;
            // Capture old values for audit log
            var oldTeamId = issue.AssignedToTeamId?.ToString() ?? "None";
            var oldTeamName = issue.AssignedToTeam?.TeamName ?? "None";
            var oldMemberId = issue.AssignedToMemberId?.ToString() ?? "None";
            var oldMemberName = issue.AssignedToMember != null 
                ? $"{issue.AssignedToMember.EmployeeName}".Trim() 
                : "None";
            var oldCCUserId = issue.CCUserId?.ToString() ?? "None";
            var oldCCUserName = issue.CCUser?.FullName ?? "None";

            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;

            // Update assignment
            issue.AssignedToTeamId = request.TeamId;
            issue.AssignedToMemberId = request.TeamMemberId;
            if (request.CCUserId.HasValue)
            {
                issue.CCUserId = request.CCUserId;
            }
            issue.UpdatedBy = currentUserId;

            _unitOfWork.Repository<QualityIssue>().Update(issue);

            // Create audit log
            var teamName = team?.TeamName ?? "None";
            var memberName = teamMember != null ? teamMember.EmployeeName ?? "Unknown" : "None";
            
            // Get CC User name if provided
            User? ccUser = null;
            if (request.CCUserId.HasValue && request.CCUserId.Value != Guid.Empty)
            {
                ccUser = await _unitOfWork.Repository<User>().GetByIdAsync(request.CCUserId.Value);
            }
            var ccUserName = ccUser?.FullName ?? "None";

            var auditLog = new AuditLog
            {
                TableName = nameof(QualityIssue),
                RecordId = issue.IssueId,
                Action = "UPDATE",
                OldValues = $"TeamId: {oldTeamId}, TeamName: {oldTeamName}, MemberId: {oldMemberId}, MemberName: {oldMemberName}, CCUserId: {oldCCUserId}, CCUserName: {oldCCUserName}",
                NewValues = $"TeamId: {request.TeamId?.ToString() ?? "None"}, TeamName: {teamName}, MemberId: {request.TeamMemberId?.ToString() ?? "None"}, MemberName: {memberName}, CCUserId: {request.CCUserId?.ToString() ?? "None"}, CCUserName: {ccUserName}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = request.TeamId.HasValue && request.TeamId.Value != Guid.Empty
                    ? $"Quality issue assigned to Team '{teamName}'" + (teamMember != null ? $" and team member '{memberName}'" : "") + (ccUser != null ? $", CC to '{ccUserName}'" : "") + $". Previous team was '{oldTeamName}'."
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
            dto.CCUserName = issue.CCUser?.FullName;
            return Result.Success(dto);
        }
    }
}

