using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Dubox.Domain.Enums ;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBuildingFloorBreakdownQueryHandler : IRequestHandler<GetBuildingFloorBreakdownQuery, Result<BuildingFloorBreakdownDto>>
{
    private readonly IDbContext _dbContext;

    public GetBuildingFloorBreakdownQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<BuildingFloorBreakdownDto>> Handle(GetBuildingFloorBreakdownQuery request, CancellationToken cancellationToken)
    {
        // Verify project exists
        var projectExists = await _dbContext.Projects
            .AnyAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

        if (!projectExists)
        {
            return Result.Failure<BuildingFloorBreakdownDto>("Project not found");
        }

        // Get building and floor counts with status breakdown using efficient grouping query
        var buildingFloorCounts = await _dbContext.Boxes
            .Where(b => b.ProjectId == request.ProjectId && b.IsActive)
            .GroupBy(b => new { 
                Building = b.BuildingNumber ?? "No Building",
                Floor = b.Floor ?? "No Floor"
            })
            .Select(g => new {
                g.Key.Building,
                g.Key.Floor,
                Count = g.Count(),
                InProgressCount = g.Count(b => b.Status == BoxStatusEnum.InProgress),
                CompletedCount = g.Count(b => b.Status == BoxStatusEnum.Completed)
            })
            .ToListAsync(cancellationToken);

        // Group by building and create the breakdown structure
        var buildings = buildingFloorCounts
            .GroupBy(x => x.Building)
            .Select(buildingGroup => new BuildingBreakdownDto
            {
                Building = buildingGroup.Key,
                TotalBoxes = buildingGroup.Sum(f => f.Count),
                InProgressCount = buildingGroup.Sum(f => f.InProgressCount),
                CompletedCount = buildingGroup.Sum(f => f.CompletedCount),
                Floors = buildingGroup
                    .Select(f => new FloorBreakdownDto
                    {
                        Floor = f.Floor,
                        BoxCount = f.Count,
                        InProgressCount = f.InProgressCount,
                        CompletedCount = f.CompletedCount
                    })
                    .OrderBy(f => f.Floor)
                    .ToList()
            })
            .OrderBy(b => b.Building)
            .ToList();

        var result = new BuildingFloorBreakdownDto
        {
            ProjectId = request.ProjectId,
            Buildings = buildings
        };

        return Result.Success(result);
    }
}
