using Dubox.Application.DTOs;
using Dubox.Application.Features.Materials.Commands;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, Result<MaterialDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateMaterialCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MaterialDto>> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.Repository<Material>()
            .GetByIdAsync(request.MaterialId, cancellationToken);

        if (material == null)
            return Result.Failure<MaterialDto>("Material not found.");

        if (!string.IsNullOrEmpty(request.MaterialCode) && request.MaterialCode != material.MaterialCode)
        {
            var codeExists = await _unitOfWork.Repository<Material>()
                .IsExistAsync(m => m.MaterialCode == request.MaterialCode, cancellationToken);

            if (codeExists)
                return Result.Failure<MaterialDto>("Cannot update: Material Code already exists for another material.");
        }


        ApplyPartialUpdate(material, request);

        _unitOfWork.Repository<Material>().Update(material);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = material.Adapt<MaterialDto>() with
        {
            IsLowStock = material.IsLowStock,
            NeedsReorder = material.NeedsReorder
        };

        return Result.Success(dto);
    }
    private void ApplyPartialUpdate(Material material, UpdateMaterialCommand request)
    {
        if (!string.IsNullOrEmpty(request.MaterialCode))
            material.MaterialCode = request.MaterialCode;

        if (!string.IsNullOrEmpty(request.MaterialName))
            material.MaterialName = request.MaterialName;

        if (request.MaterialCategory != null)
            material.MaterialCategory = request.MaterialCategory;

        if (request.Unit != null)
            material.Unit = request.Unit;

        if (request.SupplierName != null)
            material.SupplierName = request.SupplierName;

        if (request.UnitCost.HasValue)
            material.UnitCost = request.UnitCost.Value;

        if (request.MinimumStock.HasValue)
            material.MinimumStock = request.MinimumStock.Value;

        if (request.ReorderLevel.HasValue)
            material.ReorderLevel = request.ReorderLevel.Value;

        if (request.IsActive.HasValue)
            material.IsActive = request.IsActive.Value;
    }
}
