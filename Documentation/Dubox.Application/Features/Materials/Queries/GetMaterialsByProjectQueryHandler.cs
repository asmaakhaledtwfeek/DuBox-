using Dubox.Application.DTOs;
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
        // Get materials that are either:
        // 1. Specifically assigned to this project (ProjectId matches)
        // 2. Global materials (ProjectId is null)
        var materials = await _unitOfWork.Repository<Material>()
            .FindAsync(m => m.ProjectId == request.ProjectId || m.ProjectId == null, cancellationToken);

        var materialDtos = materials.Select(m => m.Adapt<MaterialDto>() with
        {
            IsLowStock = m.IsLowStock,
            NeedsReorder = m.NeedsReorder
        }).ToList();

        return Result.Success(materialDtos);
    }
}





