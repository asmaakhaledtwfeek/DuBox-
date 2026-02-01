using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Queries;

public class GetScheduleActivityDetailsQueryHandler : IRequestHandler<GetScheduleActivityDetailsQuery, Result<ScheduleActivityDto>>
{
    private readonly IDbContext _context;

    public GetScheduleActivityDetailsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ScheduleActivityDto>> Handle(GetScheduleActivityDetailsQuery request, CancellationToken cancellationToken)
    {
        var activity = await _context.ScheduleActivities
            .Include(a => a.Project)
            .Include(a => a.AssignedTeams)
                .ThenInclude(at => at.Team)
            .Include(a => a.AssignedMaterials)
            .FirstOrDefaultAsync(a => a.ScheduleActivityId == request.ScheduleActivityId, cancellationToken);

        if (activity == null)
        {
            return Result.Failure<ScheduleActivityDto>(new Error("ScheduleActivity.NotFound", "Schedule activity not found"));
        }

        var dto = new ScheduleActivityDto(
            activity.ScheduleActivityId,
            activity.ActivityName,
            activity.ActivityCode,
            activity.Description,
            activity.PlannedStartDate,
            activity.PlannedFinishDate,
            activity.ActualStartDate,
            activity.ActualFinishDate,
            activity.Status,
            activity.PercentComplete,
            activity.ProjectId,
            activity.Project?.ProjectName,
            activity.AssignedTeams.Select(at => new AssignedTeamDto(
                at.ScheduleActivityTeamId,
                at.TeamId,
                at.Team.TeamName,
                at.AssignedDate,
                at.Notes
            )).ToList(),
            activity.AssignedMaterials.Select(am => new AssignedMaterialDto(
                am.ScheduleActivityMaterialId,
                am.MaterialName,
                am.MaterialCode,
                am.Quantity,
                am.Unit,
                am.Notes
            )).ToList()
        );

        return Result.Success(dto);
    }
}

