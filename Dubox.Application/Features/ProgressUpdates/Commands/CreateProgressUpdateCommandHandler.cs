using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Dubox.Application.Features.ProgressUpdates.Commands;

public class CreateProgressUpdateCommandHandler : IRequestHandler<CreateProgressUpdateCommand, Result<ProgressUpdateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public CreateProgressUpdateCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService,
        IImageProcessingService imageProcessingService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _imageProcessingService = imageProcessingService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<ProgressUpdateDto>> Handle(CreateProgressUpdateCommand request, CancellationToken cancellationToken)
    {
        var module = PermissionModuleEnum.ProgressUpdates;
        var action = PermissionActionEnum.Create;
        var canUpdate = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
        if(!canUpdate)
            return Result.Failure<ProgressUpdateDto>("Access denied. You do not have permission to update activities progress.");

        var boxActivity = _unitOfWork.Repository<BoxActivity>().
            GetEntityWithSpec(new BoxActivitiesWithIncludesSpecification(request.BoxActivityId, request.BoxId));

        var validationResult= await ValidateProgressUpdateAsync(request ,boxActivity, cancellationToken);

        if (validationResult.IsFailure)
            return Result.Failure<ProgressUpdateDto>(validationResult.Error);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);

        if (user == null)
            return Result.Failure<ProgressUpdateDto>("User not found");

        BoxStatusEnum inferredStatus;
        if (request.ProgressPercentage >= 100) 
            inferredStatus = DetermineActivityStatusAtCompletion(boxActivity);

        else if (request.ProgressPercentage > 0)
            inferredStatus = BoxStatusEnum.InProgress;
        else
        {
            inferredStatus = BoxStatusEnum.NotStarted;
            if (boxActivity.Status == BoxStatusEnum.OnHold)
                inferredStatus = BoxStatusEnum.OnHold;
        }
        var duplicatedProgress = _unitOfWork.Repository<ProgressUpdate>().GetEntityWithSpec(
            new GetProgressUpdatesByActivitySpecification(request.BoxId, request.BoxActivityId, request.ProgressPercentage, inferredStatus));
                
        if (duplicatedProgress !=null)
        {
           var result= await CreateWIRRecordOrUpdatePositionIfWIRExist(request, boxActivity, currentUserId, cancellationToken);
            if(!result.IsSuccess)
            return Result.Failure<ProgressUpdateDto>(result.ErrorMessage);
            else
            {
               await _unitOfWork.CompleteAsync();
                var duplicatedDto = duplicatedProgress.Adapt<ProgressUpdateDto>() with
                {
                    BoxTag = boxActivity.Box.BoxTag,
                    ActivityName = boxActivity.ActivityMaster.ActivityName,
                    UpdatedByName = user.FullName ?? user.Email,
                };

                return Result.Success(duplicatedDto);
            }

        }

        var progressUpdate = request.Adapt<ProgressUpdate>();
        progressUpdate.Status = inferredStatus;
        progressUpdate.UpdatedBy = currentUserId;
        progressUpdate.UpdateDate = DateTime.UtcNow;
        progressUpdate.CreatedDate = DateTime.UtcNow;

        if (string.IsNullOrEmpty(progressUpdate.UpdateMethod))
        {
            progressUpdate.UpdateMethod = "Web";
        }
        else if (progressUpdate.UpdateMethod.Length > 50)
        {
            progressUpdate.UpdateMethod = progressUpdate.UpdateMethod.Substring(0, 50);
        }

        if (!string.IsNullOrEmpty(progressUpdate.DeviceInfo) && progressUpdate.DeviceInfo.Length > 100)
        {
            progressUpdate.DeviceInfo = progressUpdate.DeviceInfo.Substring(0, 100);
        }

        try
        {
            await _unitOfWork.Repository<ProgressUpdate>().AddAsync(progressUpdate, cancellationToken);
            var result=await CreateWIRRecordOrUpdatePositionIfWIRExist(request, boxActivity, currentUserId, cancellationToken);
            if(!result.IsSuccess)
                return Result.Failure<ProgressUpdateDto>(result.ErrorMessage);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await UpdateBoxActivityAndAddAuditLog(request, boxActivity, inferredStatus, currentUserId, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            progressUpdate.BoxProgressSnapshot = await UpdateBoxProgress(request.BoxId, currentUserId, cancellationToken);
            _unitOfWork.Repository<ProgressUpdate>().Update(progressUpdate);
            await _unitOfWork.CompleteAsync(cancellationToken);

            if (boxActivity.Box != null)
                await UpdateProjectProgress(boxActivity.Box.ProjectId, currentUserId, cancellationToken);
            
                await _unitOfWork.CompleteAsync(cancellationToken);
           

        }
        catch (Exception ex)
        {
            return Result.Failure<ProgressUpdateDto>($"Failed to save progress update: {ex.Message}");
        }

        // Get ALL existing ProgressUpdateImages for this box (across all progress updates) for version checking
        var allProgressUpdatesForBox = _dbContext.ProgressUpdates
            .Where(pu => pu.BoxId == progressUpdate.BoxId)
            .Select(pu => pu.ProgressUpdateId)
            .ToList();
        
        var existingImagesForBox = _dbContext.Set<ProgressUpdateImage>()
            .Where(img => allProgressUpdatesForBox.Contains(img.ProgressUpdateId))
            .ToList();
        
        Console.WriteLine($"🔍 VERSION DEBUG - Found {existingImagesForBox.Count} existing ProgressUpdateImages across all progress updates for Box {progressUpdate.BoxId}");
        
        var imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<ProgressUpdateImage>(
            progressUpdate.ProgressUpdateId, 
            request.Files, 
            request.ImageUrls, 
            cancellationToken, 
            fileNames: request.FileNames,
            existingImagesForVersioning: existingImagesForBox
        );
        if (!imagesProcessResult.IsSuccess)
            return Result.Failure<ProgressUpdateDto>(imagesProcessResult.ErrorMessage);

        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = progressUpdate.Adapt<ProgressUpdateDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            UpdatedByName = user.FullName ?? user.Email,
        };

        return Result.Success(dto);
    }

    
    private async Task<decimal> UpdateBoxProgress(Guid boxId, Guid currentUserId, CancellationToken cancellationToken)
    {
        var boxActivities = await _unitOfWork.Repository<BoxActivity>()
            .FindAsync(ba => ba.BoxId == boxId && ba.IsActive, cancellationToken);

        if (!boxActivities.Any())
            return 0;

        var averageProgress = boxActivities.Average(ba => ba.ProgressPercentage);
      
        var allCompleted = boxActivities.All(ba => ba.ProgressPercentage >= 100);

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

            return box.ProgressPercentage;

        }
        return 0;

    }

    private async Task UpdateProjectProgress(Guid projectId, Guid currentUserId, CancellationToken cancellationToken)
    {
        var projectBoxes = await _unitOfWork.Repository<Box>()
            .FindAsync(b => b.ProjectId == projectId, cancellationToken);

        if (!projectBoxes.Any())
            return;

        var averageProgress = projectBoxes.Average(b => b.ProgressPercentage);
        var allCompleted = projectBoxes.All(b => b.Status == BoxStatusEnum.Completed||b.Status== BoxStatusEnum.Dispatched);

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
            else if (averageProgress > 0 && oldStatus != ProjectStatusEnum.Completed && oldStatus != ProjectStatusEnum.OnHold && project.ActualStartDate == null)
                project.ActualStartDate = DateTime.UtcNow;

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
    private async Task<Result> ValidateProgressUpdateAsync(CreateProgressUpdateCommand request, BoxActivity boxActivity, CancellationToken cancellationToken)
    {
        if (boxActivity == null)
            return Result.Failure("Box activity not found");

        if (request.BoxId == Guid.Empty)
            return Result.Failure("Invalid BoxId");

        if (request.BoxActivityId == Guid.Empty)
            return Result.Failure("Invalid BoxActivityId");

        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(boxActivity.Box.ProjectId, "create progress updates", cancellationToken);

        if (!projectStatusValidation.IsSuccess)
            return Result.Failure(projectStatusValidation.Error!);
        var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(boxActivity.Box.BoxId, "create progress update", cancellationToken);

        if (!boxStatusValidation.IsSuccess)
            return Result.Failure(boxStatusValidation.Error!);
        
        // Check activity status
        // Allow position-only updates for completed activities (if WIR position data is provided)
        bool isPositionOnlyUpdate = !string.IsNullOrWhiteSpace(request.WirBay) || 
                                   !string.IsNullOrWhiteSpace(request.WirRow) || 
                                   !string.IsNullOrWhiteSpace(request.WirPosition);
        
        if (boxActivity.Status == BoxStatusEnum.Completed || boxActivity.Status == BoxStatusEnum.Delayed)
        {            
            if (!isPositionOnlyUpdate || request.ProgressPercentage != boxActivity.ProgressPercentage)
                return Result.Failure("Cannot update progress for completed activities. Only position (Bay/Row) can be updated for completed activities.");
        }

        if (boxActivity.Status == BoxStatusEnum.OnHold)
            return Result.Failure("Cannot create progress update. Activities in 'OnHold' status cannot be modified. Please change the activity status first.");

        return Result.Success();
    }

    private async Task UpdateBoxActivityAndAddAuditLog(CreateProgressUpdateCommand request, BoxActivity boxActivity, BoxStatusEnum inferredStatus, Guid currentUserId, CancellationToken cancellationToken)
    {
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

        if ((inferredStatus == BoxStatusEnum.InProgress || inferredStatus == BoxStatusEnum.Completed || inferredStatus == BoxStatusEnum.Delayed) && boxActivity.ActualStartDate == null)
            boxActivity.ActualStartDate = DateTime.UtcNow;
        
        // Set ActualEndDate when activity reaches 100% progress (either Completed or Delayed)
        if ((inferredStatus == BoxStatusEnum.Completed || inferredStatus == BoxStatusEnum.Delayed) && oldStatus != BoxStatusEnum.Completed && oldStatus != BoxStatusEnum.Delayed)
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
        await _unitOfWork.CompleteAsync(cancellationToken);

    }

    
    private async Task<(bool IsSuccess, string ErrorMessage)> CreateWIRRecordOrUpdatePositionIfWIRExist(CreateProgressUpdateCommand request, BoxActivity currentActivity, Guid currentUserId, CancellationToken cancellationToken)
    {
        BoxActivity? nextWIRActivity = null;
        if (currentActivity.ActivityMaster.IsWIRCheckpoint)
           nextWIRActivity=currentActivity;
        else
            nextWIRActivity = _unitOfWork.Repository<BoxActivity>().GetEntityWithSpec(new BoxActivitiesWithIncludesSpecification(currentActivity));

        if (nextWIRActivity == null)
            return (true,"");

        if (!string.IsNullOrWhiteSpace(request.WirBay) || !string.IsNullOrWhiteSpace(request.WirRow) || !string.IsNullOrWhiteSpace(request.WirPosition))
        {
          var isValideLocation=  await ValidateUniqueLocationInFactory(
                request.WirBay ?? string.Empty, 
                request.WirRow ?? string.Empty, 
                request.WirPosition ?? string.Empty, 
                currentActivity.BoxId, 
                cancellationToken);
            if (!isValideLocation)
                return (false, $"Location conflict: Bay {request.WirBay}, Row {request.WirRow}, Position {request.WirPosition} is already occupied by another Box in this factory. Please select a different location.");

            await ValidatePreviousWIRApproved(nextWIRActivity, currentActivity.BoxId, cancellationToken);
        }

        WIRRecord? nearestWIR = _unitOfWork.Repository<WIRRecord>()
            .Get().Where(w => w.BoxActivityId == nextWIRActivity.BoxActivityId).FirstOrDefault();
        string oldBay = string.Empty;
        string oldRow =string.Empty;
        string oldPosition = string.Empty;
        bool positionUpdated = false;
        
        if (nearestWIR == null)
        {
            string bayValue = request.WirBay ?? string.Empty;
            string rowValue = request.WirRow ?? string.Empty;
            string positionValue = request.WirPosition ?? string.Empty;

            nearestWIR = new WIRRecord
            {
                BoxActivityId = nextWIRActivity.BoxActivityId,
                WIRCode = nextWIRActivity.ActivityMaster.WIRCode ?? "WIR",
                Status = WIRRecordStatusEnum.Pending,
                RequestedDate = DateTime.UtcNow,
                RequestedBy = currentUserId,
                CreatedDate = DateTime.UtcNow,
                Bay = !string.IsNullOrWhiteSpace(bayValue) ? bayValue : null,
                Row = !string.IsNullOrWhiteSpace(rowValue) ? rowValue : null,
                Position = !string.IsNullOrWhiteSpace(positionValue) ? positionValue : null
            };
            await _unitOfWork.Repository<WIRRecord>().AddAsync(nearestWIR, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            positionUpdated = !string.IsNullOrWhiteSpace(bayValue) || !string.IsNullOrWhiteSpace(rowValue);
        }
        else
        {
            // WIR record exists - update position only if values provided and not already set
            oldBay = nearestWIR.Bay ?? string.Empty;
            oldRow = nearestWIR.Row ?? string.Empty;
            oldPosition = nearestWIR.Position ?? string.Empty;
            
            // Try to update if new values are provided
            bool wirFieldsUpdated = UpdateWIRPosition(nearestWIR, request);
            if (wirFieldsUpdated)
            {
                nearestWIR.ModifiedDate = DateTime.UtcNow;
                _unitOfWork.Repository<WIRRecord>().Update(nearestWIR);
            }
            
            positionUpdated = !string.IsNullOrWhiteSpace(nearestWIR.Bay) || 
                             !string.IsNullOrWhiteSpace(nearestWIR.Row) || 
                             !string.IsNullOrWhiteSpace(nearestWIR.Position);
            }
        
        if (positionUpdated)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(currentActivity.BoxId, cancellationToken);
            if (box != null)
            {
                var latestWIRWithPosition = _unitOfWork.Repository<WIRRecord>().GetEntityWithSpec(
                    new GetWIRRecordSpecification(currentActivity.BoxId, true));
                if (latestWIRWithPosition != null)
                {
                    // Check if box location actually needs updating (avoid unnecessary updates)
                    bool boxNeedsUpdate = box.Bay != latestWIRWithPosition.Bay || 
                                         box.Row != latestWIRWithPosition.Row || 
                                         box.Position != latestWIRWithPosition.Position;
                    
                    if (boxNeedsUpdate)
                    {
                        // Validate that this location is not already occupied by another box in the same factory
                        var isValidateLocation = await ValidateUniqueLocationInFactory(
                            latestWIRWithPosition.Bay ?? string.Empty,
                            latestWIRWithPosition.Row ?? string.Empty,
                            latestWIRWithPosition.Position ?? string.Empty,
                            currentActivity.BoxId,
                            cancellationToken);
                        if (!isValidateLocation)
                            return (false, $"Location conflict: Bay {request.WirBay}, Row {request.WirRow}, Position {request.WirPosition} is already occupied by another Box in this factory. Please select a different location.");
                        string boxOldBay = box.Bay ?? string.Empty;
                        string boxOldRow = box.Row ?? string.Empty;
                        string boxOldPosition = box.Position ?? string.Empty;
                        
                        box.Bay = latestWIRWithPosition.Bay;
                        box.Row = latestWIRWithPosition.Row;
                        box.Position = latestWIRWithPosition.Position;
                        box.ModifiedDate = DateTime.UtcNow;
                        box.ModifiedBy = currentUserId;
                        _unitOfWork.Repository<Box>().Update(box);
                        
                        // Create audit log for box position update only if values changed
                        var boxAuditLog = new AuditLog
                        {
                            TableName = nameof(Box),
                            RecordId = currentActivity.BoxId,
                            Action = "BoxPositionUpdate",
                            OldValues = $"Bay: {boxOldBay ?? "N/A"}, Row: {boxOldRow ?? "N/A"}, Position: {boxOldPosition ?? "N/A"}",
                            NewValues = $"Bay: {latestWIRWithPosition.Bay ?? "N/A"}, Row: {latestWIRWithPosition.Row ?? "N/A"}, Position: {latestWIRWithPosition.Position ?? "N/A"}",
                            ChangedBy = currentUserId,
                            ChangedDate = DateTime.UtcNow,
                            Description = $"Box position synchronized with latest WIR position. Box: {box.BoxTag}, WIR: {latestWIRWithPosition.WIRCode}"
                        };
                        await _unitOfWork.Repository<AuditLog>().AddAsync(boxAuditLog, cancellationToken);
                    }
                }
            }
            
            // Create audit log for WIR position update only if WIR values actually changed
            if (oldBay != nearestWIR.Bay || oldRow != nearestWIR.Row || oldPosition != nearestWIR.Position)
            {
            var wIRAuditLog = new AuditLog
            {
                TableName = nameof(WIRRecord),
                RecordId = nearestWIR.WIRRecordId,
                Action = "WIRPositionUpdate",
                OldValues = $"Bay: {oldBay ?? "N/A"}, Row: {oldRow ?? "N/A"}, Position: {oldPosition ?? "N/A"}",
                NewValues = $"Bay: {nearestWIR.Bay ?? "N/A"}, Row: {nearestWIR.Row ?? "N/A"}, Position: {nearestWIR.Position ?? "N/A"}",
                    ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"WIR position updated from activity progress update. WIR: {nearestWIR.WIRCode}"
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(wIRAuditLog, cancellationToken);
            }
        }
        
        return (true,string.Empty);
    }
    private bool UpdateWIRPosition(WIRRecord nearestWIR, CreateProgressUpdateCommand request)
    {
        bool positionUpdated = false;

        if (string.IsNullOrWhiteSpace(request.WirBay) &&string.IsNullOrWhiteSpace(request.WirRow) && string.IsNullOrWhiteSpace(request.WirPosition))
            return positionUpdated; // No position data to update

        if (!string.IsNullOrWhiteSpace(request.WirBay) && string.IsNullOrWhiteSpace(nearestWIR.Bay))
        {
            nearestWIR.Bay = request.WirBay.Trim();
            if (nearestWIR.Bay.Length > 50)
                nearestWIR.Bay = nearestWIR.Bay.Substring(0, 50);
            positionUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(request.WirRow) && string.IsNullOrWhiteSpace(nearestWIR.Row))
        {
            nearestWIR.Row = request.WirRow.Trim();
            if (nearestWIR.Row.Length > 50)
                nearestWIR.Row = nearestWIR.Row.Substring(0, 50);
            positionUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(request.WirPosition) && string.IsNullOrWhiteSpace(nearestWIR.Position))
        {
            nearestWIR.Position = request.WirPosition.Trim();
            if (nearestWIR.Position.Length > 50)
                nearestWIR.Position = nearestWIR.Position.Substring(0, 50);
            positionUpdated = true;
        }
        return positionUpdated;
    }

  
    private async Task ValidatePreviousWIRApproved(BoxActivity currentWIRActivity, Guid boxId, CancellationToken cancellationToken)
    {
        // Find the previous WIR activity for this box (by sequence)
        var previousWIRActivity = await _dbContext.BoxActivities
            .Include(x => x.ActivityMaster)
            .Where(x => x.BoxId == boxId &&
                       x.Sequence < currentWIRActivity.Sequence &&
                       x.ActivityMaster.IsWIRCheckpoint)
            .OrderByDescending(x => x.Sequence)
            .FirstOrDefaultAsync(cancellationToken);

        // If no previous WIR exists, validation passes (this is the first WIR)
        if (previousWIRActivity == null)
        {
            return;
        }

        // Check if previous WIR record exists and its status
        var previousWIR = await _dbContext.WIRRecords
            .Where(w => w.BoxActivityId == previousWIRActivity.BoxActivityId)
            .FirstOrDefaultAsync(cancellationToken);

        // If previous WIR record doesn't exist yet, don't allow current WIR position update
        if (previousWIR == null)
        {
            throw new InvalidOperationException(
                $"Cannot update WIR position. Previous WIR '{previousWIRActivity.ActivityMaster.WIRCode}' has not been created yet.");
        }

        var previousCheckpoint = await _dbContext.WIRCheckpoints
            .Include(cp => cp.ChecklistItems)
            .Where(cp => cp.BoxId == boxId && cp.WIRCode == previousWIR.WIRCode)
            .FirstOrDefaultAsync(cancellationToken);

        var effectiveStatus = previousCheckpoint?.Status ?? (WIRCheckpointStatusEnum)previousWIR.Status;

        // ConditionallyApproved is ALWAYS acceptable - validation passes immediately
        if (effectiveStatus == WIRCheckpointStatusEnum.ConditionalApproval)
        {
            return; // Allow position update for conditionally approved WIRs
        }

        // Check if previous WIR status is Pending or Rejected
        if (effectiveStatus == WIRCheckpointStatusEnum.Pending)
        {
            throw new InvalidOperationException(
                $"Cannot update WIR position. Previous WIR '{previousWIR.WIRCode}' is still Pending. " +
                $"It must be approved first.");
        }

        if (effectiveStatus == WIRCheckpointStatusEnum.Rejected)
        {
            throw new InvalidOperationException(
                $"Cannot update WIR position. Previous WIR '{previousWIR.WIRCode}' was Rejected. " +
                $"Issues must be resolved and it must be approved.");
        }

        // If status is Approved, check if it's "Under Review" (not all checklist items are Pass)
        if (effectiveStatus == WIRCheckpointStatusEnum.Approved)
        {
            if (previousCheckpoint != null)
            {
                // If no checklist items exist OR checklist is empty, consider it Under Review
                if (previousCheckpoint.ChecklistItems == null || !previousCheckpoint.ChecklistItems.Any())
                {
                    throw new InvalidOperationException(
                        $"Cannot update WIR position. Previous WIR '{previousWIR.WIRCode}' is Under Review. " +
                        $"Checklist items must be added and approved before proceeding.");
                }
                
                // Check if all checklist items have "Pass" status
                bool allItemsPass = previousCheckpoint.ChecklistItems.All(item => 
                    item.Status == CheckListItemStatusEnum.Pass);

                if (!allItemsPass)
                {
                    throw new InvalidOperationException(
                        $"Cannot update WIR position. Previous WIR '{previousWIR.WIRCode}' is Under Review. " +
                        $"All checklist items must have 'Pass' status before proceeding.");
                }
            }
            else
            {
                // No checkpoint exists for an Approved WIR - inconsistent state
                throw new InvalidOperationException(
                    $"Cannot update WIR position. Previous WIR '{previousWIR.WIRCode}' is Approved but has no checkpoint. " +
                    $"A QA/QC checkpoint must be created and approved before proceeding.");
            }
        }

    }

    private async Task<bool> ValidateUniqueLocationInFactory(string bayValue, string rowValue, string positionValue, Guid currentBoxId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(bayValue) && string.IsNullOrWhiteSpace(rowValue) && string.IsNullOrWhiteSpace(positionValue))
            return true;

        var currentBox = await _unitOfWork.Repository<Box>().GetByIdAsync(currentBoxId, cancellationToken);
        if (currentBox == null || !currentBox.FactoryId.HasValue)
            return true; // No factory assigned, skip validation

        bayValue = bayValue?.Trim() ?? string.Empty;
        rowValue = rowValue?.Trim() ?? string.Empty;
        positionValue = positionValue?.Trim() ?? string.Empty;

        // If the current box already has this exact position, allow it (no conflict)
        if (currentBox.Bay?.Trim() == bayValue &&  currentBox.Row?.Trim() == rowValue &&currentBox.Position?.Trim() == positionValue)
            return true;

        var conflictingBox= _unitOfWork.Repository<Box>().GetEntityWithSpec(new GetBoxWithIncludesSpecification(
            currentBoxId,currentBox.FactoryId.Value,bayValue,rowValue,positionValue
            ));
        if (conflictingBox != null)
            return false;
        return true;
    }

 
    private BoxStatusEnum DetermineActivityStatusAtCompletion(BoxActivity boxActivity)
    {
        if (!boxActivity.Duration.HasValue || boxActivity.Duration.Value <= 0)
            return BoxStatusEnum.Completed;

        DateTime? actualStart = boxActivity.ActualStartDate;
        DateTime actualEnd = boxActivity.ActualEndDate ?? DateTime.UtcNow; // Use existing end date or current time

        if (!actualStart.HasValue)
            return BoxStatusEnum.Completed; 

        var actualDurationDays = (actualEnd - actualStart.Value).TotalDays;
        var plannedDurationDays = boxActivity.Duration.Value;

        if (actualDurationDays > plannedDurationDays)
            return BoxStatusEnum.Delayed;

        return BoxStatusEnum.Completed;
    }
}