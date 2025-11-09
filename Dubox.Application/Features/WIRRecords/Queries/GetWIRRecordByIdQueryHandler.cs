using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRRecords.Queries;

public class GetWIRRecordByIdQueryHandler : IRequestHandler<GetWIRRecordByIdQuery, Result<WIRRecordDto>>
{
    private readonly IDbContext _dbContext;

    public GetWIRRecordByIdQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<WIRRecordDto>> Handle(GetWIRRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var wirRecord = await _dbContext.WIRRecords
            .Include(w => w.BoxActivity)
                .ThenInclude(ba => ba.ActivityMaster)
            .Include(w => w.BoxActivity)
                .ThenInclude(ba => ba.Box)
            .Include(w => w.RequestedByUser)
            .Include(w => w.InspectedByUser)
            .FirstOrDefaultAsync(w => w.WIRRecordId == request.WIRRecordId, cancellationToken);

        if (wirRecord == null)
            return Result.Failure<WIRRecordDto>("WIR record not found");

        var dto = wirRecord.Adapt<WIRRecordDto>() with
        {
            BoxTag = wirRecord.BoxActivity.Box.BoxTag,
            ActivityName = wirRecord.BoxActivity.ActivityMaster.ActivityName,
            RequestedByName = wirRecord.RequestedByUser.FullName ?? wirRecord.RequestedByUser.Email,
            InspectedByName = wirRecord.InspectedByUser != null ? (wirRecord.InspectedByUser.FullName ?? wirRecord.InspectedByUser.Email) : null
        };

        return Result.Success(dto);
    }
}

