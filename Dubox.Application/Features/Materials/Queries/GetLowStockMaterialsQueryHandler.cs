using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Materials.Queries;

public class GetLowStockMaterialsQueryHandler : IRequestHandler<GetLowStockMaterialsQuery, Result<List<LowStockMaterialDto>>>
{
    private readonly IDbContext _dbContext;

    public GetLowStockMaterialsQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<LowStockMaterialDto>>> Handle(GetLowStockMaterialsQuery request, CancellationToken cancellationToken)
    {
        var materials = await _dbContext.Set<Material>()
            .Where(m => m.IsActive && m.CurrentStock.HasValue && m.MinimumStock.HasValue && m.CurrentStock <= m.MinimumStock)
            .ToListAsync(cancellationToken);

        var lowStockMaterials = materials.Adapt<List<LowStockMaterialDto>>();

        return Result.Success(lowStockMaterials);
    }
}

