using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRRecords.Queries;

public class GetAllWIRRecordsQueryHandler : IRequestHandler<GetAllWIRRecordsQuery, Result<List<WIRRecordDto>>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllWIRRecordsQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<WIRRecordDto>>> Handle(GetAllWIRRecordsQuery request, CancellationToken cancellationToken)
    {
        var wirRecords = _unitOfWork.Repository<WIRRecord>()
            .GetWithSpec(new GetAllWIRRecordWithIncludesSpecification()).Data.ToList();

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

