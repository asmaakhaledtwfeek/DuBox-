using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands
{
    public class AllocateBoxMaterialCommandHandler : IRequestHandler<AllocateBoxMaterialCommand, Result<BoxMaterialDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AllocateBoxMaterialCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BoxMaterialDto>> Handle(AllocateBoxMaterialCommand request, CancellationToken cancellationToken)
        {
            if (request.AllocatedQuantity <= 0)

                return Result.Failure<BoxMaterialDto>("Allocated quantity must be greater than zero.");

            var boxMaterial = _unitOfWork.Repository<BoxMaterial>()
                .GetEntityWithSpec(new BoxMaterialWithIncludesSpecification(request.BoxMaterialId));

            if (boxMaterial == null)
                return Result.Failure<BoxMaterialDto>("Box Material entry not found.");


            var material = boxMaterial.Material;

            var currentStock = material.CurrentStock ?? 0;
            var currentAllocatedStock = material.AllocatedStock ?? 0;
            var oldAllocation = boxMaterial.AllocatedQuantity ?? 0;
            var consumed = boxMaterial.ConsumedQuantity ?? 0;
            var newAllocation = request.AllocatedQuantity;
            var netChangeInAllocation = newAllocation - oldAllocation;
            var availableNonAllocatedStock = currentStock - currentAllocatedStock;

            if (newAllocation < consumed)
                return Result.Failure<BoxMaterialDto>(
                    $"Cannot reduce allocation to {newAllocation}. The material has already been consumed by {consumed} units for this box.");

            if (netChangeInAllocation > 0 && netChangeInAllocation > availableNonAllocatedStock)
                return Result.Failure<BoxMaterialDto>(
                    $"Cannot increase allocation by {netChangeInAllocation}. Only {availableNonAllocatedStock} stock is currently available for allocation.");

            material.AllocatedStock = currentAllocatedStock + netChangeInAllocation;


            boxMaterial.AllocatedQuantity = newAllocation;
            boxMaterial.AllocatedDate = DateTime.UtcNow;

            if (newAllocation > consumed)
                boxMaterial.Status = Domain.Enums.BoxMaterialStatusEnum.Allocated;

            else if (newAllocation == consumed)
                boxMaterial.Status = Domain.Enums.BoxMaterialStatusEnum.Consumed;


            _unitOfWork.Repository<Material>().Update(material);
            _unitOfWork.Repository<BoxMaterial>().Update(boxMaterial);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = boxMaterial.Adapt<BoxMaterialDto>();

            return Result.Success(dto);
        }
    }
}
