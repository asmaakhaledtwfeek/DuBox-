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
        // Note: Progress updates don't require specific role checks beyond authentication
        // Design Engineers and other roles can create progress updates for activities they have access to
        // Access is controlled via project visibility rules in the visibility service

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

        if (!string.IsNullOrEmpty(progressUpdate.DeviceInfo) && progressUpdate.DeviceInfo.Length > 100)
        {
            progressUpdate.DeviceInfo = progressUpdate.DeviceInfo.Substring(0, 100);
        }

        await _unitOfWork.Repository<ProgressUpdate>().AddAsync(progressUpdate, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken); // Save to get ProgressUpdateId

        int sequence = 0;
        var imagesToAdd = new List<ProgressUpdateImage>();

        // Process uploaded files - convert to base64
        if (request.Files != null && request.Files.Count > 0)
        {
            try
            {
                foreach (var fileBytes in request.Files.Where(f => f != null && f.Length > 0))
                {
                    var base64 = Convert.ToBase64String(fileBytes);
                    var imageData = $"data:image/jpeg;base64,{base64}";

                    var image = new ProgressUpdateImage
                    {
                        ProgressUpdateId = progressUpdate.ProgressUpdateId,
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
                return Result.Failure<ProgressUpdateDto>("Error processing uploaded files: " + ex.Message);
            }
        }

        // Process image URLs - download and convert to base64
        if (request.ImageUrls != null && request.ImageUrls.Count > 0)
        {
            foreach (var url in request.ImageUrls.Where(url => !string.IsNullOrWhiteSpace(url)))
            {
                try
                {
                    string imageData;
                    long fileSize;
                    byte[]? imageBytes = null;

                    var trimmedUrl = url.Trim();

                    if (trimmedUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                    {
                        imageData = trimmedUrl;

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
                            return Result.Failure<ProgressUpdateDto>($"Failed to download image from URL: {trimmedUrl}");
                        }

                        var base64 = Convert.ToBase64String(imageBytes);
                        imageData = $"data:image/jpeg;base64,{base64}";
                        fileSize = imageBytes.Length;
                    }

                    var image = new ProgressUpdateImage
                    {
                        ProgressUpdateId = progressUpdate.ProgressUpdateId,
                        ImageData = imageData,
                        ImageType = "url",
                        OriginalName = trimmedUrl,
                        FileSize = fileSize,
                        Sequence = sequence++,
                        CreatedDate = DateTime.UtcNow
                    };

                    imagesToAdd.Add(image);
                }
                catch (Exception ex)
                {
                    return Result.Failure<ProgressUpdateDto>($"Error processing image from URL '{url}': {ex.Message}");
                }
            }
        }

        if (imagesToAdd.Count > 0)
        {
            foreach (var image in imagesToAdd)
            {
                await _unitOfWork.Repository<ProgressUpdateImage>().AddAsync(image, cancellationToken);
            }
        }

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

        progressUpdate.BoxProgressSnapshot = await UpdateBoxProgress(request.BoxId, currentUserId, cancellationToken);
        _unitOfWork.Repository<ProgressUpdate>().Update(progressUpdate);
        if (boxActivity.Box != null)
            await UpdateProjectProgress(boxActivity.Box.ProjectId, currentUserId, cancellationToken);

        if (inferredStatus == BoxStatusEnum.Completed && boxActivity.ActivityMaster.IsWIRCheckpoint)
        {
            var wirExists = await _dbContext.WIRRecords
                .AnyAsync(w => w.BoxActivityId == request.BoxActivityId, cancellationToken);

            if (!wirExists)
            {
                // Get image data from ProgressUpdateImage entities for WIR record
                var imageDataList = imagesToAdd.Select(img => img.ImageData).ToList();
                var photoJson = imageDataList.Count > 0
                    ? System.Text.Json.JsonSerializer.Serialize(imageDataList)
                    : null;

                var wirRecord = new WIRRecord
                {
                    BoxActivityId = request.BoxActivityId,
                    WIRCode = boxActivity.ActivityMaster.WIRCode ?? "WIR",
                    Status = WIRRecordStatusEnum.Pending,
                    RequestedDate = DateTime.UtcNow,
                    RequestedBy = currentUserId,
                    Photo = photoJson, // JSON array of image data from ProgressUpdateImage entities
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<WIRRecord>().AddAsync(wirRecord, cancellationToken);
            }
        }

        // Complete all changes (ProgressUpdate, Images, BoxActivity, AuditLog, Box, WIRRecord)
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Reload ProgressUpdate with Images to include them in DTO
        var images = await _dbContext.ProgressUpdateImages
            .Where(img => img.ProgressUpdateId == progressUpdate.ProgressUpdateId)
            .OrderBy(img => img.Sequence)
            .ToListAsync(cancellationToken);

        var dto = progressUpdate.Adapt<ProgressUpdateDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            UpdatedByName = user.FullName ?? user.Email,
            Photo = null, // No longer storing images in Photo field
            Images = images.Select(img => new ProgressUpdateImageDto
            {
                ProgressUpdateImageId = img.ProgressUpdateImageId,
                ProgressUpdateId = img.ProgressUpdateId,
                ImageData = img.ImageData,
                ImageType = img.ImageType,
                OriginalName = img.OriginalName,
                FileSize = img.FileSize,
                Sequence = img.Sequence,
                CreatedDate = img.CreatedDate
            }).ToList()
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
}