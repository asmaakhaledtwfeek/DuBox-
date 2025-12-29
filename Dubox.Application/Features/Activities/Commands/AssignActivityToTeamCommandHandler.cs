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

            // Check if project is archived
            var isArchived = await _visibilityService.IsProjectArchivedAsync(activity.Box.ProjectId, cancellationToken);
            if (isArchived)
            {
                return Result.Failure<AssignBoxActivityTeamDto>("Cannot assign activities in an archived project. Archived projects are read-only.");
            }

            // Check if project is on hold
            var isOnHold = await _visibilityService.IsProjectOnHoldAsync(activity.Box.ProjectId, cancellationToken);
            if (isOnHold)
            {
                return Result.Failure<AssignBoxActivityTeamDto>("Cannot assign activities in a project on hold. Projects on hold only allow project status changes.");
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

            TeamGroup? teamGroup = null;
            string groupTag = "";
            
            if (request.TeamGroupId.HasValue && request.TeamGroupId.Value != Guid.Empty)
            {
                teamGroup = await _unitOfWork.Repository<TeamGroup>().GetByIdAsync(request.TeamGroupId.Value);

                if (teamGroup == null)
                    return Result.Failure<AssignBoxActivityTeamDto>("Team Group not found.");

                if (teamGroup.TeamId != request.TeamId)
                    return Result.Failure<AssignBoxActivityTeamDto>("The selected team group does not belong to the specified team.");

                if (!teamGroup.IsActive)
                    return Result.Failure<AssignBoxActivityTeamDto>("The selected team group is not active.");

                groupTag = teamGroup.GroupTag;
            }

            var oldTeamId = activity.TeamId;
            var oldAssignedGroupId = activity.AssignedGroupId;

            activity.TeamId = request.TeamId;
            activity.AssignedGroupId = request.TeamGroupId;
            
            // Set assigned member - use member from request if provided, otherwise use group leader if available
            if (request.AssignedMemberId.HasValue && request.AssignedMemberId.Value != Guid.Empty)
            {
                activity.AssignedMemberId = request.AssignedMemberId;
            }
            else if (teamGroup != null && teamGroup.GroupLeaderId.HasValue)
            {
                activity.AssignedMemberId = teamGroup.GroupLeaderId;
            }
            else
            {
                activity.AssignedMemberId = null;
            }
            
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            activity.ModifiedBy = currentUserId;
            activity.ModifiedDate = DateTime.UtcNow;

            _unitOfWork.Repository<BoxActivity>().Update(activity);

            var log = new AuditLog
            {
                TableName = nameof(BoxActivity),
                RecordId = activity.BoxActivityId,
                Action = "Assignment",
                OldValues = $"TeamId: {oldTeamId}, GroupId: {oldAssignedGroupId}",
                NewValues = $"TeamId: {request.TeamId}, GroupId: {request.TeamGroupId}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Activity assigned to Team '{team.TeamName}'" + (teamGroup != null ? $" and Group '{groupTag}'" : "") + $". Old team ID was {oldTeamId}.",
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
                AssignedGroupId = teamGroup?.TeamGroupId,
                AssignedGroupTag = groupTag
            };

            return Result.Success(responseDto);
        }
    }
}
