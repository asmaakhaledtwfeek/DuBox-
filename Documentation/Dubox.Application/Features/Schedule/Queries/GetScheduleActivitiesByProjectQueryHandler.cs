using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Queries;

public class GetScheduleActivitiesByProjectQueryHandler : IRequestHandler<GetScheduleActivitiesByProjectQuery, Result<List<ScheduleActivityListDto>>>
{
    private readonly IDbContext _context;

    public GetScheduleActivitiesByProjectQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ScheduleActivityListDto>>> Handle(GetScheduleActivitiesByProjectQuery request, CancellationToken cancellationToken)
    {
        var activities = await _context.ScheduleActivities
            .Where(a => a.ProjectId == request.ProjectId)
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
