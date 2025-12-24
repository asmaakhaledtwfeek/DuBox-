using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRRecords.Queries;

public class GetWIRRecordByIdQueryHandler : IRequestHandler<GetWIRRecordByIdQuery, Result<WIRRecordDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public GetWIRRecordByIdQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WIRRecordDto>> Handle(GetWIRRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var wirRecord = _unitOfWork.Repository<WIRRecord>().
           GetEntityWithSpec(new GetWIRRecordWIthIncludesSpecification(request.WIRRecordId));

        if (wirRecord == null)
            return Result.Failure<WIRRecordDto>("WIR record not found");

        // Get all activities up to this WIR checkpoint
        var activitiesUpToWIR = await _dbContext.BoxActivities
            .Include(ba => ba.ActivityMaster)
            .Where(ba => ba.BoxId == wirRecord.BoxActivity.BoxId && 
                        ba.Sequence <= wirRecord.BoxActivity.Sequence)
            .OrderBy(ba => ba.Sequence)
            .Select(ba => ba.ActivityMaster.ActivityName)
            .ToListAsync(cancellationToken);

        var dto = wirRecord.Adapt<WIRRecordDto>() with
        {
            BoxTag = wirRecord.BoxActivity.Box.BoxTag,
            BoxName = wirRecord.BoxActivity.Box.BoxName,
            ActivityName = wirRecord.BoxActivity.ActivityMaster.ActivityName,
            ActivityNames = activitiesUpToWIR,
            ActivityCount = activitiesUpToWIR.Count,
            RequestedByName = wirRecord.RequestedByUser.FullName ?? wirRecord.RequestedByUser.Email,
            InspectedByName = wirRecord.InspectedByUser != null ? (wirRecord.InspectedByUser.FullName ?? wirRecord.InspectedByUser.Email) : null
        };

        return Result.Success(dto);
    }
}

