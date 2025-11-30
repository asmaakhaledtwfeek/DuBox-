using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxTypeStatsByProjectQueryHandler : IRequestHandler<GetBoxTypeStatsByProjectQuery, Result<BoxTypeStatsByProjectDto>>
{
    private readonly IDbContext _dbContext;

    public GetBoxTypeStatsByProjectQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<BoxTypeStatsByProjectDto>> Handle(GetBoxTypeStatsByProjectQuery request, CancellationToken cancellationToken)
    {
        // Verify project exists
        var projectExists = await _dbContext.Projects
            .AnyAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

        if (!projectExists)
        {
            return Result.Failure<BoxTypeStatsByProjectDto>("Project not found");
        }

        // Optimized query: Group boxes by BoxType and calculate statistics in a single database query
        var boxTypeStats = await _dbContext.Boxes
            .Where(b => b.ProjectId == request.ProjectId && b.IsActive)
            .GroupBy(b => b.BoxType)
            .Select(g => new BoxTypeStatDto
            {
                BoxType = g.Key,
                BoxCount = g.Count(),
                OverallProgress = g.Average(b => b.ProgressPercentage)
            })
            .OrderBy(s => s.BoxType)
            .ToListAsync(cancellationToken);

        var result = new BoxTypeStatsByProjectDto
        {
            ProjectId = request.ProjectId,
            BoxTypeStats = boxTypeStats
        };

        return Result.Success(result);
    }
}

