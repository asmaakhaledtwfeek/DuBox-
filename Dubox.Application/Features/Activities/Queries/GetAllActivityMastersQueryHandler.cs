using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Activities.Queries;

public class GetAllActivityMastersQueryHandler : IRequestHandler<GetAllActivityMastersQuery, Result<List<ActivityMasterDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllActivityMastersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ActivityMasterDto>>> Handle(GetAllActivityMastersQuery request, CancellationToken cancellationToken)
    {
        var activities = await _unitOfWork.Repository<ActivityMaster>()
            .GetAllAsync(cancellationToken);

        var activityDtos = activities
            .OrderBy(a => a.OverallSequence)
            .Adapt<List<ActivityMasterDto>>();

        return Result.Success(activityDtos);
    }
}

