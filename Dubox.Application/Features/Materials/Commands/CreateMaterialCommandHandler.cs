using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Materials.Commands;

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, Result<MaterialDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMaterialCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MaterialDto>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var materialExists = await _unitOfWork.Repository<Material>()
            .IsExistAsync(m => m.MaterialCode == request.MaterialCode, cancellationToken);

        if (materialExists)
            return Result.Failure<MaterialDto>("Material with this code already exists");

        var material = request.Adapt<Material>();
        material.IsActive = true;
        await _unitOfWork.Repository<Material>().AddAsync(material, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = material.Adapt<MaterialDto>() with
        {
            IsLowStock = material.IsLowStock,
            NeedsReorder = material.NeedsReorder
        };

        return Result.Success(dto);
    }
}

