using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Cost.Queries;

public class GetDistinctCostTypesQueryHandler : IRequestHandler<GetDistinctCostTypesQuery, Result<List<CostTypeDto>>>
{
    private readonly IDbContext _context;

    public GetDistinctCostTypesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CostTypeDto>>> Handle(GetDistinctCostTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all HR costs with non-empty types and Active status
            var hrCosts = await _context.Set<HRCostRecord>()
                .Where(h => !string.IsNullOrEmpty(h.Type) && (h.Status == "Active" || h.Status == null))
                .Select(h => new { h.HRCostRecordId, h.Type })
                .ToListAsync(cancellationToken);

            // Then group and get distinct types in memory
            var costTypes = hrCosts
                .GroupBy(h => h.Type)
                .Select(g => new CostTypeDto(
                    g.First().HRCostRecordId,
                    g.Key!
                ))
                .OrderBy(ct => ct.CostType)
                .ToList();

            return Result.Success(costTypes);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<CostTypeDto>>(new Error("QueryError", $"Error retrieving cost types: {ex.Message}"));
        }
    }
}

