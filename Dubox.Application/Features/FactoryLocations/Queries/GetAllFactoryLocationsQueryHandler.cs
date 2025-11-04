using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public class GetAllFactoryLocationsQueryHandler : IRequestHandler<GetAllFactoryLocationsQuery, Result<List<FactoryLocationDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllFactoryLocationsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<FactoryLocationDto>>> Handle(GetAllFactoryLocationsQuery request, CancellationToken cancellationToken)
    {
        var locations = await _unitOfWork.Repository<FactoryLocation>()
            .GetAllAsync(cancellationToken);

        var locationDtos = locations.Select(l => l.Adapt<FactoryLocationDto>() with
        {
            AvailableCapacity = l.AvailableCapacity,
            IsFull = l.IsFull
        }).ToList();

        return Result.Success(locationDtos);
    }
}

