using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Materials.Queries
{
    public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, Result<MaterialDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetMaterialByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<MaterialDto>> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
        {
            var material = await _unitOfWork.Repository<Material>().GetByIdAsync(request.materialId);
            if (material == null)
                return Result.Failure<MaterialDto>("Material not found");

            return Result.Success(material.Adapt<MaterialDto>());
        }
    }
}
