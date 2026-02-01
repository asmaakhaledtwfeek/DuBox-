using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{
    public class RemoveTeamMemberCommandHandler : IRequestHandler<RemoveTeamMemberCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveTeamMemberCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Repository<Team>()
                .GetByIdAsync(request.TeamId, cancellationToken);

            if (team == null)
                return Result.Failure<bool>("Team not found.");

            if (!team.IsActive)
                return Result.Failure<bool>("Cannot remove members from an inactive team.");

            var teamMember = await _unitOfWork.Repository<TeamMember>()
                .GetByIdAsync(request.TeamMemberId, cancellationToken);

            if (teamMember == null)
                return Result.Failure<bool>("Team member not found.");

            if (teamMember.TeamId != request.TeamId)
                return Result.Failure<bool>("Team member does not belong to this team.");

            var wasTeamLeader = team.TeamLeaderMemberId == request.TeamMemberId;
            if (wasTeamLeader)
                team.TeamLeaderMemberId = null;

            var assignedNotComplatedActivities = _unitOfWork.Repository<BoxActivity>()
                .GetWithSpec(new GetNotComplatedActivitiesByAssignedMemberIdSpecification(request.TeamMemberId)).Data.ToList();
            if (assignedNotComplatedActivities.Any())
            {
                foreach (var activity in assignedNotComplatedActivities)
                    activity.AssignedMemberId = null;

                _unitOfWork.Repository<BoxActivity>().UpdateRange(assignedNotComplatedActivities);
            }
            teamMember.IsActive = false;
            _unitOfWork.Repository<Team>().Update(team);

            _unitOfWork.Repository<TeamMember>().Update(teamMember);

            // Create audit log
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            var auditLog = new AuditLog
            {
                TableName = nameof(TeamMember),
                RecordId = teamMember.TeamMemberId,
                Action = "Removal",
                OldValues = $"IsActive: true, TeamId: {team.TeamId}, TeamCode: {team.TeamCode}, TeamName: {team.TeamName}",
                NewValues = $"IsActive: false{(wasTeamLeader ? ", TeamLeader removed" : "")}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Team member removed from team '{team.TeamCode} - {team.TeamName}'.{(wasTeamLeader ? " Team leader role was also removed." : "")}{(assignedNotComplatedActivities.Any() ? $" {assignedNotComplatedActivities.Count} assigned activities were unassigned." : "")}"
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(true);
        }
    }
}


