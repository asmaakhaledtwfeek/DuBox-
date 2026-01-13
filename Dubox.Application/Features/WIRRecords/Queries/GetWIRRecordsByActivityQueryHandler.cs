using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRRecords.Queries;

public class GetWIRRecordsByActivityQueryHandler : IRequestHandler<GetWIRRecordsByActivityQuery, Result<List<WIRRecordDto>>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public GetWIRRecordsByActivityQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<WIRRecordDto>>> Handle(GetWIRRecordsByActivityQuery request, CancellationToken cancellationToken)
    {
        var wirRecords = _unitOfWork.Repository<WIRRecord>()
            .GetWithSpec(new GetAllWIRRecordWithIncludesSpecification()).Data
            .Where(w => w.BoxActivityId == request.BoxActivityId)
            .ToList();

        if (!wirRecords.Any())
            return Result.Success(new List<WIRRecordDto>());

        var wirDtos = new List<WIRRecordDto>();

        foreach (var w in wirRecords)
        {
            // Get all activities up to this WIR checkpoint (execute query separately)
            var activitiesUpToWIR = await _dbContext.BoxActivities
                .Include(ba => ba.ActivityMaster)
                .Where(ba => ba.BoxId == w.BoxActivity.BoxId && 
                            ba.Sequence <= w.BoxActivity.Sequence)
                .OrderBy(ba => ba.Sequence)
                .Select(ba => ba.ActivityMaster.ActivityName)
                .ToListAsync(cancellationToken);

            var dto = w.Adapt<WIRRecordDto>() with
            {
                BoxTag = w.BoxActivity.Box.BoxTag,
                BoxName = w.BoxActivity.Box.BoxName,
                ActivityName = w.BoxActivity.ActivityMaster.ActivityName,
                ActivityNames = activitiesUpToWIR,
                ActivityCount = activitiesUpToWIR.Count,
                RequestedByName = w.RequestedByUser.FullName ?? w.RequestedByUser.Email,
                InspectedByName = w.InspectedByUser != null ? (w.InspectedByUser.FullName ?? w.InspectedByUser.Email) : null
            };

            wirDtos.Add(dto);
        }

        return Result.Success(wirDtos);
    }
}

