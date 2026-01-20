using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Commands;

public class AssignTeamCommandHandler : IRequestHandler<AssignTeamCommand, Result<Guid>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AssignTeamCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(AssignTeamCommand request, CancellationToken cancellationToken)
    {
        // Check if activity exists
        var activityExists = await _context.ScheduleActivities
            .AnyAsync(a => a.ScheduleActivityId == request.ScheduleActivityId, cancellationToken);

        if (!activityExists)
        {
            return Result.Failure<Guid>(new Error("ScheduleActivity.NotFound", "Schedule activity not found"));
        }

        // Check if team exists
        var teamExists = await _context.Teams
            .AnyAsync(t => t.TeamId == request.TeamId, cancellationToken);

        if (!teamExists)
        {
            return Result.Failure<Guid>(new Error("Team.NotFound", "Team not found"));
        }

        // Check if already assigned
        var alreadyAssigned = await _context.ScheduleActivityTeams
            .AnyAsync(sat => sat.ScheduleActivityId == request.ScheduleActivityId && sat.TeamId == request.TeamId, cancellationToken);

        if (alreadyAssigned)
        {
            return Result.Failure<Guid>(new Error("ScheduleActivity.TeamAlreadyAssigned", "Team is already assigned to this activity"));
        }

        var assignment = new ScheduleActivityTeam
        {
            ScheduleActivityId = request.ScheduleActivityId,
            TeamId = request.TeamId,
            AssignedDate = DateTime.UtcNow,
            Notes = request.Notes,
            CreatedBy = _currentUserService.UserId,
            CreatedDate = DateTime.UtcNow
        };

        _context.ScheduleActivityTeams.Add(assignment);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(assignment.ScheduleActivityTeamId);
    }
}




