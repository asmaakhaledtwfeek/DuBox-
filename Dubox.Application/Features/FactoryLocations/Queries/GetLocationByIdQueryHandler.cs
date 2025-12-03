using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, Result<FactoryLocationDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLocationByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FactoryLocationDto>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var location = await _unitOfWork.Repository<Domain.Entities.FactoryLocation>()
            .GetByIdAsync(request.LocationId, cancellationToken);

        if (location == null)
            return Result.Failure<FactoryLocationDto>("Location not found");

        var dto = location.Adapt<FactoryLocationDto>() with
        {
            AvailableCapacity = location.AvailableCapacity,
            IsFull = location.IsFull
        };

        return Result.Success(dto);
    }
}

