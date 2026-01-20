using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Cost.Queries;

public class GetHRCostFilterOptionsQueryHandler : IRequestHandler<GetHRCostFilterOptionsQuery, Result<HRCostFilterOptionsDto>>
{
    private readonly IDbContext _context;

    public GetHRCostFilterOptionsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<HRCostFilterOptionsDto>> Handle(GetHRCostFilterOptionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Cascading filter hierarchy: Type → Chapter → Sub Chapter → Classification → Sub Classification → Units
            // Type is always loaded independently (all available types)
            // Each subsequent filter depends on the previous selections

            // Types are always loaded independently (first in hierarchy)
            // Filter out null and empty string values
            var types = await _context.HRCostRecords
                .Where(h => h.Type != null && h.Type != "")
                .Select(h => h.Type!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync(cancellationToken);

            // Start building cascading query for dependent filters
            var query = _context.HRCostRecords.AsQueryable();

            // Apply Type filter first (required for cascading)
            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                query = query.Where(h => h.Type == request.Type);
            }

            // Chapters depend on Type selection
            var chapters = await query
                .Where(h => h.Chapter != null)
                .Select(h => h.Chapter!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync(cancellationToken);

            // Apply Chapter filter for next level
            if (!string.IsNullOrWhiteSpace(request.Chapter))
            {
                query = query.Where(h => h.Chapter == request.Chapter);
            }

            // Sub Chapters depend on Chapter selection
            var subChapters = await query
                .Where(h => h.SubChapter != null)
                .Select(h => h.SubChapter!)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync(cancellationToken);

            // Apply SubChapter filter for next level
            if (!string.IsNullOrWhiteSpace(request.SubChapter))
            {
                query = query.Where(h => h.SubChapter == request.SubChapter);
            }

            // Classifications depend on Sub Chapter selection
            var classifications = await query
                .Where(h => h.Classification != null)
                .Select(h => h.Classification!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync(cancellationToken);

            // Apply Classification filter for next level
            if (!string.IsNullOrWhiteSpace(request.Classification))
            {
                query = query.Where(h => h.Classification == request.Classification);
            }

            // Sub Classifications depend on Classification selection
            var subClassifications = await query
                .Where(h => h.SubClassification != null)
                .Select(h => h.SubClassification!)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync(cancellationToken);

            // Apply SubClassification filter for next level
            if (!string.IsNullOrWhiteSpace(request.SubClassification))
            {
                query = query.Where(h => h.SubClassification == request.SubClassification);
            }

            // Units depend on Sub Classification selection
            var units = await query
                .Where(h => h.Units != null)
                .Select(h => h.Units!)
                .Distinct()
                .OrderBy(u => u)
                .ToListAsync(cancellationToken);

            // Codes are not used in filtering anymore but kept for API compatibility
            var codes = new List<string>();

            // Statuses are returned empty - frontend will use hardcoded values
            var statuses = new List<string>();

            var result = new HRCostFilterOptionsDto(
                codes,
                chapters,
                subChapters,
                classifications,
                subClassifications,
                units,
                types,
                statuses
            );

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<HRCostFilterOptionsDto>(new Error("QueryFailed", $"Failed to retrieve filter options: {ex.Message}"));
        }
    }
}

