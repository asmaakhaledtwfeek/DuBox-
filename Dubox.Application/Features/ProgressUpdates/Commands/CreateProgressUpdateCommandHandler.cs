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
    private readonly IImageProcessingService _imageProcessingService;

    public CreateProgressUpdateCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService,
        IImageProcessingService imageProcessingService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _imageProcessingService = imageProcessingService;
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
        //var isDuplicate = await _unitOfWork.Repository<ProgressUpdate>()
        //                     .IsExistAsync(pu =>
        //                        pu.BoxId == request.BoxId &&
        //                        pu.BoxActivityId == request.BoxActivityId &&
        //                        pu.ProgressPercentage == request.ProgressPercentage &&
        //                        pu.Status == inferredStatus);

        //if (isDuplicate)
        //    return Result.Failure<ProgressUpdateDto>("A progress update with the exact same details already exists for this activity.");

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

        if (progressUpdate.BoxId == Guid.Empty)
            return Result.Failure<ProgressUpdateDto>("Invalid BoxId");

        if (progressUpdate.BoxActivityId == Guid.Empty)
            return Result.Failure<ProgressUpdateDto>("Invalid BoxActivityId");

        try
        {
            await _unitOfWork.Repository<ProgressUpdate>().AddAsync(progressUpdate, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken); // Save to get ProgressUpdateId
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
        
        (bool, string) imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<ProgressUpdateImage>(
            progressUpdate.ProgressUpdateId, 
            request.Files, 
            request.ImageUrls, 
            cancellationToken, 
            fileNames: request.FileNames,
            existingImagesForVersioning: existingImagesForBox
        );
        if (!imagesProcessResult.Item1)
        {
            return Result.Failure<ProgressUpdateDto>(imagesProcessResult.Item2);
        }
        
        try
        {
            await AddAuditLogAndWIRRecord(request, boxActivity, inferredStatus, currentUserId, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            // Handle WIR validation errors (location conflicts, previous WIR approval, etc.)
            return Result.Failure<ProgressUpdateDto>(ex.Message);
        }
        
        progressUpdate.BoxProgressSnapshot = await UpdateBoxProgress(request.BoxId, currentUserId, cancellationToken);
        _unitOfWork.Repository<ProgressUpdate>().Update(progressUpdate);

        if (boxActivity.Box != null)
            await UpdateProjectProgress(boxActivity.Box.ProjectId, currentUserId, cancellationToken);
        try
        {
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure<ProgressUpdateDto>($"Failed to save changes: {ex.Message}. Inner: {ex.InnerException?.Message}");
        }
        var dto = progressUpdate.Adapt<ProgressUpdateDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            UpdatedByName = user.FullName ?? user.Email,
        };

        return Result.Success(dto);
    }

    private async Task<(bool, string)> ImagesProcessing(Guid ProgressId, List<byte[]>? files, List<string>? imageUrls, CancellationToken cancellationToken)
    {
        int sequence = 0;
        var imagesToAdd = new List<ProgressUpdateImage>();

        if (files != null && files.Count > 0)
        {
            try
            {
                foreach (var fileBytes in files.Where(f => f != null && f.Length > 0))
                {
                    var base64 = Convert.ToBase64String(fileBytes);
                    var imageData = $"data:image/jpeg;base64,{base64}";

                    var image = new ProgressUpdateImage
                    {
                        ProgressUpdateId = ProgressId,
                        ImageData = imageData,
                        ImageType = "file",
                        FileSize = fileBytes.Length,
                        Sequence = sequence++,
                        CreatedDate = DateTime.UtcNow
                    };

                    imagesToAdd.Add(image);
                }
            }
            catch (Exception ex)
            {
                return (false, "Error processing uploaded files: " + ex.Message);
            }
        }

        if (imageUrls != null && imageUrls.Count > 0)
        {
            foreach (var url in imageUrls.Where(url => !string.IsNullOrWhiteSpace(url)))
            {
                try
                {
                    string imageType = "url";
                    string imageData;
                    long fileSize;
                    byte[]? imageBytes = null;

                    var trimmedUrl = url.Trim();

                    if (trimmedUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                    {
                        imageData = trimmedUrl;
                        imageType = "file";
                        var base64Index = trimmedUrl.IndexOf(',');
                        if (base64Index >= 0 && base64Index < trimmedUrl.Length - 1)
                        {
                            var base64Part = trimmedUrl.Substring(base64Index + 1);
                            imageBytes = Convert.FromBase64String(base64Part);
                            fileSize = imageBytes.Length;
                        }
                        else
                        {
                            fileSize = 0;
                        }
                    }
                    else
                    {
                        imageBytes = await _imageProcessingService.DownloadImageFromUrlAsync(trimmedUrl, cancellationToken);

                        if (imageBytes == null || imageBytes.Length == 0)
                        {
                            return (false, $"Failed to download image from URL: {trimmedUrl}");
                        }

                        var base64 = Convert.ToBase64String(imageBytes);
                        imageData = $"data:image/jpeg;base64,{base64}";
                        fileSize = imageBytes.Length;
                    }

                    var originalName = trimmedUrl;
                    if (trimmedUrl.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                    {
                        originalName = $"Captured Image {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";
                    }
                    else if (originalName.Length > 500)
                    {
                        originalName = originalName.Substring(0, 500);
                    }

                    var image = new ProgressUpdateImage
                    {
                        ProgressUpdateId = ProgressId,
                        ImageData = imageData,
                        ImageType = imageType,
                        OriginalName = originalName,
                        FileSize = fileSize,
                        Sequence = sequence++,
                        CreatedDate = DateTime.UtcNow
                    };

                    imagesToAdd.Add(image);
                }
                catch (Exception ex)
                {
                    return (false, $"Error processing image from URL '{url}': {ex.Message}");
                }

            }
        }
        if (imagesToAdd.Count > 0)
            await _unitOfWork.Repository<ProgressUpdateImage>().AddRangeAsync(imagesToAdd, cancellationToken);
        return (true, string.Empty);

    }
    private async Task<decimal> UpdateBoxProgress(Guid boxId, Guid currentUserId, CancellationToken cancellationToken)
    {
        var boxActivities = await _unitOfWork.Repository<BoxActivity>()
            .FindAsync(ba => ba.BoxId == boxId && ba.IsActive, cancellationToken);

        if (!boxActivities.Any())
            return 0;

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

    private async Task AddAuditLogAndWIRRecord(CreateProgressUpdateCommand request, BoxActivity boxActivity, BoxStatusEnum inferredStatus, Guid currentUserId, CancellationToken cancellationToken)
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

        if ((inferredStatus == BoxStatusEnum.InProgress || inferredStatus == BoxStatusEnum.Completed )&& boxActivity.ActualStartDate == null)
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

        await CreateWIRRecordOrUpdatePositionIfWIRExist(request, boxActivity, currentUserId,cancellationToken);
    }

    
    private async Task CreateWIRRecordOrUpdatePositionIfWIRExist(CreateProgressUpdateCommand request, BoxActivity currentActivity, Guid currentUserId, CancellationToken cancellationToken)
    {
        BoxActivity? nextWIRActivity = null;
        if (currentActivity.ActivityMaster.IsWIRCheckpoint)
        {
           nextWIRActivity=currentActivity;
        }

        else
        {
             nextWIRActivity = await _dbContext.BoxActivities
                 .Include(x => x.ActivityMaster)
                 .Where(x =>
                         x.BoxId == currentActivity.BoxId &&
                         x.Sequence > currentActivity.Sequence &&
                         x.ActivityMaster.IsWIRCheckpoint)
                 .OrderBy(x => x.Sequence)
                 .FirstOrDefaultAsync(cancellationToken);
        }

        // If no WIR activity found, return
        if (nextWIRActivity == null)
            return;

        // Validate that location (Bay/Row/Position) is unique in the same factory
        // Only validate if position data is being provided
        if (!string.IsNullOrWhiteSpace(request.WirBay) || !string.IsNullOrWhiteSpace(request.WirRow) || !string.IsNullOrWhiteSpace(request.WirPosition))
        {
            await ValidateUniqueLocationInFactory(
                request.WirBay ?? string.Empty, 
                request.WirRow ?? string.Empty, 
                request.WirPosition ?? string.Empty, 
                currentActivity.BoxId, 
                cancellationToken);

            // Validate that previous WIR (if any) is approved before updating position for the current WIR
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
            // WIR record doesn't exist yet - create it
            // Each WIR section should have its own position values
            // Only use values from the current request (no inheritance from previous WIRs)
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
            
            // If provided values exist, consider it as position updated
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
            
            // Check if WIR has position data (either newly set or already existing)
            // This ensures box location is always synchronized with WIR position
            positionUpdated = !string.IsNullOrWhiteSpace(nearestWIR.Bay) || 
                             !string.IsNullOrWhiteSpace(nearestWIR.Row) || 
                             !string.IsNullOrWhiteSpace(nearestWIR.Position);
            }
        
        if (positionUpdated)
        {
            // Update box location to match the LATEST WIR position
            // The box location should reflect the current WIR section
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(currentActivity.BoxId, cancellationToken);
            if (box != null)
            {
                // Find the most recent WIR with position data for this box
                // This ensures box location reflects the current/latest WIR section
                var latestWIRWithPosition = await _dbContext.WIRRecords
                    .Include(w => w.BoxActivity)
                    .Where(w => w.BoxActivity.BoxId == currentActivity.BoxId)
                    .Where(w => !string.IsNullOrWhiteSpace(w.Bay) || !string.IsNullOrWhiteSpace(w.Row) || !string.IsNullOrWhiteSpace(w.Position))
                    .OrderByDescending(w => w.CreatedDate)
                    .FirstOrDefaultAsync(cancellationToken);

                if (latestWIRWithPosition != null)
                {
                    // Check if box location actually needs updating (avoid unnecessary updates)
                    bool boxNeedsUpdate = box.Bay != latestWIRWithPosition.Bay || 
                                         box.Row != latestWIRWithPosition.Row || 
                                         box.Position != latestWIRWithPosition.Position;
                    
                    if (boxNeedsUpdate)
                    {
                        // Validate that this location is not already occupied by another box in the same factory
                        await ValidateUniqueLocationInFactory(
                            latestWIRWithPosition.Bay ?? string.Empty,
                            latestWIRWithPosition.Row ?? string.Empty,
                            latestWIRWithPosition.Position ?? string.Empty,
                            currentActivity.BoxId,
                            cancellationToken);
                        
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
    }
    private bool UpdateWIRPosition(WIRRecord nearestWIR, CreateProgressUpdateCommand request)
    {
        bool positionUpdated = false;

        if (string.IsNullOrWhiteSpace(request.WirBay) &&
           string.IsNullOrWhiteSpace(request.WirRow) &&
           string.IsNullOrWhiteSpace(request.WirPosition))
        {
            return positionUpdated; // No position data to update
        }

        // Update WIR position fields only if they are currently empty
        // Once set, position values should not be overwritten
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

    /// <summary>
    /// Validates that the previous WIR (if any) is fully approved before allowing position updates
    /// 
    /// CRITICAL: A WIR is considered "fully approved" ONLY when:
    /// 1. Status is Approved or ConditionallyApproved, AND
    /// 2. All checklist items have "Pass" status (not Under Review)
    /// 
    /// "Under Review" state: Status = Approved but NOT all checklist items are Pass
    /// </summary>
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

        // Load the WIR checkpoint to get the effective status
        // The checkpoint status is the source of truth (matches frontend logic)
        var previousCheckpoint = await _dbContext.WIRCheckpoints
            .Include(cp => cp.ChecklistItems)
            .Where(cp => cp.BoxId == boxId && cp.WIRCode == previousWIR.WIRCode)
            .FirstOrDefaultAsync(cancellationToken);

        // Use checkpoint status if available, otherwise fall back to WIR record status
        // Cast WIRRecordStatusEnum to WIRCheckpointStatusEnum (both have same values)
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

        // If we reach here, previous WIR is fully approved - validation passes
    }

    /// <summary>
    /// Validates that the Bay/Row/Position combination is unique within the same factory
    /// Prevents two boxes from occupying the same location in a factory
    /// </summary>
    private async Task ValidateUniqueLocationInFactory(string bayValue, string rowValue, string positionValue, Guid currentBoxId, CancellationToken cancellationToken)
    {
        // Only validate if position data is being provided
        if (string.IsNullOrWhiteSpace(bayValue) && 
            string.IsNullOrWhiteSpace(rowValue) && 
            string.IsNullOrWhiteSpace(positionValue))
        {
            return;
        }

        // Get the current box to find its factory
        var currentBox = await _unitOfWork.Repository<Box>().GetByIdAsync(currentBoxId, cancellationToken);
        if (currentBox == null || !currentBox.FactoryId.HasValue)
        {
            return; // No factory assigned, skip validation
        }

        bayValue = bayValue?.Trim() ?? string.Empty;
        rowValue = rowValue?.Trim() ?? string.Empty;
        positionValue = positionValue?.Trim() ?? string.Empty;

        // Check if another box in the same factory has the same Bay/Row/Position combination
        var conflictingBox = await _dbContext.Boxes
            .Where(b => b.FactoryId == currentBox.FactoryId &&
                       b.BoxId != currentBoxId && // Exclude current box
                       b.Bay == bayValue &&
                       b.Row == rowValue &&
                       b.Position == positionValue &&
                       !string.IsNullOrWhiteSpace(b.Bay) && // Ensure not comparing empty strings
                       !string.IsNullOrWhiteSpace(b.Row))
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingBox != null)
        {
            throw new InvalidOperationException(
                $"Location conflict: Bay '{bayValue}', Row '{rowValue}', Position '{positionValue}' " +
                $"is already occupied by Box '{conflictingBox.BoxTag}' in this factory. " +
                $"Please select a different location.");
        }
    }
}