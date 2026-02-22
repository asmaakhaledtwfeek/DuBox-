using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Commands;

public class CreateScheduleActivitiesFromTemplateCommandHandler : IRequestHandler<CreateScheduleActivitiesFromTemplateCommand, Result<List<Guid>>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateScheduleActivitiesFromTemplateCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<Guid>>> Handle(CreateScheduleActivitiesFromTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.ActivityTemplates
            .Include(t => t.TemplateActivities)
            .FirstOrDefaultAsync(t => t.ActivityTemplateId == request.ActivityTemplateId, cancellationToken);

        if (template == null)
        {
            return Result.Failure<List<Guid>>(new Error("ActivityTemplate.NotFound", "Activity template not found"));
        }

        if (!template.IsActive)
        {
            return Result.Failure<List<Guid>>(new Error("ActivityTemplate.Inactive", "Activity template is not active"));
        }

        var createdActivityIds = new List<Guid>();
        var currentStartDate = request.BaseStartDate;

        // Create schedule activities from template activities (ordered by sequence)
        foreach (var templateActivity in template.TemplateActivities.OrderBy(a => a.OverallSequence))
        {
            var plannedFinishDate = currentStartDate.AddDays(templateActivity.EstimatedDurationDays);

            var scheduleActivity = new ScheduleActivity
            {
                // Clone all properties from template activity (which are already snapshots)
                ActivityCode = templateActivity.ActivityCode,
                ActivityName = templateActivity.ActivityName,
                Stage = templateActivity.Stage,
                StageNumber = templateActivity.StageNumber,
                SequenceInStage = templateActivity.SequenceInStage,
                OverallSequence = templateActivity.OverallSequence,
                Description = templateActivity.Description,
                EstimatedDurationDays = templateActivity.EstimatedDurationDays,
                IsWIRCheckpoint = templateActivity.IsWIRCheckpoint,
                WIRCode = templateActivity.WIRCode,
                ApplicableBoxTypes = templateActivity.ApplicableBoxTypes,
                DependsOnActivities = templateActivity.DependsOnActivities,
                
                // Schedule-specific fields
                PlannedStartDate = currentStartDate,
                PlannedFinishDate = plannedFinishDate,
                ProjectId = request.ProjectId,
                Status = "Planned",
                PercentComplete = 0,
                CreatedBy = _currentUserService.UserId,
                CreatedDate = DateTime.UtcNow
            };

            _context.ScheduleActivities.Add(scheduleActivity);
            createdActivityIds.Add(scheduleActivity.ScheduleActivityId);

            // Move to next start date (sequential activities)
            currentStartDate = plannedFinishDate.AddDays(1);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(createdActivityIds);
    }
}
