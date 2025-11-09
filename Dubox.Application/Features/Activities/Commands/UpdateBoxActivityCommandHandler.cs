using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Activities.Commands;

public class UpdateBoxActivityCommandHandler : IRequestHandler<UpdateBoxActivityCommand, Result<BoxActivityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;

    public UpdateBoxActivityCommandHandler(IUnitOfWork unitOfWork, IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<BoxActivityDto>> Handle(UpdateBoxActivityCommand request, CancellationToken cancellationToken)
    {
        var boxActivity = await _dbContext.BoxActivities
            .Include(ba => ba.ActivityMaster)
            .Include(ba => ba.Box)
            .FirstOrDefaultAsync(ba => ba.BoxActivityId == request.BoxActivityId, cancellationToken);

        if (boxActivity == null)
            return Result.Failure<BoxActivityDto>("Box activity not found");

        // Track if status changed to trigger auto-actions
        var statusChanged = boxActivity.Status != request.Status;
        var wasCompleted = boxActivity.Status == BoxStatusEnum.Completed;
        var nowCompleted = request.Status == BoxStatusEnum.Completed;

        boxActivity.Status = request.Status;
        boxActivity.ProgressPercentage = request.ProgressPercentage;
        boxActivity.WorkDescription = request.WorkDescription;
        boxActivity.IssuesEncountered = request.IssuesEncountered;
        boxActivity.TeamId = request.AssignedTeam;
        boxActivity.MaterialsAvailable = request.MaterialsAvailable;

        // Update dates based on status
        if (request.Status == BoxStatusEnum.InProgress && boxActivity.ActualStartDate == null)
        {
            boxActivity.ActualStartDate = DateTime.UtcNow;
        }

        if (request.Status == BoxStatusEnum.Completed && !wasCompleted)
        {
            boxActivity.ActualEndDate = DateTime.UtcNow;
            boxActivity.ProgressPercentage = 100;
        }

        boxActivity.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.Repository<BoxActivity>().Update(boxActivity);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Update box overall progress
        await UpdateBoxProgress(boxActivity.BoxId, cancellationToken);

        // Check if this is a WIR checkpoint and completed
        if (nowCompleted && boxActivity.ActivityMaster.IsWIRCheckpoint)
        {
            // Auto-create WIR record if not exists
            var wirExists = await _dbContext.WIRRecords
                .AnyAsync(w => w.BoxActivityId == boxActivity.BoxActivityId, cancellationToken);

            if (!wirExists)
            {
                // WIR should be created manually or through separate endpoint
                // This is just a placeholder for the logic
            }
        }

        var dto = boxActivity.Adapt<BoxActivityDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityCode = boxActivity.ActivityMaster.ActivityCode,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            Stage = boxActivity.ActivityMaster.Stage,
            IsWIRCheckpoint = boxActivity.ActivityMaster.IsWIRCheckpoint,
            WIRCode = boxActivity.ActivityMaster.WIRCode
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
            _unitOfWork.Repository<Box>().Update(box);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}

