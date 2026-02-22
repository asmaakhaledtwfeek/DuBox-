using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Materials.Queries;

public class GetMaterialsByProjectQueryHandler : IRequestHandler<GetMaterialsByProjectQuery, Result<List<MaterialDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMaterialsByProjectQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<MaterialDto>>> Handle(GetMaterialsByProjectQuery request, CancellationToken cancellationToken)
    {
        // Get materials that are selected for this project through ProjectMaterials
        var projectMaterials = await _unitOfWork.Repository<ProjectMaterial>()
            .FindAsync(pm => pm.ProjectId == request.ProjectId && pm.IsSelected, cancellationToken);

        var materialIds = projectMaterials.Select(pm => pm.MaterialId).ToList();
        
        var materials = await _unitOfWork.Repository<Material>()
            .FindAsync(m => materialIds.Contains(m.MaterialId) && m.IsActive, cancellationToken);

        var materialDtos = materials.Select(m => m.Adapt<MaterialDto>() with
        {
            IsLowStock = m.IsLowStock,
            NeedsReorder = m.NeedsReorder
        }).ToList();

        return Result.Success(materialDtos);
    }
}
