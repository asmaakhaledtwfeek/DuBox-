using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands
{
    public class DefineBoxMaterialRequirementCommandHandler : IRequestHandler<DefineBoxMaterialRequirementCommand, Result<List<BoxMaterialDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DefineBoxMaterialRequirementCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Result<List<BoxMaterialDto>>> Handle(DefineBoxMaterialRequirementCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            const string dateFormat = "yyyy-MM-dd HH:mm:ss";

            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);

            if (box == null)
                return Result.Failure<List<BoxMaterialDto>>("Box not found.");

            var materialIds = request.Requirements.Select(r => r.MaterialId).ToList();

            var existingMaterials = _unitOfWork.Repository<Material>()
                 .Get().Count(m => materialIds.Contains(m.MaterialId));

            if (existingMaterials != materialIds.Count)
                return Result.Failure<List<BoxMaterialDto>>("One or more Materials not found.");

            var oldRequirements = _unitOfWork.Repository<BoxMaterial>()
           .Get()
           .Where(am => am.BoxId == request.BoxId)
           .ToList();

            var oldRequirementsDetails = string.Join(" | ", oldRequirements.Select(r => $"ID:{r.MaterialId}, Qty:{r.RequiredQuantity}"));
            if (oldRequirements.Any())
            {
                _unitOfWork.Repository<BoxMaterial>().DeleteRange(oldRequirements);
                var deleteLog = new AuditLog
                {
                    TableName = nameof(BoxMaterial),
                    RecordId = request.BoxId,
                    Action = "BulkDeletion",
                    OldValues = oldRequirementsDetails,
                    NewValues = "N/A (Replaced by new requirements)",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"Cleared {oldRequirements.Count} material requirements for Box {request.BoxId}."
                };
                await _unitOfWork.Repository<AuditLog>().AddAsync(deleteLog, cancellationToken);
            }
            var newRequirements = request.Requirements.Select(r => new BoxMaterial
            {
                BoxId = request.BoxId,
                MaterialId = r.MaterialId,
                RequiredQuantity = r.RequiredQuantity,
                Status = BoxMaterialStatusEnum.Pending
            }).ToList();

            await _unitOfWork.Repository<BoxMaterial>().AddRangeAsync(newRequirements, cancellationToken);
            var newRequirementsDetails = string.Join(" | ", newRequirements.Select(r => $"ID:{r.MaterialId}, Qty:{r.RequiredQuantity}"));

            var createLog = new AuditLog
            {
                TableName = nameof(BoxMaterial),
                RecordId = request.BoxId,
                Action = "BulkCreation",
                OldValues = "N/A (Previous requirements cleared)",
                NewValues = newRequirementsDetails,
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Defined {newRequirements.Count} new material requirements for Box {request.BoxId}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(createLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var boxMaterialsavedRequirements = _unitOfWork.Repository<BoxMaterial>()
            .GetWithSpec(new BoxMaterialByBoxIdSpecification(request.BoxId)).Data.ToList();

            var dto = boxMaterialsavedRequirements.Adapt<List<BoxMaterialDto>>();
            return Result.Success(dto);
        }
    }
}
