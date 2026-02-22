using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Commands;

public class CreateScheduleActivityFromMasterCommandHandler : IRequestHandler<CreateScheduleActivityFromMasterCommand, Result<Guid>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateScheduleActivityFromMasterCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateScheduleActivityFromMasterCommand request, CancellationToken cancellationToken)
    {
        var activityMaster = await _context.ActivityMasters
            .FirstOrDefaultAsync(am => am.ActivityMasterId == request.ActivityMasterId, cancellationToken);

        if (activityMaster == null)
        {
            return Result.Failure<Guid>(new Error("ActivityMaster.NotFound", "Activity master not found"));
        }

        // Clone all important properties from ActivityMaster (snapshot)
        var scheduleActivity = new ScheduleActivity
        {
            SourceActivityMasterId = activityMaster.ActivityMasterId,
            IsCustomActivity = false,
            
            // Cloned properties from ActivityMaster
            ActivityCode = activityMaster.ActivityCode,
            ActivityName = activityMaster.ActivityName,
            Stage = activityMaster.Stage,
            StageNumber = activityMaster.StageNumber,
            SequenceInStage = activityMaster.SequenceInStage,
            OverallSequence = activityMaster.OverallSequence,
            Description = activityMaster.Description,
            EstimatedDurationDays = activityMaster.EstimatedDurationDays,
            IsWIRCheckpoint = activityMaster.IsWIRCheckpoint,
            WIRCode = activityMaster.WIRCode,
            ApplicableBoxTypes = activityMaster.ApplicableBoxTypes,
            DependsOnActivities = activityMaster.DependsOnActivities,
            
            // Schedule-specific fields
            PlannedStartDate = request.PlannedStartDate,
            PlannedFinishDate = request.PlannedFinishDate,
            ProjectId = request.ProjectId,
            Status = "Planned",
            PercentComplete = 0,
            CreatedBy = _currentUserService.UserId,
            CreatedDate = DateTime.UtcNow
        };

        _context.ScheduleActivities.Add(scheduleActivity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(scheduleActivity.ScheduleActivityId);
    }
}
