using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Application.Utilities;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetProjectsSummaryReportQueryHandler : IRequestHandler<GetProjectsSummaryReportQuery, Result<ProjectsSummaryReportResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectsSummaryReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectsSummaryReportResponseDto>> Handle(GetProjectsSummaryReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 25 : (request.PageSize > 100 ? 100 : request.PageSize);

            var paginatedResult = _unitOfWork.Repository<Project>().GetWithSpec(new ProjectsSummaryReportSpecification(request, enablePaging: true));

            var countResult = _unitOfWork.Repository<Project>().GetWithSpec(new ProjectsSummaryReportSpecification(request, enablePaging: false));
            var countQuery = countResult.Data;

            var totalCount = await countQuery.CountAsync(cancellationToken);
            var kpis = await CalculateKpisEfficiently(countQuery, cancellationToken);
            var statusDistribution = await CalculateStatusDistribution(countQuery, cancellationToken);

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
        var stats = await query
            .GroupBy(p => 1)
            .Select(g => new
            {
                TotalProjects = g.Count(),
                ActiveProjects = g.Count(p => p.IsActive),
                InactiveProjects = g.Count(p => !p.IsActive),
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

