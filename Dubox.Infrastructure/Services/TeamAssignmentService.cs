using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Infrastructure.Services
{
    public class TeamAssignmentService : ITeamAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public TeamAssignmentService(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _visibilityService = visibilityService;
        }
        public async Task<Result<AssignmentValidationResult>> ValidateAssignmentAsync(Guid? teamId,Guid? memberId,CancellationToken cancellationToken = default)
        {
            if (!teamId.HasValue || teamId.Value == Guid.Empty)
                return Result.Success(new AssignmentValidationResult(null, null));

            var team = await _unitOfWork.Repository<Team>().GetByIdAsync(teamId.Value, cancellationToken);
            if (team == null)
                return Result.Failure<AssignmentValidationResult>("Team not found.");

            var canAccess = await _visibilityService.CanAccessTeamAsync(teamId.Value, cancellationToken);
            if (!canAccess)
                return Result.Failure<AssignmentValidationResult>("Access denied. You do not have permission to assign this team.");

            TeamMember? member = null;
            if (memberId.HasValue && memberId.Value != Guid.Empty)
            {
                member = await _unitOfWork.Repository<TeamMember>().GetByIdAsync(memberId.Value, cancellationToken);
                if (member == null)
                    return Result.Failure<AssignmentValidationResult>("Team member not found.");

                if (member.TeamId != team.TeamId || !member.IsActive)
                    return Result.Failure<AssignmentValidationResult>("Selected member is not an active member of the selected team.");
            }

            return Result.Success(new AssignmentValidationResult(team, member));
        }
    

        // Create assignment audit log
        public async Task LogAssignmentChangeAsync<TEntity>(
            Guid recordId,
            string tableName,
            Guid? oldTeamId,
            string oldTeamName,
            Guid? oldMemberId,
            string oldMemberName,
            Guid? newTeamId,
            string newTeamName,
            Guid? newMemberId,
            string newMemberName,
            CancellationToken cancellationToken = default)
        {
            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var userId)
                ? userId
                : Guid.Empty;

            var description = newTeamId.HasValue && newTeamId.Value != Guid.Empty
                ? $"Assigned to Team '{newTeamName}'" + (newMemberId.HasValue ? $" and member '{newMemberName}'" : "") + $". Previous team was '{oldTeamName}'."
                : $"Unassigned from Team '{oldTeamName}'.";

            var auditLog = new AuditLog
            {
                TableName = tableName,
                RecordId = recordId,
                Action = "Assignment",
                OldValues = $"TeamId: {oldTeamId?.ToString() ?? "None"}, TeamName: {oldTeamName}, MemberId: {oldMemberId?.ToString() ?? "None"}, MemberName: {oldMemberName}",
                NewValues = $"TeamId: {newTeamId?.ToString() ?? "None"}, TeamName: {newTeamName}, MemberId: {newMemberId?.ToString() ?? "None"}, MemberName: {newMemberName}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = description
            };

            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
        }
    }
}
