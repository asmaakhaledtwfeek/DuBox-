using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ProgressUpdates.Commands;

public class CreateProgressUpdateCommandHandler : IRequestHandler<CreateProgressUpdateCommand, Result<ProgressUpdateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateProgressUpdateCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ProgressUpdateDto>> Handle(CreateProgressUpdateCommand request, CancellationToken cancellationToken)
    {

        var boxActivity = _unitOfWork.Repository<BoxActivity>().
            GetEntityWithSpec(new BoxActivitiesWithIncludesSpecification(request.BoxActivityId, request.BoxId));

        if (boxActivity == null)
            return Result.Failure<ProgressUpdateDto>("Box activity not found");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);

        if (user == null)
            return Result.Failure<ProgressUpdateDto>("User not found");
        BoxStatusEnum inferredStatus;
        if (request.ProgressPercentage >= 100)
            inferredStatus = BoxStatusEnum.Completed;
        else if (request.ProgressPercentage > 0)
            inferredStatus = BoxStatusEnum.InProgress;
        else
        {
            inferredStatus = BoxStatusEnum.NotStarted;
            if (boxActivity.Status == BoxStatusEnum.OnHold)
                inferredStatus = BoxStatusEnum.OnHold;
        }
        var isDuplicate = await _unitOfWork.Repository<ProgressUpdate>()
                             .IsExistAsync(pu =>
                                pu.BoxId == request.BoxId &&
                                pu.BoxActivityId == request.BoxActivityId &&
                                pu.ProgressPercentage == request.ProgressPercentage &&
                                pu.Status == inferredStatus);

        if (isDuplicate)
            return Result.Failure<ProgressUpdateDto>("A progress update with the exact same details already exists for this activity.");

        var progressUpdate = request.Adapt<ProgressUpdate>();
        progressUpdate.Status = inferredStatus;
        progressUpdate.UpdatedBy = currentUserId;
        progressUpdate.UpdateDate = DateTime.UtcNow;
        progressUpdate.CreatedDate = DateTime.UtcNow;

        await _unitOfWork.Repository<ProgressUpdate>().AddAsync(progressUpdate, cancellationToken);

        const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        var oldStatus = boxActivity.Status;
        var oldProgress = boxActivity.ProgressPercentage;
        var oldActualStartDate = boxActivity.ActualStartDate;
        var oldActualEndDate = boxActivity.ActualEndDate;

        boxActivity.ProgressPercentage = request.ProgressPercentage;
        boxActivity.Status = inferredStatus;
        boxActivity.WorkDescription = request.WorkDescription;
        boxActivity.IssuesEncountered = request.IssuesEncountered;
        boxActivity.ModifiedDate = DateTime.UtcNow;
        boxActivity.ModifiedBy = currentUserId;

        if (inferredStatus == BoxStatusEnum.InProgress && boxActivity.ActualStartDate == null)
            boxActivity.ActualStartDate = DateTime.UtcNow;

        if (inferredStatus == BoxStatusEnum.Completed && oldStatus != BoxStatusEnum.Completed)
        {
            boxActivity.ActualEndDate = DateTime.UtcNow;
            boxActivity.ProgressPercentage = 100;
        }
        var newActualStartDateString = boxActivity.ActualStartDate?.ToString(dateFormat) ?? "N/A";
        var newActualEndDateString = boxActivity.ActualEndDate?.ToString(dateFormat) ?? "N/A";

        _unitOfWork.Repository<BoxActivity>().Update(boxActivity);
        var log = new AuditLog
        {
            TableName = nameof(BoxActivity),
            RecordId = boxActivity.BoxActivityId,
            Action = "ProgressUpdate",
            OldValues = $"Progress: {oldProgress}%, Status: {oldStatus.ToString()}, Start: {oldActualStartDate?.ToString(dateFormat) ?? "N/A"}, End: {oldActualEndDate?.ToString(dateFormat) ?? "N/A"}",
            NewValues = $"Progress: {boxActivity.ProgressPercentage}%, Status: {boxActivity.Status.ToString()}, Start: {newActualStartDateString}, End: {newActualEndDateString}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Progress updated to {boxActivity.ProgressPercentage}%. Status inferred to {boxActivity.Status.ToString()}."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);

        await UpdateBoxProgress(request.BoxId, currentUserId, cancellationToken);

        if (boxActivity.Box != null)
            await UpdateProjectProgress(boxActivity.Box.ProjectId, currentUserId, cancellationToken);


        if (inferredStatus == BoxStatusEnum.Completed && boxActivity.ActivityMaster.IsWIRCheckpoint)
        {
            var wirExists = await _dbContext.WIRRecords
                .AnyAsync(w => w.BoxActivityId == request.BoxActivityId, cancellationToken);

            if (!wirExists)
            {
                var wirRecord = new WIRRecord
                {
                    BoxActivityId = request.BoxActivityId,
                    WIRCode = boxActivity.ActivityMaster.WIRCode ?? "WIR",
                    Status = WIRRecordStatusEnum.Pending,
                    RequestedDate = DateTime.UtcNow,
                    RequestedBy = currentUserId,
                    PhotoUrls = progressUpdate.PhotoUrls,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<WIRRecord>().AddAsync(wirRecord, cancellationToken);
            }
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = progressUpdate.Adapt<ProgressUpdateDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            UpdatedByName = user.FullName ?? user.Email
        };

        return Result.Success(dto);
    }

    private async Task UpdateBoxProgress(Guid boxId, Guid currentUserId, CancellationToken cancellationToken)
    {
        var boxActivities = await _unitOfWork.Repository<BoxActivity>()
            .FindAsync(ba => ba.BoxId == boxId && ba.IsActive, cancellationToken);

        if (!boxActivities.Any())
            return;

        var averageProgress = boxActivities.Average(ba => ba.ProgressPercentage);
        var allCompleted = boxActivities.All(ba => ba.Status == BoxStatusEnum.Completed);

        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxId, cancellationToken);
        if (box != null)
        {
            var oldProgress = box.ProgressPercentage;
            var oldStatus = box.Status;
            const string dateFormat = "yyyy-MM-dd HH:mm:ss";
            var oldActualStartDateString = box.ActualStartDate?.ToString(dateFormat) ?? "N/A";
            var oldActualEndDateString = box.ActualEndDate?.ToString(dateFormat) ?? "N/A";

            bool isProgressChanged = oldProgress != averageProgress;
            bool isStatusChanged = false;

            box.ProgressPercentage = averageProgress;

            if (allCompleted && oldStatus != BoxStatusEnum.Completed)
            {
                box.Status = BoxStatusEnum.Completed;
                box.ActualEndDate = DateTime.UtcNow;
                isStatusChanged = true;
            }
            else if (averageProgress > 0 && oldStatus != BoxStatusEnum.InProgress && oldStatus != BoxStatusEnum.Completed)
            {
                box.Status = BoxStatusEnum.InProgress;
                if (box.ActualStartDate == null)
                    box.ActualStartDate = DateTime.UtcNow;
                isStatusChanged = true;
            }
            if (isProgressChanged || isStatusChanged)
            {
                box.ModifiedDate = DateTime.UtcNow;
                box.ModifiedBy = currentUserId;
                _unitOfWork.Repository<Box>().Update(box);

                var newActualStartDateString = box.ActualStartDate?.ToString(dateFormat) ?? "N/A";
                var newActualEndDateString = box.ActualEndDate?.ToString(dateFormat) ?? "N/A";

                var log = new AuditLog
                {
                    TableName = nameof(Box),
                    RecordId = box.BoxId,
                    Action = isStatusChanged ? "StatusAutoChange" : "ProgressAutoUpdate",
                    OldValues = $"Progress: {oldProgress}%, Status: {oldStatus.ToString()}, Start: {oldActualStartDateString}, End: {oldActualEndDateString}",
                    NewValues = $"Progress: {box.ProgressPercentage}%, Status: {box.Status.ToString()}, Start: {newActualStartDateString}, End: {newActualEndDateString}",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"Box progress updated automatically from {oldProgress}% to {box.ProgressPercentage}%."
                };
                await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
            }

        }
    }

    private async Task UpdateProjectProgress(Guid projectId, Guid currentUserId, CancellationToken cancellationToken)
    {
        var projectBoxes = await _unitOfWork.Repository<Box>()
            .FindAsync(b => b.ProjectId == projectId, cancellationToken);

        if (!projectBoxes.Any())
            return;

        var averageProgress = projectBoxes.Average(b => b.ProgressPercentage);
        var allCompleted = projectBoxes.All(b => b.Status == BoxStatusEnum.Completed);

        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(projectId, cancellationToken);

        if (project != null)
        {
            var oldProgress = project.ProgressPercentage;
            var oldStatus = project.Status;
            const string dateFormat = "yyyy-MM-dd HH:mm:ss";
            var oldActualStartDateString = project.ActualStartDate?.ToString(dateFormat) ?? "N/A";
            var oldActualEndDateString = project.ActualEndDate?.ToString(dateFormat) ?? "N/A";

            bool isProgressChanged = oldProgress != averageProgress;
            bool isStatusChanged = false;

            project.ProgressPercentage = averageProgress;

            if (allCompleted && oldStatus != ProjectStatusEnum.Completed)
            {
                project.Status = ProjectStatusEnum.Completed;
                project.ActualEndDate = DateTime.UtcNow;
                isStatusChanged = true;
            }
            else if (averageProgress > 0 && oldStatus != ProjectStatusEnum.Completed && oldStatus != ProjectStatusEnum.OnHold && oldStatus != ProjectStatusEnum.Active)
            {
                project.Status = ProjectStatusEnum.Active;
                if (project.ActualStartDate == null)
                    project.ActualStartDate = DateTime.UtcNow;
                isStatusChanged = true;
            }

            if (isProgressChanged || isStatusChanged)
            {
                project.ModifiedDate = DateTime.UtcNow;
                project.ModifiedBy = _currentUserService.Username;
                _unitOfWork.Repository<Project>().Update(project);

                var newActualStartDateString = project.ActualStartDate?.ToString(dateFormat) ?? "N/A";
                var newActualEndDateString = project.ActualEndDate?.ToString(dateFormat) ?? "N/A";

                var log = new AuditLog
                {
                    TableName = nameof(Project),
                    RecordId = project.ProjectId,
                    Action = isStatusChanged ? "StatusAutoChange" : "ProgressAutoUpdate",
                    OldValues = $"Progress: {oldProgress}%, Status: {oldStatus.ToString()}, Start: {oldActualStartDateString}, End: {oldActualEndDateString}",
                    NewValues = $"Progress: {project.ProgressPercentage}%, Status: {project.Status.ToString()}, Start: {newActualStartDateString}, End: {newActualEndDateString}",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"Project progress updated automatically from {oldProgress}% to {project.ProgressPercentage}%."
                };
                await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
            }
        }
    }
}