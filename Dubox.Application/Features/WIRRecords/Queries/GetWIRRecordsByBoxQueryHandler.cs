using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRRecords.Queries;

public class GetWIRRecordsByBoxQueryHandler : IRequestHandler<GetWIRRecordsByBoxQuery, Result<List<WIRRecordDto>>>
{
    private readonly IDbContext _dbContext;

    public GetWIRRecordsByBoxQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<WIRRecordDto>>> Handle(GetWIRRecordsByBoxQuery request, CancellationToken cancellationToken)
    {
        var wirRecords = await _dbContext.WIRRecords
            .Include(w => w.BoxActivity)
                .ThenInclude(ba => ba.ActivityMaster)
            .Include(w => w.BoxActivity)
                .ThenInclude(ba => ba.Box)
            .Include(w => w.RequestedByUser)
            .Include(w => w.InspectedByUser)
            .Where(w => w.BoxActivity.BoxId == request.BoxId)
            .OrderByDescending(w => w.RequestedDate)
            .ToListAsync(cancellationToken);

        var wirDtos = wirRecords.Select(w =>
        {
            var dto = w.Adapt<WIRRecordDto>();
            return dto with
            {
                BoxTag = w.BoxActivity.Box.BoxTag,
                ActivityName = w.BoxActivity.ActivityMaster.ActivityName,
                RequestedByName = w.RequestedByUser.FullName ?? w.RequestedByUser.Email,
                InspectedByName = w.InspectedByUser != null ? (w.InspectedByUser.FullName ?? w.InspectedByUser.Email) : null
            };
        }).ToList();

        return Result.Success(wirDtos);
    }
}

