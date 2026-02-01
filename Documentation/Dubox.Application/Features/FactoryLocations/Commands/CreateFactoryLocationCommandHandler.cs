using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Commands;

public class CreateFactoryLocationCommandHandler : IRequestHandler<CreateFactoryLocationCommand, Result<FactoryLocationDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateFactoryLocationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FactoryLocationDto>> Handle(CreateFactoryLocationCommand request, CancellationToken cancellationToken)
    {
        var locationExists = await _unitOfWork.Repository<FactoryLocation>()
            .IsExistAsync(l => l.LocationCode == request.LocationCode, cancellationToken);

        if (locationExists)
            return Result.Failure<FactoryLocationDto>("Location with this code already exists");

        var location = new FactoryLocation
        {
            LocationCode = request.LocationCode,
            LocationName = request.LocationName,
            LocationType = request.LocationType,
            Bay = request.Bay,
            Row = request.Row,
            Position = request.Position,
            Capacity = request.Capacity,
            CurrentOccupancy = 0,
            IsActive = true
        };

        await _unitOfWork.Repository<FactoryLocation>().AddAsync(location, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = location.Adapt<FactoryLocationDto>() with
        {
            AvailableCapacity = location.AvailableCapacity,
            IsFull = location.IsFull
        };

        return Result.Success(dto);
    }
}

