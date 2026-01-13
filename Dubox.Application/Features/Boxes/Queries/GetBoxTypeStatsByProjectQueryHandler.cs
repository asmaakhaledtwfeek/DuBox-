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

        // Get all boxes for the project
        var boxes = await _dbContext.Boxes
            .Where(b => b.ProjectId == request.ProjectId && b.IsActive)
            .ToListAsync(cancellationToken);

        // Parse BoxTag to extract type and subtype abbreviations
        // BoxTag format: ProjectNumber-Building-Floor-Type-SubType
        var boxesWithParsedTags = boxes.Select(b => {
            var parts = b.BoxTag.Split('-');
            string boxType = "Unknown";
            string? subType = null;
            
            // Extract type and subtype from BoxTag (format: Project-Building-Floor-Type-SubType)
            if (parts.Length >= 4)
            {
                boxType = parts[3]; // Type abbreviation
                if (parts.Length >= 5)
                {
                    subType = parts[4]; // SubType abbreviation
                }
            }
            
            return new {
                Box = b,
                BoxType = boxType,
                SubType = subType
            };
        }).ToList();

        // Group by box type and calculate statistics including sub types
        var boxTypeStats = boxesWithParsedTags
            .GroupBy(b => b.BoxType)
            .Select(typeGroup => new BoxTypeStatDto
            {
                BoxType = typeGroup.Key,
                BoxCount = typeGroup.Count(),
                OverallProgress = typeGroup.Any() ? typeGroup.Average(b => b.Box.ProgressPercentage) : 0,
                SubTypes = typeGroup
                    .Where(b => !string.IsNullOrEmpty(b.SubType))
                    .GroupBy(b => b.SubType!)
                    .Select(subTypeGroup => new BoxSubTypeStatDto
                    {
                        SubTypeName = subTypeGroup.Key,
                        SubTypeAbbreviation = subTypeGroup.Key,
                        BoxCount = subTypeGroup.Count(),
                        Progress = subTypeGroup.Any() ? subTypeGroup.Average(b => b.Box.ProgressPercentage) : 0
                    })
                    .OrderBy(st => st.SubTypeAbbreviation)
                    .ToList()
            })
            .OrderBy(s => s.BoxType)
            .ToList();

        var result = new BoxTypeStatsByProjectDto
        {
            ProjectId = request.ProjectId,
            BoxTypeStats = boxTypeStats
        };

        return Result.Success(result);
    }
}


