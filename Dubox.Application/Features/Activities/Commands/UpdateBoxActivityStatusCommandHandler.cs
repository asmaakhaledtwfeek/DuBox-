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
        var module = PermissionModuleEnum.Activities;
        var action = PermissionActionEnum.UpdateStatus;
        var canModify = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
        if (!canModify)
            return Result.Failure<BoxActivityDto>("Access denied. You do not have permission to update activities.");

        var activity = _unitOfWork.Repository<BoxActivity>()
             .GetEntityWithSpec(new GetBoxActivityByIdSpecification(request.BoxActivityId));

        if (activity == null)
            return Result.Failure<BoxActivityDto>("Box Activity not found.");

       
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(activity.Box.ProjectId, cancellationToken);
        if (!canAccessProject)
            return Result.Failure<BoxActivityDto>("Access denied. You do not have permission to modify activities in this project.");

        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(activity.Box.ProjectId, "update activity status", cancellationToken);

        if (!projectStatusValidation.IsSuccess)
            return Result.Failure<BoxActivityDto>(projectStatusValidation.Error!);

        var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(activity.Box.BoxId, "update activity status", cancellationToken);

        if (!boxStatusValidation.IsSuccess)
            return Result.Failure<BoxActivityDto>(boxStatusValidation.Error!);

        var oldStatus = activity.Status;
        var newStatus = request.Status;
        if (newStatus == oldStatus)
            return Result.Failure<BoxActivityDto>($"Box Activity is already in status: {oldStatus}. No status update needed.");

        // Validate activity status transitions based on business rules
        var statusValidationResult = ValidateActivityStatusTransition(oldStatus, newStatus, activity.ProgressPercentage);
        if (!statusValidationResult.IsValid)
            return Result.Failure<BoxActivityDto>(statusValidationResult.ErrorMessage);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        var oldStatusString = oldStatus.ToString();
        var oldActualStartDateString = activity.ActualStartDate?.ToString(dateFormat) ?? "N/A";
        var oldActualEndDateString = activity.ActualEndDate?.ToString(dateFormat) ?? "N/A";

        activity.WorkDescription = request.WorkDescription;
        activity.IssuesEncountered = request.IssuesEncountered;


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
        if (oldStatus == BoxStatusEnum.NotStarted || oldStatus == BoxStatusEnum.InProgress)
        {
            if (newStatus != BoxStatusEnum.OnHold)
                return (false, $"Cannot change activity status from {oldStatus} to {newStatus}. Activities in '{oldStatus}' status can only be changed to 'OnHold'.");
            return (true, string.Empty);
        }

        if (oldStatus == BoxStatusEnum.Completed)
            return (false, "Cannot change activity status. Activities in 'Completed' status cannot be modified.");

        if (oldStatus == BoxStatusEnum.OnHold)
        {
            if (newStatus == BoxStatusEnum.NotStarted)
            {
                if (progressPercentage != 0)
                    return (false, "Cannot change activity status from 'OnHold' to 'NotStarted'. This transition is only allowed when activity progress is 0%.");
                return (true, string.Empty);
            }
            else if (newStatus == BoxStatusEnum.InProgress)
            {
                if (progressPercentage >= 100)
                    return (false, "Cannot change activity status from 'OnHold' to 'InProgress'. This transition is only allowed when activity progress is less than 100%.");
                return (true, string.Empty);
            }
            else
                return (false, $"Cannot change activity status from 'OnHold' to '{newStatus}'. Activities on hold can only be changed to 'NotStarted' (if progress = 0%) or 'InProgress' (if progress < 100%).");

        }
        return (true, string.Empty);
    }

}