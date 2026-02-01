using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Materials.Queries;

public class GetAllMaterialsQueryHandler : IRequestHandler<GetAllMaterialsQuery, Result<List<MaterialDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllMaterialsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<MaterialDto>>> Handle(GetAllMaterialsQuery request, CancellationToken cancellationToken)
    {
        var materials = await _unitOfWork.Repository<Material>()
            .GetAllAsync(cancellationToken);

        var materialDtos = materials.Select(m => m.Adapt<MaterialDto>() with
        {
            IsLowStock = m.IsLowStock,
            NeedsReorder = m.NeedsReorder
        }).ToList();

        return Result.Success(materialDtos);
    }
}

