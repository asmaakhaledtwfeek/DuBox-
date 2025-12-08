using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

/// <summary>
/// Query to get missing materials report - identifies material shortages
/// </summary>
public record GetMissingMaterialsReportQuery(Guid? ProjectId = null) : IRequest<Result<List<MissingMaterialsReportDto>>>;

public class GetMissingMaterialsReportQueryHandler : IRequestHandler<GetMissingMaterialsReportQuery, Result<List<MissingMaterialsReportDto>>>
{
    private readonly IDbContext _dbContext;

    public GetMissingMaterialsReportQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<MissingMaterialsReportDto>>> Handle(GetMissingMaterialsReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all box materials with their requirements and allocations
            var boxMaterialsQuery = _dbContext.BoxMaterials
                .Include(bm => bm.Material)
                .Include(bm => bm.Box)
                    .ThenInclude(b => b.Project)
                .AsQueryable();

            if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
            {
                boxMaterialsQuery = boxMaterialsQuery.Where(bm => bm.Box.ProjectId == request.ProjectId.Value);
            }

            var boxMaterials = await boxMaterialsQuery.ToListAsync(cancellationToken);

            if (!boxMaterials.Any())
            {
                return Result.Success(new List<MissingMaterialsReportDto>());
            }

            // Group by material and calculate shortages
            var materialGroups = boxMaterials
                .GroupBy(bm => new
                {
                    MaterialId = bm.MaterialId,
                    MaterialName = bm.Material.MaterialName,
                    MaterialCode = bm.Material.MaterialCode,
                    Unit = bm.Material.Unit
                })
                .Select(g => new
                {
                    g.Key.MaterialName,
                    g.Key.MaterialCode,
                    g.Key.Unit,
                    RequiredQuantity = g.Sum(bm => bm.RequiredQuantity),
                    AllocatedQuantity = g.Sum(bm => bm.AllocatedQuantity),
                    AffectedBoxes = g.Select(bm => bm.BoxId).Distinct().Count()
                })
                .ToList();

            // Filter only materials with shortages
            var missingMaterials = materialGroups
                .Where(m => m.AllocatedQuantity < m.RequiredQuantity)
                .Select(m => new MissingMaterialsReportDto
                {
                    MaterialName = m.MaterialName,
                    MaterialCode = m.MaterialCode,
                    RequiredQuantity = (int)m.RequiredQuantity,
                    AvailableQuantity = (int)m.AllocatedQuantity,
                    ShortageQuantity = (int)(m.RequiredQuantity - m.AllocatedQuantity),
                    AffectedBoxes = m.AffectedBoxes,
                    Unit = m.Unit,
                    ExpectedDeliveryDate = null // Can be enhanced with purchase order data
                })
                .OrderByDescending(m => m.ShortageQuantity)
                .ToList();

            return Result.Success(missingMaterials);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<MissingMaterialsReportDto>>($"Failed to generate missing materials report: {ex.Message}");
        }
    }
}



