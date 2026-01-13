using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public class GetBoxLocationHistoryQueryHandler : IRequestHandler<GetBoxLocationHistoryQuery, Result<List<BoxLocationHistoryDto>>>
{
    private readonly IDbContext _dbContext;

    public GetBoxLocationHistoryQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<BoxLocationHistoryDto>>> Handle(GetBoxLocationHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _dbContext.BoxLocationHistory
            .Include(h => h.Box)
            .Include(h => h.Location)
            .Include(h => h.MovedFromLocation)
            .Include(h => h.MovedByUser)
            .Where(h => h.BoxId == request.BoxId)
            .OrderByDescending(h => h.MovedDate)
            .ToListAsync(cancellationToken);

        var dtos = history.Select(h => new BoxLocationHistoryDto
        {
            HistoryId = h.HistoryId,
            BoxId = h.BoxId,
            BoxTag = h.Box.BoxTag,
            SerialNumber = h.Box.SerialNumber,
            LocationId = h.LocationId,
            LocationCode = h.Location.LocationCode,
            LocationName = h.Location.LocationName,
            MovedFromLocationId = h.MovedFromLocationId,
            MovedFromLocationCode = h.MovedFromLocation?.LocationCode,
            MovedFromLocationName = h.MovedFromLocation?.LocationName,
            MovedDate = h.MovedDate,
            Reason = h.Reason,
            MovedBy = h.MovedBy,
            MovedByUsername = h.MovedByUser?.Email,
            MovedByFullName = h.MovedByUser?.FullName
        }).ToList();

        return Result.Success(dtos);
    }
}

