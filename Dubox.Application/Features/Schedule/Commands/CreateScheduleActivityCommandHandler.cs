using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Commands;

public class CreateScheduleActivityCommandHandler : IRequestHandler<CreateScheduleActivityCommand, Result<Guid>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateScheduleActivityCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateScheduleActivityCommand request, CancellationToken cancellationToken)
    {
        // Check if activity code already exists
        var exists = await _context.ScheduleActivities
            .AnyAsync(a => a.ActivityCode == request.ActivityCode, cancellationToken);

        if (exists)
        {
            return Result.Failure<Guid>(new Error("ScheduleActivity.DuplicateCode", "Activity code already exists"));
        }

        var activity = new ScheduleActivity
        {
            ActivityName = request.ActivityName,
            ActivityCode = request.ActivityCode,
            Description = request.Description,
            PlannedStartDate = request.PlannedStartDate,
            PlannedFinishDate = request.PlannedFinishDate,
            ProjectId = request.ProjectId,
            Status = "Planned",
            PercentComplete = 0,
            CreatedBy = _currentUserService.UserId,
            CreatedDate = DateTime.UtcNow
        };

        _context.ScheduleActivities.Add(activity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(activity.ScheduleActivityId);
    }
}




