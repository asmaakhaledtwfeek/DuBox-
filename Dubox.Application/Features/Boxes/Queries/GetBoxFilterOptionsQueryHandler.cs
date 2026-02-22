using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxFilterOptionsQueryHandler : IRequestHandler<GetBoxFilterOptionsQuery, Result<BoxFilterOptionsDto>>
{
    private readonly IDbContext _dbContext;

    public GetBoxFilterOptionsQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<BoxFilterOptionsDto>> Handle(GetBoxFilterOptionsQuery request, CancellationToken cancellationToken)
    {
        // Verify project exists
        var projectExists = await _dbContext.Projects
            .AnyAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

        if (!projectExists)
        {
            return Result.Failure<BoxFilterOptionsDto>("Project not found");
        }

        var activeBoxes = _dbContext.Boxes
            .Where(b => b.ProjectId == request.ProjectId && b.IsActive);

        // Get SubTypes counts (extract from BoxTag format: ProjectNumber-Building-Floor-Type-SubType)
        var subTypeCounts =  activeBoxes
            .AsEnumerable() // Move to client-side for string manipulation
            .Where(b => !string.IsNullOrEmpty(b.BoxTag))
            .Select(b => {
                var parts = b.BoxTag.Split('-');
                return parts.Length >= 5 ? parts[4] : null;
            })
            .Where(subType => !string.IsNullOrEmpty(subType))
            .GroupBy(subType => subType!)
            .Select(g => new FilterOptionDto
            {
                Value = g.Key,
                Count = g.Count()
            })
            .OrderBy(f => f.Value)
            .ToList();

        // Get Buildings counts
        var buildingCounts = await activeBoxes
            .Where(b => !string.IsNullOrEmpty(b.BuildingNumber))
            .GroupBy(b => b.BuildingNumber!)
            .Select(g => new FilterOptionDto
            {
                Value = g.Key,
                Count = g.Count()
            })
            .OrderBy(f => f.Value)
            .ToListAsync(cancellationToken);

        // Get Floors counts
        var floorCounts = await activeBoxes
            .Where(b => !string.IsNullOrEmpty(b.Floor))
            .GroupBy(b => b.Floor!)
            .Select(g => new FilterOptionDto
            {
                Value = g.Key,
                Count = g.Count()
            })
            .OrderBy(f => f.Value)
            .ToListAsync(cancellationToken);

        // Get Zones counts
        var zoneCounts = await activeBoxes
            .Where(b => !string.IsNullOrEmpty(b.Zone))
            .GroupBy(b => b.Zone!)
            .Select(g => new FilterOptionDto
            {
                Value = g.Key,
                Count = g.Count()
            })
            .OrderBy(f => f.Value)
            .ToListAsync(cancellationToken);

        var result = new BoxFilterOptionsDto
        {
            ProjectId = request.ProjectId,
            SubTypes = subTypeCounts,
            Buildings = buildingCounts,
            Floors = floorCounts,
            Zones = zoneCounts
        };

        return Result.Success(result);
    }
}
