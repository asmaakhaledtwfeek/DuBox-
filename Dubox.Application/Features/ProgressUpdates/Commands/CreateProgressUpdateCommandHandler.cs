using Dubox.Application.DTOs;
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
        // Verify box and activity exist
        var boxActivity = await _dbContext.BoxActivities
            .Include(ba => ba.ActivityMaster)
            .Include(ba => ba.Box)
                .ThenInclude(b => b.Project)
            .FirstOrDefaultAsync(ba => ba.BoxActivityId == request.BoxActivityId && ba.BoxId == request.BoxId, cancellationToken);

        if (boxActivity == null)
            return Result.Failure<ProgressUpdateDto>("Box activity not found");

        // Get current user
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);

        if (user == null)
            return Result.Failure<ProgressUpdateDto>("User not found");

        // Create progress update record (audit trail)
        var progressUpdate = new ProgressUpdate
        {
            BoxId = request.BoxId,
            BoxActivityId = request.BoxActivityId,
            UpdateDate = DateTime.UtcNow,
            UpdatedBy = currentUserId,
            ProgressPercentage = request.ProgressPercentage,
            Status = request.Status,
            WorkDescription = request.WorkDescription,
            IssuesEncountered = request.IssuesEncountered,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            LocationDescription = request.LocationDescription,
            PhotoUrls = request.PhotoUrls != null ? string.Join(",", request.PhotoUrls) : null,
            UpdateMethod = request.UpdateMethod,
            DeviceInfo = request.DeviceInfo,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<ProgressUpdate>().AddAsync(progressUpdate, cancellationToken);

        // Update the BoxActivity record
        var oldStatus = boxActivity.Status;
        var oldProgress = boxActivity.ProgressPercentage;

        boxActivity.ProgressPercentage = request.ProgressPercentage;
        boxActivity.Status = request.Status;
        boxActivity.WorkDescription = request.WorkDescription;
        boxActivity.IssuesEncountered = request.IssuesEncountered;
        boxActivity.ModifiedDate = DateTime.UtcNow;
        boxActivity.ModifiedBy = _currentUserService.Username;

        // Update dates based on status
        if (request.Status == BoxStatusEnum.InProgress && boxActivity.ActualStartDate == null)
        {
            boxActivity.ActualStartDate = DateTime.UtcNow;
        }

        if (request.Status == BoxStatusEnum.Completed && oldStatus != BoxStatusEnum.Completed)
        {
            boxActivity.ActualEndDate = DateTime.UtcNow;
            boxActivity.ProgressPercentage = 100;
        }

        _unitOfWork.Repository<BoxActivity>().Update(boxActivity);

        // Update Box overall progress
        await UpdateBoxProgress(request.BoxId, cancellationToken);

        // Check if WIR checkpoint should be created
        if (request.Status == BoxStatusEnum.Completed && boxActivity.ActivityMaster.IsWIRCheckpoint)
        {
            var wirExists = await _dbContext.WIRRecords
                .AnyAsync(w => w.BoxActivityId == request.BoxActivityId, cancellationToken);

            if (!wirExists)
            {
                var wirRecord = new WIRRecord
                {
                    BoxActivityId = request.BoxActivityId,
                    WIRCode = boxActivity.ActivityMaster.WIRCode ?? "WIR",
                    Status = "Pending",
                    RequestedDate = DateTime.UtcNow,
                    RequestedBy = currentUserId,
                    PhotoUrls = progressUpdate.PhotoUrls,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<WIRRecord>().AddAsync(wirRecord, cancellationToken);
            }
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Prepare response DTO
        var dto = progressUpdate.Adapt<ProgressUpdateDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            UpdatedByName = user.FullName ?? user.Email
        };

        return Result.Success(dto);
    }

    private async Task UpdateBoxProgress(Guid boxId, CancellationToken cancellationToken)
    {
        var boxActivities = await _dbContext.BoxActivities
            .Where(ba => ba.BoxId == boxId && ba.IsActive)
            .ToListAsync(cancellationToken);

        if (!boxActivities.Any())
            return;

        var averageProgress = boxActivities.Average(ba => ba.ProgressPercentage);
        var allCompleted = boxActivities.All(ba => ba.Status == BoxStatusEnum.Completed);

        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxId, cancellationToken);
        if (box != null)
        {
            box.ProgressPercentage = averageProgress;

            if (allCompleted)
            {
                box.Status = BoxStatusEnum.Completed;
                box.ActualEndDate = DateTime.UtcNow;
            }
            else if (averageProgress > 0)
            {
                box.Status = BoxStatusEnum.InProgress;
                if (box.ActualStartDate == null)
                    box.ActualStartDate = DateTime.UtcNow;
            }

            box.ModifiedDate = DateTime.UtcNow;
            box.ModifiedBy = _currentUserService.Username;
            _unitOfWork.Repository<Box>().Update(box);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}

