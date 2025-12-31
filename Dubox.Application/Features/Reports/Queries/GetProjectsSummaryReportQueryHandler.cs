using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Application.Utilities;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetProjectsSummaryReportQueryHandler : IRequestHandler<GetProjectsSummaryReportQuery, Result<ProjectsSummaryReportResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetProjectsSummaryReportQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<ProjectsSummaryReportResponseDto>> Handle(GetProjectsSummaryReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 25 : (request.PageSize > 100 ? 100 : request.PageSize);

            // Get filtered query for paginated list (with all filters applied)
            var paginatedResult = _unitOfWork.Repository<Project>().GetWithSpec(new ProjectsSummaryReportSpecification(request, accessibleProjectIds, enablePaging: true));

            var countResult = _unitOfWork.Repository<Project>().GetWithSpec(new ProjectsSummaryReportSpecification(request, accessibleProjectIds, enablePaging: false));
            var countQuery = countResult.Data;

            var totalCount = await countQuery.CountAsync(cancellationToken);

            // Calculate KPIs and status distribution from ALL accessible projects (without filters)
            // This ensures summary cards show true totals regardless of active filters
            var allProjectsQuery = _unitOfWork.Repository<Project>().GetWithSpec(
                new ProjectsSummaryReportSpecification(
                    new GetProjectsSummaryReportQuery 
                    { 
                        PageNumber = 1, 
                        PageSize = 1,
                        IsActive = null,  // Don't filter by IsActive for KPIs
                        Status = null,    // Don't filter by Status for KPIs
                        Search = null     // Don't filter by Search for KPIs
                    }, 
                    accessibleProjectIds, 
                    enablePaging: false
                )
            ).Data;

            var kpis = await CalculateKpisEfficiently(allProjectsQuery, cancellationToken);
            var statusDistribution = await CalculateStatusDistribution(allProjectsQuery, cancellationToken);

            // Fix: Use status distribution to get accurate completed projects count
            // This ensures consistency between summary cards and status chart
            var completedCountFromDistribution = statusDistribution.TryGetValue("Completed", out var completedCount) ? completedCount : 0;
            if (kpis.CompletedProjects != completedCountFromDistribution)
            {
                kpis = kpis with { CompletedProjects = completedCountFromDistribution };
            }

            var projects = await paginatedResult.Data.ToListAsync(cancellationToken);
            var projectItems = projects.Adapt<List<ProjectSummaryItemDto>>();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new ProjectsSummaryReportResponseDto
            {
                Items = projectItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Kpis = kpis,
                StatusDistribution = statusDistribution
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<ProjectsSummaryReportResponseDto>($"Failed to generate projects summary report: {ex.Message}");
        }
    }

    private async Task<ProjectsSummaryReportKpisDto> CalculateKpisEfficiently(IQueryable<Project> query, CancellationToken cancellationToken)
    {
        // Use status string comparison for consistency with status distribution calculation
        var stats = await query
            .GroupBy(p => 1)
            .Select(g => new
            {
                TotalProjects = g.Count(),
                ActiveProjects = g.Count(p => p.Status.ToString() == "Active"),
                InactiveProjects = g.Count(p => !p.IsActive),
                OnHoldProjects = g.Count(p => p.Status.ToString() == "OnHold"),
                CompletedProjects = g.Count(p => p.Status.ToString() == "Completed"),
                ArchivedProjects = g.Count(p => p.Status.ToString() == "Archived"),
                ClosedProjects = g.Count(p => p.Status.ToString() == "Closed"),
                TotalBoxes = g.Sum(p => p.TotalBoxes),
                AverageProgress = (decimal?)g.Average(p => p.ProgressPercentage)
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (stats == null)
        {
            return new ProjectsSummaryReportKpisDto
            {
                TotalProjects = 0,
                ActiveProjects = 0,
                InactiveProjects = 0,
                OnHoldProjects = 0,
                CompletedProjects = 0,
                ArchivedProjects = 0,
                ClosedProjects = 0,
                TotalBoxes = 0,
                AverageProgressPercentage = 0,
                AverageProgressPercentageFormatted = ProgressFormatter.FormatProgress(0)
            };
        }

        var averageProgress = stats.AverageProgress ?? 0m;

        return new ProjectsSummaryReportKpisDto
        {
            TotalProjects = stats.TotalProjects,
            ActiveProjects = stats.ActiveProjects,
            InactiveProjects = stats.InactiveProjects,
            OnHoldProjects = stats.OnHoldProjects,
            CompletedProjects = stats.CompletedProjects,
            ArchivedProjects = stats.ArchivedProjects,
            ClosedProjects = stats.ClosedProjects,
            TotalBoxes = stats.TotalBoxes,
            AverageProgressPercentage = averageProgress,
            AverageProgressPercentageFormatted = ProgressFormatter.FormatProgress(averageProgress)
        };
    }

    private async Task<Dictionary<string, int>> CalculateStatusDistribution(IQueryable<Project> query, CancellationToken cancellationToken)
    {
        var statusGroups = await query
            .GroupBy(p => p.Status.ToString())
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return statusGroups.ToDictionary(g => g.Status, g => g.Count);
    }
}

