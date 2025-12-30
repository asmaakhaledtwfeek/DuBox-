using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;

namespace Dubox.Application.Features.Activities.Commands;

using Dubox.Application.Specifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateBoxActivityStatusCommandHandler : IRequestHandler<UpdateBoxActivityStatusCommand, Result<BoxActivityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    
    public UpdateBoxActivityStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<BoxActivityDto>> Handle(UpdateBoxActivityStatusCommand request, CancellationToken cancellationToken)
    {
        var activity = _unitOfWork.Repository<BoxActivity>()
             .GetEntityWithSpec(new GetBoxActivityByIdSpecification(request.BoxActivityId));

        if (activity == null)
            return Result.Failure<BoxActivityDto>("Box Activity not found.");

        // Check if project is archived
        var isArchived = await _visibilityService.IsProjectArchivedAsync(activity.Box.ProjectId, cancellationToken);
        if (isArchived)
        {
            return Result.Failure<BoxActivityDto>("Cannot update activity status in an archived project. Archived projects are read-only.");
        }

        // Check if project is on hold
        var isOnHold = await _visibilityService.IsProjectOnHoldAsync(activity.Box.ProjectId, cancellationToken);
        if (isOnHold)
        {
            return Result.Failure<BoxActivityDto>("Cannot update activity status in a project on hold. Projects on hold only allow project status changes.");
        }

        // Check if box is Dispatched - cannot perform any actions on activities
        if (activity.Box.Status == BoxStatusEnum.Dispatched)
        {
            return Result.Failure<BoxActivityDto>("Cannot update activity status. The box is dispatched and no actions are allowed on boxes or activities.");
        }

        // Check if box is OnHold - cannot perform actions on activities
        if (activity.Box.Status == BoxStatusEnum.OnHold)
        {
            return Result.Failure<BoxActivityDto>("Cannot update activity status. The box is on hold and no actions are allowed on activities. Only box status changes are allowed.");
        }

        var oldStatus = activity.Status;
        var newStatus = request.Status;
        if (newStatus == oldStatus)
            return Result.Failure<BoxActivityDto>($"Box Activity is already in status: {oldStatus}. No status update needed.");

        // Validate activity status transitions based on business rules
        var statusValidationResult = ValidateActivityStatusTransition(oldStatus, newStatus, activity.ProgressPercentage);
        if (!statusValidationResult.IsValid)
        {
            return Result.Failure<BoxActivityDto>(statusValidationResult.ErrorMessage);
        }

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        var oldStatusString = oldStatus.ToString();
        var oldActualStartDateString = activity.ActualStartDate?.ToString(dateFormat) ?? "N/A";
        var oldActualEndDateString = activity.ActualEndDate?.ToString(dateFormat) ?? "N/A";

        activity.WorkDescription = request.WorkDescription;
        activity.IssuesEncountered = request.IssuesEncountered;

        if (newStatus != oldStatus)
        {
            if (newStatus == BoxStatusEnum.InProgress && !activity.ActualStartDate.HasValue)
            {
                activity.ActualStartDate = DateTime.UtcNow;
                await UpdateParentBoxAndProjectStatusIfNecessary(activity, cancellationToken);
            }
            else if (newStatus != BoxStatusEnum.InProgress && activity.ActualEndDate.HasValue)
            {
                activity.ActualEndDate = null;
            }
        }

        activity.Status = newStatus;
        activity.ModifiedDate = DateTime.UtcNow;
        activity.ModifiedBy = currentUserId;

        var newActualStartDateString = activity.ActualStartDate?.ToString(dateFormat) ?? "N/A";
        var newActualEndDateString = activity.ActualEndDate?.ToString(dateFormat) ?? "N/A";

        _unitOfWork.Repository<BoxActivity>().Update(activity);
        var log = new AuditLog
        {
            TableName = nameof(BoxActivity),
            RecordId = activity.BoxActivityId,
            Action = "StatusChange",
            OldValues = $"Status: {oldStatusString}, Start: {oldActualStartDateString}, End: {oldActualEndDateString}",
            NewValues = $"Status: {newStatus.ToString()}, Start: {newActualStartDateString}, End: {newActualEndDateString}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Activity status manually updated from {oldStatusString} to {newStatus.ToString()}."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(activity.Adapt<BoxActivityDto>());
    }

    private (bool IsValid, string ErrorMessage) ValidateActivityStatusTransition(BoxStatusEnum oldStatus, BoxStatusEnum newStatus, decimal progressPercentage)
    {
        // Rule 1: If activity status is "NotStarted" or "InProgress", user can only change status to "OnHold"
        if (oldStatus == BoxStatusEnum.NotStarted || oldStatus == BoxStatusEnum.InProgress)
        {
            if (newStatus != BoxStatusEnum.OnHold)
            {
                return (false, $"Cannot change activity status from {oldStatus} to {newStatus}. Activities in '{oldStatus}' status can only be changed to 'OnHold'.");
            }
            return (true, string.Empty);
        }

        // Rule 2: If activity status is "Completed", user cannot change status
        if (oldStatus == BoxStatusEnum.Completed)
        {
            return (false, "Cannot change activity status. Activities in 'Completed' status cannot be modified.");
        }

        // Rule 3: If status is "OnHold", user can:
        //   - Change to "NotStarted" if activity progress = 0
        //   - Change to "InProgress" if activity progress < 100
        if (oldStatus == BoxStatusEnum.OnHold)
        {
            if (newStatus == BoxStatusEnum.NotStarted)
            {
                if (progressPercentage != 0)
                {
                    return (false, "Cannot change activity status from 'OnHold' to 'NotStarted'. This transition is only allowed when activity progress is 0%.");
                }
                return (true, string.Empty);
            }
            else if (newStatus == BoxStatusEnum.InProgress)
            {
                if (progressPercentage >= 100)
                {
                    return (false, "Cannot change activity status from 'OnHold' to 'InProgress'. This transition is only allowed when activity progress is less than 100%.");
                }
                return (true, string.Empty);
            }
            else
            {
                return (false, $"Cannot change activity status from 'OnHold' to '{newStatus}'. Activities on hold can only be changed to 'NotStarted' (if progress = 0%) or 'InProgress' (if progress < 100%).");
            }
        }

        // Allow other transitions (if any) - this covers edge cases
        return (true, string.Empty);
    }

    private async Task UpdateParentBoxAndProjectStatusIfNecessary(BoxActivity activity, CancellationToken cancellationToken)
    {
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        const string dateFormat = "yyyy-MM-dd HH:mm:ss";

        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(activity.BoxId, cancellationToken);

        if (box != null && box.Status == BoxStatusEnum.NotStarted)
        {
            var boxOldStatus = box.Status.ToString();
            var boxOldActualStartDateString = box.ActualStartDate?.ToString(dateFormat) ?? "N/A";

            box.Status = BoxStatusEnum.InProgress;
            box.ActualStartDate = DateTime.UtcNow;
            _unitOfWork.Repository<Box>().Update(box);

            var boxNewActualStartDateString = box.ActualStartDate?.ToString(dateFormat) ?? "N/A";

            var boxLog = new AuditLog
            {
                TableName = nameof(Box),
                RecordId = box.BoxId,
                Action = "StatusAutoChange",
                OldValues = $"Status: {boxOldStatus}, Start: {boxOldActualStartDateString}",
                NewValues = $"Status: {BoxStatusEnum.InProgress.ToString()}, Start: {boxNewActualStartDateString}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Box status automatically moved to InProgress due to activity {activity.BoxActivityId} start."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(boxLog, cancellationToken);

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(box.ProjectId, cancellationToken);

            if (project != null)
            {

                var projectOldActualStartDateString = project.ActualStartDate?.ToString(dateFormat) ?? "N/A";
                if (!project.ActualStartDate.HasValue)
                {
                    project.ActualStartDate = DateTime.UtcNow;

                    _unitOfWork.Repository<Project>().Update(project);

                    var projectNewActualStartDateString = project.ActualStartDate?.ToString(dateFormat) ?? "N/A";

                    var projectLog = new AuditLog
                    {
                        TableName = nameof(Project),
                        RecordId = project.ProjectId,
                        Action = "ActualStartSet",
                        OldValues = $"Start: {projectOldActualStartDateString}",
                        NewValues = $"Start: {projectNewActualStartDateString}",
                        ChangedBy = currentUserId,
                        ChangedDate = DateTime.UtcNow,
                        Description = $"Project start date automatically initialized due to Box {box.BoxId} start."
                    };
                    await _unitOfWork.Repository<AuditLog>().AddAsync(projectLog, cancellationToken);
                }
            }
        }
    }
}

