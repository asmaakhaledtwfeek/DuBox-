using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Queries;

public class GetScheduleActivitiesQueryHandler : IRequestHandler<GetScheduleActivitiesQuery, Result<List<ScheduleActivityListDto>>>
{
    private readonly IDbContext _context;

    public GetScheduleActivitiesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ScheduleActivityListDto>>> Handle(GetScheduleActivitiesQuery request, CancellationToken cancellationToken)
    {
        var activities = await _context.ScheduleActivities
            .Include(a => a.AssignedTeams)
            .Include(a => a.AssignedMaterials)
            .OrderByDescending(a => a.CreatedDate)
            .Select(a => new ScheduleActivityListDto(
                a.ScheduleActivityId,
                a.ActivityName,
                a.ActivityCode,
                a.PlannedStartDate,
                a.PlannedFinishDate,
                a.Status,
                a.PercentComplete,
                a.AssignedTeams.Count,
                a.AssignedMaterials.Count
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(activities);
    }
}



