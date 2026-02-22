using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
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
        // Load ALL activities (all levels at once)
        var allActivities = await _context.ScheduleActivities
            .Include(a => a.AssignedTeams)
            .Include(a => a.AssignedMaterials)
            .OrderBy(a => a.ActivityCode)
            .ToListAsync(cancellationToken);

        if (!allActivities.Any())
        {
            return Result.Success(new List<ScheduleActivityListDto>());
        }

        Console.WriteLine($"[ScheduleQuery] Loaded {allActivities.Count} total activities");

        // Build the complete hierarchy tree using shared builder
        var hierarchy = ScheduleActivityHierarchyBuilder.BuildHierarchy(allActivities);

        Console.WriteLine($"[ScheduleQuery] Built hierarchy with {hierarchy.Count} root activities");

        return Result.Success(hierarchy);
    }
}















