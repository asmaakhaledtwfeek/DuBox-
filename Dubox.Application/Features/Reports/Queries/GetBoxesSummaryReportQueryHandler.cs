using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Application.Utilities;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetBoxesSummaryReportQueryHandler : IRequestHandler<GetBoxesSummaryReportQuery, Result<PaginatedBoxSummaryReportResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetBoxesSummaryReportQueryHandler(IUnitOfWork unitOfWork, IDbContext dbContext, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _visibilityService = visibilityService;
    }

    public async Task<Result<PaginatedBoxSummaryReportResponseDto>> Handle(GetBoxesSummaryReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 25 : (request.PageSize > 100 ? 100 : request.PageSize);

            var paginatedSpec = new BoxesSummaryReportSpecification(request, accessibleProjectIds, enablePaging: true);
            var paginatedResult = _unitOfWork.Repository<Box>().GetWithSpec(paginatedSpec);

            var countSpec = new BoxesSummaryReportSpecification(request, accessibleProjectIds, enablePaging: false);
            var countResult = _unitOfWork.Repository<Box>().GetWithSpec(countSpec);
            var totalCount = countResult.Count > 0 ? countResult.Count : await countResult.Data.CountAsync(cancellationToken);

            var kpis = await CalculateKpisEfficiently(countResult.Data, cancellationToken);
            var aggregations = await CalculateAggregationsEfficiently(countResult.Data, cancellationToken);

            var boxes = await paginatedResult.Data.ToListAsync(cancellationToken);

            var boxIds = boxes.Select(b => b.BoxId).ToList();

            var lastUpdateDates = boxIds.Any()
                ? await _dbContext.ProgressUpdates
                    .Where(pu => boxIds.Contains(pu.BoxId))
                    .GroupBy(pu => pu.BoxId)
                    .Select(g => new { BoxId = g.Key, LastUpdate = g.Max(pu => pu.UpdateDate) })
                    .ToDictionaryAsync(x => x.BoxId, x => (DateTime?)x.LastUpdate, cancellationToken)
                : new Dictionary<Guid, DateTime?>();

            var activitiesCounts = boxIds.Any()
                ? await _dbContext.BoxActivities
                    .Where(ba => boxIds.Contains(ba.BoxId) && ba.IsActive)
                    .GroupBy(ba => ba.BoxId)
                    .Select(g => new { BoxId = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.BoxId, x => x.Count, cancellationToken)
                : new Dictionary<Guid, int>();

            var assetsCounts = boxIds.Any()
                ? await _dbContext.BoxAssets
                    .Where(ba => boxIds.Contains(ba.BoxId))
                    .GroupBy(ba => ba.BoxId)
                    .Select(g => new { BoxId = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.BoxId, x => x.Count, cancellationToken)
                : new Dictionary<Guid, int>();

            var qualityIssuesCounts = boxIds.Any()
                ? await _dbContext.QualityIssues
                    .Where(qi => boxIds.Contains(qi.BoxId) 
                        && qi.Status != QualityIssueStatusEnum.Resolved 
                        && qi.Status != QualityIssueStatusEnum.Closed)
                    .GroupBy(qi => qi.BoxId)
                    .Select(g => new { BoxId = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.BoxId, x => x.Count, cancellationToken)
                : new Dictionary<Guid, int>();

            // Fetch box type and subtype names for all boxes
            // Group by project to ensure we query the correct project-specific types
            var boxTypeLookup = boxes
                .Where(b => b.ProjectBoxTypeId.HasValue)
                .Select(b => new { b.ProjectId, BoxTypeId = b.ProjectBoxTypeId!.Value })
                .Distinct()
                .ToList();
            
            var boxSubTypeIds = boxes.Where(b => b.ProjectBoxSubTypeId.HasValue).Select(b => b.ProjectBoxSubTypeId!.Value).Distinct().ToList();
            
            // Fetch all box types in a single query
            var boxTypeNames = new Dictionary<(Guid ProjectId, int BoxTypeId), string>();
            if (boxTypeLookup.Any())
            {
                var projectIds = boxTypeLookup.Select(l => l.ProjectId).Distinct().ToList();
                var typeIds = boxTypeLookup.Select(l => l.BoxTypeId).Distinct().ToList();
                
                var allProjectBoxTypes = await _dbContext.ProjectBoxTypes
                    .Where(pbt => projectIds.Contains(pbt.ProjectId) && typeIds.Contains(pbt.Id))
                    .ToListAsync(cancellationToken);
                
                foreach (var lookup in boxTypeLookup)
                {
                    var projectBoxType = allProjectBoxTypes
                        .FirstOrDefault(pbt => pbt.Id == lookup.BoxTypeId && pbt.ProjectId == lookup.ProjectId);
                    if (projectBoxType != null)
                    {
                        boxTypeNames[(lookup.ProjectId, lookup.BoxTypeId)] = projectBoxType.TypeName;
                    }
                }
            }
            
            var boxSubTypeNames = boxSubTypeIds.Any()
                ? await _dbContext.ProjectBoxSubTypes
                    .Where(pbst => boxSubTypeIds.Contains(pbst.Id))
                    .ToDictionaryAsync(pbst => pbst.Id, pbst => pbst.SubTypeName, cancellationToken)
                : new Dictionary<int, string>();

            var items = boxes.Select(box =>
            {
                var dto = box.Adapt<BoxSummaryReportItemDto>();

                // Get box type and subtype names
                string? boxTypeName = null;
                string? boxSubTypeName = null;
                if (box.ProjectBoxTypeId.HasValue && boxTypeNames.ContainsKey((box.ProjectId, box.ProjectBoxTypeId.Value)))
                {
                    boxTypeName = boxTypeNames[(box.ProjectId, box.ProjectBoxTypeId.Value)];
                }
                if (box.ProjectBoxSubTypeId.HasValue && boxSubTypeNames.ContainsKey(box.ProjectBoxSubTypeId.Value))
                {
                    boxSubTypeName = boxSubTypeNames[box.ProjectBoxSubTypeId.Value];
                }

                // Get factory name and position
                string? factoryName = null;
                string? factoryPosition = null;
                if (box.FactoryId != null)
                {
                    factoryName = box.Factory?.FactoryName;
                    // Build position string from Bay, Row, Position
                    var positionParts = string.Empty;
                    if (!string.IsNullOrWhiteSpace(box.Position))
                        positionParts=$"Pos: {box.Position}";
                  
                    
                    factoryPosition = positionParts;
                }
                else if (!string.IsNullOrWhiteSpace(box.Bay) || !string.IsNullOrWhiteSpace(box.Row) || !string.IsNullOrWhiteSpace(box.Position))
                {
                    var positionParts = string.Empty;
                    if (!string.IsNullOrWhiteSpace(box.Position))
                        positionParts = $"Pos: {box.Position}";
                }

                dto = dto with
                {
                    BoxTypeName = boxTypeName,
                    BoxSubTypeName = boxSubTypeName,
                    FactoryName = factoryName,
                    FactoryPosition = factoryPosition,
                    LastUpdateDate = lastUpdateDates.ContainsKey(box.BoxId)
                        ? lastUpdateDates[box.BoxId]
                        : box.ModifiedDate,
                    ActivitiesCount = activitiesCounts.ContainsKey(box.BoxId)
                        ? activitiesCounts[box.BoxId]
                        : 0,
                    AssetsCount = assetsCounts.ContainsKey(box.BoxId)
                        ? assetsCounts[box.BoxId]
                        : 0,
                    QualityIssuesCount = qualityIssuesCounts.ContainsKey(box.BoxId)
                        ? qualityIssuesCounts[box.BoxId]
                        : 0
                };

                return dto;
            }).ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PaginatedBoxSummaryReportResponseDto
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Kpis = kpis,
                Aggregations = aggregations
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatedBoxSummaryReportResponseDto>($"Failed to generate boxes summary report: {ex.Message}");
        }
    }



    private async Task<BoxSummaryReportKpisDto> CalculateKpisEfficiently(IQueryable<Box> query, CancellationToken cancellationToken)
    {
        var stats = await query
            .GroupBy(b => 1)
            .Select(g => new
            {
                TotalBoxes = g.Count(),
                InProgressCount = g.Count(b => b.Status == BoxStatusEnum.InProgress),
                CompletedCount = g.Count(b => b.Status == BoxStatusEnum.Completed),
                NotStartedCount = g.Count(b => b.Status == BoxStatusEnum.NotStarted),
                DispatchedCount = g.Count(b => b.Status == BoxStatusEnum.Dispatched),
                AverageProgress = (decimal?)g.Average(b => b.ProgressPercentage)
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (stats == null)
        {
            return new BoxSummaryReportKpisDto
            {
                TotalBoxes = 0,
                InProgressCount = 0,
                CompletedCount = 0,
                NotStartedCount = 0,
                DispatchedCount = 0,
                AverageProgress = 0,
                AverageProgressFormatted = ProgressFormatter.FormatProgress(0)
            };
        }

        var averageProgress = stats.AverageProgress ?? 0m;

        return new BoxSummaryReportKpisDto
        {
            TotalBoxes = stats.TotalBoxes,
            InProgressCount = stats.InProgressCount,
            CompletedCount = stats.CompletedCount,
            NotStartedCount = stats.NotStartedCount,
            DispatchedCount = stats.DispatchedCount,
            AverageProgress = averageProgress,
            AverageProgressFormatted = ProgressFormatter.FormatProgress(averageProgress)
        };
    }

    private async Task<BoxSummaryReportAggregationsDto> CalculateAggregationsEfficiently(IQueryable<Box> query, CancellationToken cancellationToken)
    {
        var statusGroups = await query
            .GroupBy(b => b.Status.ToString())
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var statusDistribution = statusGroups.ToDictionary(g => g.Status, g => g.Count);

        var progressRangeGroups = await query
            .GroupBy(b => b.ProgressPercentage >= 0 && b.ProgressPercentage <= 25 ? "0-25" :
                         b.ProgressPercentage > 25 && b.ProgressPercentage <= 50 ? "26-50" :
                         b.ProgressPercentage > 50 && b.ProgressPercentage <= 75 ? "51-75" : "76-100")
            .Select(g => new { Range = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var progressRanges = new Dictionary<string, int>
        {
            ["0-25"] = 0,
            ["26-50"] = 0,
            ["51-75"] = 0,
            ["76-100"] = 0
        };

        foreach (var group in progressRangeGroups)
        {
            if (progressRanges.ContainsKey(group.Range))
            {
                progressRanges[group.Range] = group.Count;
            }
        }

        var topProjects = await query
            .Where(b => b.Project != null)
            .GroupBy(b => new { b.ProjectId, b.Project!.ProjectCode, b.Project!.ProjectName })
            .Select(g => new ProjectBoxCountDto
            {
                ProjectId = g.Key.ProjectId,
                ProjectCode = g.Key.ProjectCode,
                ProjectName = g.Key.ProjectName,
                BoxCount = g.Count()
            })
            .OrderByDescending(p => p.BoxCount)
            .Take(5)
            .ToListAsync(cancellationToken);

        return new BoxSummaryReportAggregationsDto
        {
            StatusDistribution = statusDistribution,
            ProgressRangeDistribution = progressRanges,
            TopProjects = topProjects
        };
    }
}
