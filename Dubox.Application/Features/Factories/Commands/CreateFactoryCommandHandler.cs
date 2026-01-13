using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Factories.Commands;

public class CreateFactoryCommandHandler : IRequestHandler<CreateFactoryCommand, Result<FactoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateFactoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FactoryDto>> Handle(CreateFactoryCommand request, CancellationToken cancellationToken)
    {
        // Check if factory with this code already exists
        var factoryExists = await _unitOfWork.Repository<Factory>()
            .IsExistAsync(f => f.FactoryCode == request.FactoryCode, cancellationToken);

        if (factoryExists)
            return Result.Failure<FactoryDto>("Factory with this code already exists");

        // Create new factory
        var factory = new Factory
        {
            FactoryCode = request.FactoryCode,
            FactoryName = request.FactoryName,
            Location = request.Location,
            Capacity = request.Capacity,
            CurrentOccupancy = 0,
            IsActive = true
        };

        await _unitOfWork.Repository<Factory>().AddAsync(factory, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Map to DTO with computed properties
        var dto = factory.Adapt<FactoryDto>() with
        {
            AvailableCapacity = factory.AvailableCapacity,
            IsFull = factory.IsFull
        };

        return Result.Success(dto);
    }
}

