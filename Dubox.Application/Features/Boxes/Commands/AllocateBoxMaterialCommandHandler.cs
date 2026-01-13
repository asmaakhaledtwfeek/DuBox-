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
        private readonly ICurrentUserService _currentUserService;

        public AllocateBoxMaterialCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<BoxMaterialDto>> Handle(AllocateBoxMaterialCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            const string dateFormat = "yyyy-MM-dd HH:mm:ss";

            if (request.AllocatedQuantity <= 0)
                return Result.Failure<BoxMaterialDto>("Allocated quantity must be greater than zero.");

            var boxMaterial = _unitOfWork.Repository<BoxMaterial>()
                .GetEntityWithSpec(new BoxMaterialWithIncludesSpecification(request.BoxMaterialId));

            if (boxMaterial == null)
                return Result.Failure<BoxMaterialDto>("Box Material entry not found.");


            var material = boxMaterial.Material;

            var oldAllocationBoxMaterial = boxMaterial.AllocatedQuantity ?? 0;
            var oldAllocatedStockMaterial = material.AllocatedStock ?? 0;

            var currentStock = material.CurrentStock ?? 0;
            var consumed = boxMaterial.ConsumedQuantity ?? 0;
            var newAllocation = request.AllocatedQuantity;
            var netChangeInAllocation = newAllocation - oldAllocationBoxMaterial;
            var availableNonAllocatedStock = currentStock - oldAllocatedStockMaterial;

            if (newAllocation < consumed)
                return Result.Failure<BoxMaterialDto>(
                    $"Cannot reduce allocation to {newAllocation}. The material has already been consumed by {consumed} units for this box.");

            if (netChangeInAllocation > 0 && netChangeInAllocation > availableNonAllocatedStock)
                return Result.Failure<BoxMaterialDto>(
                    $"Cannot increase allocation by {netChangeInAllocation}. Only {availableNonAllocatedStock} stock is currently available for allocation.");

            material.AllocatedStock = oldAllocatedStockMaterial + netChangeInAllocation;

            boxMaterial.AllocatedQuantity = newAllocation;
            boxMaterial.AllocatedDate = DateTime.UtcNow;
            var oldStatus = boxMaterial.Status;
            if (newAllocation > consumed)
                boxMaterial.Status = Domain.Enums.BoxMaterialStatusEnum.Allocated;
            else if (newAllocation == consumed)
                boxMaterial.Status = Domain.Enums.BoxMaterialStatusEnum.Consumed;

            _unitOfWork.Repository<Material>().Update(material);
            _unitOfWork.Repository<BoxMaterial>().Update(boxMaterial);
            var boxMaterialLog = new AuditLog
            {
                TableName = nameof(BoxMaterial),
                RecordId = boxMaterial.BoxMaterialId,
                Action = "AllocationUpdate",
                OldValues = $"AllocatedQuantity: {oldAllocationBoxMaterial}, Status: {oldStatus.ToString()}",
                NewValues = $"AllocatedQuantity: {newAllocation}, Status: {boxMaterial.Status.ToString()}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Material allocation changed by {netChangeInAllocation} units for Box {boxMaterial.BoxId}. New quantity: {newAllocation}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(boxMaterialLog, cancellationToken);

            var materialLog = new AuditLog
            {
                TableName = nameof(Material),
                RecordId = material.MaterialId,
                Action = "StockAllocationChange",
                OldValues = $"AllocatedStock: {oldAllocatedStockMaterial}",
                NewValues = $"AllocatedStock: {material.AllocatedStock}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Global allocated stock for Material '{material.MaterialName}' changed by {netChangeInAllocation} due to Box allocation update."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(materialLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = boxMaterial.Adapt<BoxMaterialDto>();

            return Result.Success(dto);
        }
    }
}
