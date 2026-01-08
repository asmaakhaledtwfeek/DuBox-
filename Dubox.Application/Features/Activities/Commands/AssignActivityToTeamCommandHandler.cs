using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
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
            var module= PermissionModuleEnum.Activities;
            var action= PermissionActionEnum.Edit;
            var canModify = await _visibilityService.CanPerformAsync( module , action , cancellationToken);
            if (!canModify)
                return Result.Failure<AssignBoxActivityTeamDto>("Access denied. You do not have permission to assign activities.");

            var activity = _unitOfWork.Repository<BoxActivity>().GetEntityWithSpec(new GetBoxActivityByIdSpecification(request.BoxActivityId));

            if (activity == null)
                return Result.Failure<AssignBoxActivityTeamDto>("Box Activity not found.");

            var canAccessProject = await _visibilityService.CanAccessProjectAsync(activity.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<AssignBoxActivityTeamDto>("Access denied. You do not have permission to modify activities in this project.");

            var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync( activity.Box.ProjectId, "assign activities to crew", cancellationToken);

           if(!projectStatusValidation.IsSuccess)
                return Result.Failure<AssignBoxActivityTeamDto>(projectStatusValidation.Error!);

            var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(activity.Box.BoxId, "assign activities to crew", cancellationToken);

            if (!boxStatusValidation.IsSuccess)
                return Result.Failure<AssignBoxActivityTeamDto>(boxStatusValidation.Error!);

            var activityStatusValidation = await _visibilityService.GetActivityStatusChecksAsync(activity.BoxActivityId, "assign activities to crew", cancellationToken);

            if (!activityStatusValidation.IsSuccess)
                return Result.Failure<AssignBoxActivityTeamDto>(activityStatusValidation.Error!);
           
            var team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.TeamId);

            if (team == null)
                return Result.Failure<AssignBoxActivityTeamDto>("Team not found.");

            TeamMember? teamMember=null;
            if(request.AssignedMemberId.HasValue)
                teamMember=await _unitOfWork.Repository<TeamMember>().GetByIdAsync(request.AssignedMemberId);
            if (teamMember == null)
                return Result.Failure<AssignBoxActivityTeamDto>("Team Member not found.");
            if (teamMember !=null && (teamMember.TeamId!=team.TeamId || !teamMember.IsActive) )
                return Result.Failure<AssignBoxActivityTeamDto>("Selected member is not a member in selected team.");
            // Check if user has access to this team
            var canAccessTeam = await _visibilityService.CanAccessTeamAsync(request.TeamId, cancellationToken);
            if (!canAccessTeam)
                return Result.Failure<AssignBoxActivityTeamDto>("Access denied. You do not have permission to assign this team.");
            
            var oldTeamId = activity.TeamId;
            var oldMemberId = activity.AssignedMemberId;

            activity.TeamId = request.TeamId;
            activity.AssignedMemberId = request.AssignedMemberId;
            
            // Set assigned member - use member from request if provided, otherwise use group leader if available
            if (request.AssignedMemberId.HasValue && request.AssignedMemberId.Value != Guid.Empty)
                activity.AssignedMemberId = request.AssignedMemberId;
           
            else
                activity.AssignedMemberId = null;
            
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            activity.ModifiedBy = currentUserId;
            activity.ModifiedDate = DateTime.UtcNow;

            _unitOfWork.Repository<BoxActivity>().Update(activity);

            var log = new AuditLog
            {
                TableName = nameof(BoxActivity),
                RecordId = activity.BoxActivityId,
                Action = "Assignment",
                OldValues = $"TeamId: {oldTeamId}, MemberId: {oldMemberId}",
                NewValues = $"TeamId: {request.TeamId}, MemberId: {request.AssignedMemberId}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Activity assigned to Team '{team.TeamName}'" + (teamMember != null ? $" and team member '{teamMember.EmployeeName}'" : "") + $". Old team ID was {oldTeamId}.",
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
            };

            return Result.Success(responseDto);
        }
    }
}
