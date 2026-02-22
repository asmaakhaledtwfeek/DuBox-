using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

/// <summary>
/// Query to get missing materials report - identifies material shortages by project
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
            // Get all project materials with their requirements and allocations
            var projectMaterialsQuery = _dbContext.ProjectMaterials
                .Include(pm => pm.Material)
                .Include(pm => pm.Project)
                .AsQueryable();

            if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
            {
                projectMaterialsQuery = projectMaterialsQuery.Where(pm => pm.ProjectId == request.ProjectId.Value);
            }

            var projectMaterials = await projectMaterialsQuery.ToListAsync(cancellationToken);

            if (!projectMaterials.Any())
            {
                return Result.Success(new List<MissingMaterialsReportDto>());
            }

            // Group by material and calculate shortages
            var materialGroups = projectMaterials
                .GroupBy(pm => new
                {
                    MaterialId = pm.MaterialId,
                    MaterialName = pm.Material.MaterialName,
                    MaterialCode = pm.Material.MaterialCode,
                    Unit = pm.Material.Unit
                })
                .Select(g => new
                {
                    g.Key.MaterialName,
                    g.Key.MaterialCode,
                    g.Key.Unit,
                    RequiredQuantity = g.Sum(pm => pm.RequiredQuantity ?? 0),
                    AllocatedQuantity = g.Sum(pm => pm.AllocatedQuantity ?? 0),
                    AffectedProjects = g.Select(pm => pm.ProjectId).Distinct().Count()
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
                    Unit = m.Unit ?? "units",
                    AffectedBoxes = m.AffectedProjects // Changed to projects
                })
                .OrderByDescending(m => m.ShortageQuantity)
                .ToList();

            return Result.Success(missingMaterials);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<MissingMaterialsReportDto>>($"Error generating missing materials report: {ex.Message}");
        }
    }
}
