using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Cost.Queries;

public class GetCostCodeFilterOptionsQueryHandler : IRequestHandler<GetCostCodeFilterOptionsQuery, Result<CostCodeFilterOptionsDto>>
{
    private readonly IDbContext _context;

    public GetCostCodeFilterOptionsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CostCodeFilterOptionsDto>> Handle(GetCostCodeFilterOptionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Start with all active cost codes
            var query = _context.CostCodes.Where(c => c.IsActive).AsQueryable();

            // Apply cascading filters
            if (!string.IsNullOrWhiteSpace(request.Level1))
            {
                query = query.Where(c => c.CostCodeLevel1 == request.Level1);
            }

            if (!string.IsNullOrWhiteSpace(request.Level2))
            {
                query = query.Where(c => c.CostCodeLevel2 == request.Level2);
            }

            // Get distinct values for each level
            var level1Options = await _context.CostCodes
                .Where(c => c.IsActive && c.CostCodeLevel1 != null)
                .Select(c => c.CostCodeLevel1!)
                .Distinct()
                .OrderBy(l => l)
                .ToListAsync(cancellationToken);

            var level2Options = await query
                .Where(c => c.CostCodeLevel2 != null)
                .Select(c => c.CostCodeLevel2!)
                .Distinct()
                .OrderBy(l => l)
                .ToListAsync(cancellationToken);

            var level3Options = await query
                .Where(c => c.CostCodeLevel3 != null)
                .Select(c => c.CostCodeLevel3!)
                .Distinct()
                .OrderBy(l => l)
                .ToListAsync(cancellationToken);

            var result = new CostCodeFilterOptionsDto(
                level1Options,
                level2Options,
                level3Options
            );

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<CostCodeFilterOptionsDto>(new Error("QueryFailed", $"Failed to retrieve cost code filter options: {ex.Message}"));
        }
    }
}

