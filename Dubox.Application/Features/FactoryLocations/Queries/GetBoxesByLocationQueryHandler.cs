using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public class GetBoxesByLocationQueryHandler : IRequestHandler<GetBoxesByLocationQuery, Result<LocationBoxesDto>>
{
    private readonly IDbContext _dbContext;

    public GetBoxesByLocationQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<LocationBoxesDto>> Handle(GetBoxesByLocationQuery request, CancellationToken cancellationToken)
    {
        var location = await _dbContext.FactoryLocations
            .FirstOrDefaultAsync(l => l.LocationId == request.LocationId, cancellationToken);

        if (location == null)
            return Result.Failure<LocationBoxesDto>("Location not found");

        var boxes = await _dbContext.Boxes
            .Include(b => b.Project)
            .Include(b => b.CurrentLocation)
            .Where(b => b.CurrentLocationId == request.LocationId)
            .ToListAsync(cancellationToken);

        var boxDtos = boxes.Select(b => b.Adapt<BoxDto>() with
        {
            ProjectCode = b.Project.ProjectCode,
            Status = b.Status.ToString(),
            CurrentLocationId = b.CurrentLocationId,
            CurrentLocationCode = b.CurrentLocation?.LocationCode,
            CurrentLocationName = b.CurrentLocation?.LocationName
        }).ToList();

        // Calculate status counts
        var statusCounts = boxes
            .GroupBy(b => b.Status)
            .Select(g => new LocationBoxStatusCountDto
            {
                Status = g.Key.ToString(),
                Count = g.Count()
            })
            .ToList();

        var result = new LocationBoxesDto
        {
            LocationId = location.LocationId,
            LocationCode = location.LocationCode,
            LocationName = location.LocationName,
            Boxes = boxDtos,
            StatusCounts = statusCounts,
            TotalBoxes = boxes.Count
        };

        return Result.Success(result);
    }
}

