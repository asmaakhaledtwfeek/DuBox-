using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands
{
    public class AssignActivityToTeamCommandHandler : IRequestHandler<AssignActivityToTeamCommand, Result<AssignBoxActivityTeamDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public AssignActivityToTeamCommandHandler(
            IUnitOfWork unitOfWork, 
            ICurrentUserService currentUserService,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _visibilityService = visibilityService;
        }

        public async Task<Result<AssignBoxActivityTeamDto>> Handle(AssignActivityToTeamCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<AssignBoxActivityTeamDto>("Access denied. Viewer role has read-only access and cannot assign activities.");
            }

            var activity = _unitOfWork.Repository<BoxActivity>().GetEntityWithSpec(new GetBoxActivityByIdSpecification(request.BoxActivityId));

            if (activity == null)
                return Result.Failure<AssignBoxActivityTeamDto>("Box Activity not found.");

            // Check if user has access to the project containing this activity
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(activity.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<AssignBoxActivityTeamDto>("Access denied. You do not have permission to modify activities in this project.");
            }

            var team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.TeamId);

            if (team == null)
                return Result.Failure<AssignBoxActivityTeamDto>("Team not found.");

            // Check if user has access to this team
            var canAccessTeam = await _visibilityService.CanAccessTeamAsync(request.TeamId, cancellationToken);
            if (!canAccessTeam)
            {
                return Result.Failure<AssignBoxActivityTeamDto>("Access denied. You do not have permission to assign this team.");
            }

            var teamMember = _unitOfWork.Repository<TeamMember>().GetEntityWithSpec(new GetTeamMemberWithIcludesSpecification(request.TeamMemberId));

            if (teamMember == null)
                return Result.Failure<AssignBoxActivityTeamDto>(" Team Member not found.");

            if (teamMember.TeamId != request.TeamId)
                return Result.Failure<AssignBoxActivityTeamDto>("The selected team member does not belong to the specified team.");

            var oldTeamId = activity.TeamId;
            var oldAssignedMemberId = activity.AssignedMemberId;

            activity.TeamId = request.TeamId;
            activity.AssignedMemberId = request.TeamMemberId;

            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            activity.ModifiedBy = currentUserId;
            activity.ModifiedDate = DateTime.UtcNow;

            _unitOfWork.Repository<BoxActivity>().Update(activity);

            var log = new AuditLog
            {
                TableName = nameof(BoxActivity),
                RecordId = activity.BoxActivityId,
                Action = "Assignment",
                OldValues = $"TeamId: {oldTeamId}, MemberId: {oldAssignedMemberId}",
                NewValues = $"TeamId: {request.TeamId}, MemberId: {request.TeamMemberId}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Activity assigned to Team '{team.TeamName}' and Member '{teamMember.EmployeeName}'. Old team ID was {oldTeamId}.",
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var responseDto = new AssignBoxActivityTeamDto
            {
                BoxActivityId = activity.BoxActivityId,
                ActivityCode = activity.ActivityMaster.ActivityCode,
                ActivityName = activity.ActivityMaster.ActivityName,
                TeamId = team.TeamId,
                TeamCode = team.TeamCode,
                TeamName = team.TeamName,
                AssigneeToId = teamMember.TeamMemberId,
                AssigneeTo = teamMember.EmployeeName == string.Empty ? teamMember.User.FullName : teamMember.EmployeeName
            };

            return Result.Success(responseDto);
        }
    }
}
