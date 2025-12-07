using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Activities.Queries;

public class GetBoxActivitiesByBoxQueryHandler : IRequestHandler<GetBoxActivitiesByBoxQuery, Result<List<BoxActivityDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBoxActivitiesByBoxQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<BoxActivityDto>>> Handle(GetBoxActivitiesByBoxQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetBoxActivitiesByBoxSpecification(request.BoxId);
        var boxActivitiesResult = _unitOfWork.Repository<BoxActivity>().GetWithSpec(specification);
        
        // Use AsNoTracking for read-only query to improve performance
        var boxActivities = boxActivitiesResult.Data
            .AsNoTracking()
            .ToList();

        var boxActivityDtos = boxActivities.Adapt<List<BoxActivityDto>>();

        return Result.Success(boxActivityDtos);
    }
}

