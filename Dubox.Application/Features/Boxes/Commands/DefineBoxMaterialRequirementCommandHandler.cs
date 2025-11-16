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

        public DefineBoxMaterialRequirementCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<BoxMaterialDto>>> Handle(DefineBoxMaterialRequirementCommand request, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);

            if (box == null)
                return Result.Failure<List<BoxMaterialDto>>("Box Activity not found.");

            var materialIds = request.Requirements.Select(r => r.MaterialId).ToList();

            var existingMaterials = _unitOfWork.Repository<Material>()
                 .Get().Count(m => materialIds.Contains(m.MaterialId));

            if (existingMaterials != materialIds.Count)
                return Result.Failure<List<BoxMaterialDto>>("One or more Materials not found.");

            var oldRequirements = _unitOfWork.Repository<BoxMaterial>()
           .Get()
           .Where(am => am.BoxId == request.BoxId)
           .ToList();
            if (oldRequirements.Any())
                _unitOfWork.Repository<BoxMaterial>().DeleteRange(oldRequirements);
            var newRequirements = request.Requirements.Select(r => new BoxMaterial
            {
                BoxId = request.BoxId,
                MaterialId = r.MaterialId,
                RequiredQuantity = r.RequiredQuantity,
                Status = BoxMaterialStatusEnum.Pending
            }).ToList();

            await _unitOfWork.Repository<BoxMaterial>().AddRangeAsync(newRequirements, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var boxMaterialsavedRequirements = _unitOfWork.Repository<BoxMaterial>()
            .GetWithSpec(new BoxMaterialByBoxIdSpecification(request.BoxId)).Data.ToList();

            var dto = boxMaterialsavedRequirements.Adapt<List<BoxMaterialDto>>();
            return Result.Success(dto);
        }
    }
}
