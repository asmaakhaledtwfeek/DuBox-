using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
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

        var lowStockMaterials = materials.Select(m => new LowStockMaterialDto
        {
            MaterialId = m.MaterialId,
            MaterialCode = m.MaterialCode,
            MaterialName = m.MaterialName,
            CurrentStock = m.CurrentStock,
            MinimumStock = m.MinimumStock,
            ReorderLevel = m.ReorderLevel,
            Shortage = (m.MinimumStock ?? 0) - (m.CurrentStock ?? 0),
            NeedsReorder = m.NeedsReorder
        }).ToList();

        return Result.Success(lowStockMaterials);
    }
}

