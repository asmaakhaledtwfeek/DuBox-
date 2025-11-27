using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

/// <summary>
/// Query to get box progress report grouped by building
/// </summary>
public record GetBoxProgressReportQuery(Guid? ProjectId = null) : IRequest<Result<List<BoxProgressReportDto>>>;

public class GetBoxProgressReportQueryHandler : IRequestHandler<GetBoxProgressReportQuery, Result<List<BoxProgressReportDto>>>
{
    private readonly IDbContext _dbContext;

    public GetBoxProgressReportQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<BoxProgressReportDto>>> Handle(GetBoxProgressReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all boxes, optionally filtered by project
            var boxesQuery = _dbContext.Boxes
                .Include(b => b.Project)
                .AsQueryable();

            if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
            {
                boxesQuery = boxesQuery.Where(b => b.ProjectId == request.ProjectId.Value);
            }

            var boxes = await boxesQuery.ToListAsync(cancellationToken);

            if (!boxes.Any())
            {
                return Result<List<BoxProgressReportDto>>.Success(new List<BoxProgressReportDto>());
            }

            // Group boxes by building and calculate statistics
            var groupedData = boxes
                .GroupBy(b => new 
                { 
                    Building = b.Building ?? "Unknown Building", 
                    ProjectId = b.ProjectId,
                    ProjectName = b.Project?.ProjectName ?? "Unknown Project"
                })
                .Select(g => new BoxProgressReportDto
                {
                    Building = g.Key.Building,
                    ProjectId = g.Key.ProjectId.ToString(),
                    ProjectName = g.Key.ProjectName,
                    // Classify boxes based on progress percentage into different phases
                    NonAssembled = g.Count(b => b.ProgressPercentage == 0),
                    Backing = g.Count(b => b.ProgressPercentage > 0 && b.ProgressPercentage < 20),
                    Released1stFix = g.Count(b => b.ProgressPercentage >= 20 && b.ProgressPercentage < 40),
                    Released2ndFix = g.Count(b => b.ProgressPercentage >= 40 && b.ProgressPercentage < 80),
                    Released3rdFix = g.Count(b => b.ProgressPercentage >= 80),
                    Total = g.Count(),
                    ProgressPercentage = g.Any() ? Math.Round(g.Average(b => b.ProgressPercentage), 2) : 0
                })
                .OrderBy(r => r.Building)
                .ToList();

            return Result<List<BoxProgressReportDto>>.Success(groupedData);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<BoxProgressReportDto>>($"Failed to generate box progress report: {ex.Message}");
        }
    }
}

