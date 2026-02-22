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

            // This handler now works with ProjectMaterial for project-level material allocation
            // Box-level material tracking is handled by BoxMaterial entity
            var projectMaterial = _unitOfWork.Repository<ProjectMaterial>()
                .GetEntityWithSpec(new GetProjectMaterialWithIncludesSpecification(request.BoxMaterialId));

            if (projectMaterial == null)
                return Result.Failure<BoxMaterialDto>("Project Material entry not found.");

            if (request.AllocatedQuantity <= 0)
                return Result.Failure<BoxMaterialDto>("Allocated quantity must be greater than zero.");

            var material = projectMaterial.Material;

            var oldAllocation = projectMaterial.AllocatedQuantity ?? 0;
            var oldAllocatedStock = material.AllocatedStock ?? 0;

            var currentStock = material.CurrentStock ?? 0;
            var consumed = projectMaterial.ConsumedQuantity ?? 0;
            var newAllocation = request.AllocatedQuantity;
            var netChangeInAllocation = newAllocation - oldAllocation;
            var availableNonAllocatedStock = currentStock - oldAllocatedStock;

            if (newAllocation < consumed)
                return Result.Failure<BoxMaterialDto>(
                    $"Cannot reduce allocation to {newAllocation}. The material has already been consumed by {consumed} units.");

            if (netChangeInAllocation > 0 && netChangeInAllocation > availableNonAllocatedStock)
                return Result.Failure<BoxMaterialDto>(
                    $"Cannot increase allocation by {netChangeInAllocation}. Only {availableNonAllocatedStock} stock is currently available for allocation.");

            material.AllocatedStock = oldAllocatedStock + netChangeInAllocation;

            projectMaterial.AllocatedQuantity = newAllocation;
            projectMaterial.AllocatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Material>().Update(material);
            _unitOfWork.Repository<ProjectMaterial>().Update(projectMaterial);
            
            var projectMaterialLog = new AuditLog
            {
                TableName = nameof(ProjectMaterial),
                RecordId = projectMaterial.ProjectMaterialId,
                Action = "AllocationUpdate",
                OldValues = $"AllocatedQuantity: {oldAllocation}",
                NewValues = $"AllocatedQuantity: {newAllocation}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Material allocation changed by {netChangeInAllocation} units for Project {projectMaterial.ProjectId}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(projectMaterialLog, cancellationToken);

            var materialLog = new AuditLog
            {
                TableName = nameof(Material),
                RecordId = material.MaterialId,
                Action = "StockAllocationChange",
                OldValues = $"AllocatedStock: {oldAllocatedStock}",
                NewValues = $"AllocatedStock: {material.AllocatedStock}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Global allocated stock for Material '{material.MaterialName}' changed by {netChangeInAllocation} due to project allocation update."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(materialLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = projectMaterial.Adapt<BoxMaterialDto>();

            return Result.Success(dto);
        }
    }
}
