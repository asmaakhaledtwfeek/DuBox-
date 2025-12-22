using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRRecords.Queries;

public class GetWIRRecordsByBoxQueryHandler : IRequestHandler<GetWIRRecordsByBoxQuery, Result<List<WIRRecordDto>>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    public GetWIRRecordsByBoxQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<WIRRecordDto>>> Handle(GetWIRRecordsByBoxQuery request, CancellationToken cancellationToken)
    {
        // Filter WIR records by the specific BoxId - CRITICAL FIX
        var wirRecords = await _unitOfWork.Repository<WIRRecord>()
            .GetWithSpec(new GetAllWIRRecordWithIncludesSpecification()).Data
            .Where(w => w.BoxActivity.BoxId == request.BoxId) // Filter by BoxId!
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var wirDtos = wirRecords.Select(w =>
        {
            var dto = w.Adapt<WIRRecordDto>();
            return dto with
            {
                BoxId = w.BoxActivity.BoxId, // Include BoxId in response
                BoxTag = w.BoxActivity.Box.BoxTag,
                ActivityName = w.BoxActivity.ActivityMaster.ActivityName,
                RequestedByName = w.RequestedByUser.FullName ?? w.RequestedByUser.Email,
                InspectedByName = w.InspectedByUser != null ? (w.InspectedByUser.FullName ?? w.InspectedByUser.Email) : null
            };
        }).ToList();

        return Result.Success(wirDtos);
    }
}

