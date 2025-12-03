using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{
    public class AssignedTeamMembersCommandHandler : IRequestHandler<AssignedTeamMembersCommand, Result<TeamMembersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AssignedTeamMembersCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Result<TeamMembersDto>> Handle(AssignedTeamMembersCommand request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Repository<Team>()
            .GetByIdAsync(request.TeamId, cancellationToken);
            if (team == null)
                return Result.Failure<TeamMembersDto>("This Team not found.");
            if (request.UserIds == null || !request.UserIds.Any())
                return Result.Failure<TeamMembersDto>("No users provided to assign.");

            var users = await _unitOfWork.Repository<User>()
             .FindAsync(u => request.UserIds.Contains(u.UserId), cancellationToken);

            if (users == null || users.Count == 0)
                return Result.Failure<TeamMembersDto>("No valid users found.");

            var existingMembers = _unitOfWork.Repository<TeamMember>()
                    .GetWithSpec(new TeamMembersByUserIdsSpecification(request.TeamId))
                    .Data.ToList();
            var membersToRemove = existingMembers
                     .Where(tm => !request.UserIds.Contains(tm.UserId))
                     .ToList();

            if (membersToRemove.Any())
            {
                foreach (var memberToRemove in membersToRemove)
                    memberToRemove.IsActive = false;
                _unitOfWork.Repository<TeamMember>().UpdateRange(membersToRemove);

            }

            var existingUserIds = existingMembers.Select(tm => tm.UserId).ToHashSet();
            var newMembers = users
                .Where(u => !existingUserIds.Contains(u.UserId))
                .Select(u => new TeamMember
                {
                    TeamId = team.TeamId,
                    UserId = u.UserId,
                    IsActive = u.IsActive
                })
                .ToList();

            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

            // Create audit logs for removed members
            if (membersToRemove.Any())
            {
                var removeLogs = membersToRemove.Select(member => new AuditLog
                {
                    TableName = nameof(TeamMember),
                    RecordId = member.TeamMemberId,
                    Action = "BulkRemoval",
                    OldValues = $"IsActive: true, TeamId: {team.TeamId}",
                    NewValues = "IsActive: false",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"Team member removed from team '{team.TeamCode} - {team.TeamName}' during bulk assignment update."
                }).ToList();
                await _unitOfWork.Repository<AuditLog>().AddRangeAsync(removeLogs, cancellationToken);
            }

            // Create audit logs for new members
            if (newMembers.Count > 0)
            {
                await _unitOfWork.Repository<TeamMember>().AddRangeAsync(newMembers, cancellationToken);
                
                var addLogs = newMembers.Select(member => new AuditLog
                {
                    TableName = nameof(TeamMember),
                    RecordId = member.TeamMemberId,
                    Action = "BulkAssignment",
                    OldValues = "N/A (New Assignment)",
                    NewValues = $"TeamId: {team.TeamId}, TeamCode: {team.TeamCode}, TeamName: {team.TeamName}",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"Team member assigned to team '{team.TeamCode} - {team.TeamName}' during bulk assignment update."
                }).ToList();
                await _unitOfWork.Repository<AuditLog>().AddRangeAsync(addLogs, cancellationToken);
            }

            // Create team-level audit log
            var teamAuditLog = new AuditLog
            {
                TableName = nameof(Team),
                RecordId = team.TeamId,
                Action = "MembersUpdate",
                OldValues = $"MemberCount: {existingMembers.Count(m => m.IsActive)}",
                NewValues = $"MemberCount: {request.UserIds.Count}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Team '{team.TeamCode} - {team.TeamName}' members updated. {newMembers.Count} added, {membersToRemove.Count} removed."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(teamAuditLog, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            var teamMembers = _unitOfWork.Repository<TeamMember>()
                  .GetWithSpec(new TeamMembersByUserIdsSpecification(request.TeamId))
                       .Data.ToList();
            var dto = (team, teamMembers).Adapt<TeamMembersDto>();

            return Result.Success(dto);
        }
    }
}
