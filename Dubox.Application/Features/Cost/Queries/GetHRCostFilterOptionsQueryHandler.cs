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
            // Start with all records
            var query = _context.HRCostRecords.AsQueryable();

            // Apply cascading filters based on what's already selected
            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                query = query.Where(h => h.Code == request.Code);
            }

            if (!string.IsNullOrWhiteSpace(request.Chapter))
            {
                query = query.Where(h => h.Chapter == request.Chapter);
            }

            if (!string.IsNullOrWhiteSpace(request.SubChapter))
            {
                query = query.Where(h => h.SubChapter == request.SubChapter);
            }

            if (!string.IsNullOrWhiteSpace(request.Classification))
            {
                query = query.Where(h => h.Classification == request.Classification);
            }

            if (!string.IsNullOrWhiteSpace(request.SubClassification))
            {
                query = query.Where(h => h.SubClassification == request.SubClassification);
            }

            if (!string.IsNullOrWhiteSpace(request.Units))
            {
                query = query.Where(h => h.Units == request.Units);
            }

            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                query = query.Where(h => h.Type == request.Type);
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                query = query.Where(h => h.Status == request.Status);
            }

            // Get distinct values for each filter
            var codes = await query
                .Where(h => h.Code != null)
                .Select(h => h.Code!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync(cancellationToken);

            var chapters = await query
                .Where(h => h.Chapter != null)
                .Select(h => h.Chapter!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync(cancellationToken);

            var subChapters = await query
                .Where(h => h.SubChapter != null)
                .Select(h => h.SubChapter!)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync(cancellationToken);

            var classifications = await query
                .Where(h => h.Classification != null)
                .Select(h => h.Classification!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync(cancellationToken);

            var subClassifications = await query
                .Where(h => h.SubClassification != null)
                .Select(h => h.SubClassification!)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync(cancellationToken);

            var units = await query
                .Where(h => h.Units != null)
                .Select(h => h.Units!)
                .Distinct()
                .OrderBy(u => u)
                .ToListAsync(cancellationToken);

            var types = await query
                .Where(h => h.Type != null)
                .Select(h => h.Type!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync(cancellationToken);

            var statuses = await query
                .Where(h => h.Status != null)
                .Select(h => h.Status!)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync(cancellationToken);

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

