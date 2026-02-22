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

        // Calculate capacity based on sections if provided, otherwise use legacy method
        int? capacity = request.Capacity;
        
        if (request.Sections != null && request.Sections.Any())
        {
            // Calculate total capacity from all sections
            capacity = request.Sections.Sum(section =>
            {
                var minBayChar = section.MinBay.ToUpper()[0];
                var maxBayChar = section.MaxBay.ToUpper()[0];
                var rowCount = section.MaxRow - section.MinRow + 1;
                var bayCount = maxBayChar - minBayChar + 1;
                return rowCount * bayCount;
            });
        }
        else
        {
            // Legacy: Calculate capacity from factory-level bay and row
            var minBayChar = request.MinBay.ToUpper()[0];
            var maxBayChar = request.MaxBay.ToUpper()[0];
            var rowCount = request.MaxRow - request.MinRow + 1;
            var bayCount = maxBayChar - minBayChar + 1;
            capacity = rowCount * bayCount;
        }

        // Create new factory
        var factory = new Factory
        {
            FactoryCode = request.FactoryCode,
            FactoryName = request.FactoryName,
            Location = request.Location,
            Capacity = capacity,
            MinRow = request.MinRow,
            MaxRow = request.MaxRow,
            MinBay = request.MinBay.ToUpper(),
            MaxBay = request.MaxBay.ToUpper(),
            CurrentOccupancy = 0,
            IsActive = true
        };

        await _unitOfWork.Repository<Factory>().AddAsync(factory, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Create factory sections if provided
        if (request.Sections != null && request.Sections.Any())
        {
            foreach (var sectionDto in request.Sections)
            {
                var minBayChar = sectionDto.MinBay.ToUpper()[0];
                var maxBayChar = sectionDto.MaxBay.ToUpper()[0];
                var rowCount = sectionDto.MaxRow - sectionDto.MinRow + 1;
                var bayCount = maxBayChar - minBayChar + 1;
                var sectionCapacity = rowCount * bayCount;

                var section = new FactorySection
                {
                    FactoryId = factory.FactoryId,
                    SectionType = sectionDto.SectionType,
                    SectionName = sectionDto.SectionName,
                    MinRow = sectionDto.MinRow,
                    MaxRow = sectionDto.MaxRow,
                    MinBay = sectionDto.MinBay.ToUpper(),
                    MaxBay = sectionDto.MaxBay.ToUpper(),
                    Capacity = sectionCapacity,
                    CurrentOccupancy = 0,
                    IsActive = true
                };

                await _unitOfWork.Repository<FactorySection>().AddAsync(section, cancellationToken);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        // Map to DTO with computed properties and sections
        var dto = factory.Adapt<FactoryDto>() with
        {
            AvailableCapacity = factory.AvailableCapacity,
            IsFull = factory.IsFull
        };

        return Result.Success(dto);
    }
}

