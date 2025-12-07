using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class ExportActivitiesReportQueryHandler : IRequestHandler<ExportActivitiesReportQuery, Result<Stream>>
{
    private readonly IDbContext _dbContext;
    private readonly IExcelService _excelService;
    private const int BatchSize = 1000;

    public ExportActivitiesReportQueryHandler(IDbContext dbContext, IExcelService excelService)
    {
        _dbContext = dbContext;
        _excelService = excelService;
    }

    public async Task<Result<Stream>> Handle(ExportActivitiesReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await ExportToExcelAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure<Stream>($"Failed to export activities report: {ex.Message}");
        }
    }

    private async Task<Result<Stream>> ExportToExcelAsync(ExportActivitiesReportQuery request, CancellationToken cancellationToken)
    {
        // Build base query
        var baseQuery = BuildBaseQuery(request);

        // Collect all data in batches to avoid loading everything into memory at once
        var allActivities = new List<ActivityExportDto>();
        int skip = 0;
        bool hasMoreData = true;

        while (hasMoreData)
        {
            var batch = await baseQuery
                .OrderByDescending(ba => ba.BoxActivityId)
                .Skip(skip)
                .Take(BatchSize)
                .Select(ba => new
                {
                    ActivityId = ba.BoxActivityId,
                    ActivityName = ba.ActivityMaster.ActivityName,
                    BoxTag = ba.Box.BoxTag,
                    ProjectName = ba.Box.Project.ProjectName,
                    TeamName = ba.Team != null ? ba.Team.TeamName : null,
                    Status = ba.Status.ToString(),
                    ProgressPercentage = ba.ProgressPercentage,
                    PlannedStartDate = ba.PlannedStartDate,
                    PlannedEndDate = ba.PlannedEndDate,
                    ActualStartDate = ba.ActualStartDate,
                    ActualEndDate = ba.ActualEndDate,
                    ActualDuration = ba.ActualStartDate.HasValue && ba.ActualEndDate.HasValue
                        ? (int?)(ba.ActualEndDate.Value.Date - ba.ActualStartDate.Value.Date).Days + 1
                        : null,
                    DelayDays = ba.PlannedEndDate.HasValue &&
                                !ba.ActualEndDate.HasValue &&
                                ba.PlannedEndDate < DateTime.UtcNow
                                ? (int?)(DateTime.UtcNow.Date - ba.PlannedEndDate.Value.Date).Days
                                : null
                })
                .ToListAsync(cancellationToken);

            if (batch.Count == 0)
            {
                hasMoreData = false;
                break;
            }

            // Transform to ActivityExportDto with proper formatting
            foreach (var item in batch)
            {
                allActivities.Add(new ActivityExportDto
                {
                    ActivityId = item.ActivityId,
                    ActivityName = item.ActivityName,
                    BoxTag = item.BoxTag,
                    ProjectName = item.ProjectName,
                    AssignedTeam = item.TeamName ?? string.Empty,
                    Status = item.Status,
                    ProgressPercentage = item.ProgressPercentage,
                    PlannedStartDate = item.PlannedStartDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    PlannedEndDate = item.PlannedEndDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    ActualStartDate = item.ActualStartDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    ActualEndDate = item.ActualEndDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    ActualDuration = item.ActualDuration?.ToString() ?? string.Empty,
                    DelayDays = item.DelayDays?.ToString() ?? string.Empty
                });
            }

            skip += BatchSize;
            hasMoreData = batch.Count == BatchSize;
        }

        // Use ExcelService to generate the Excel file
        var headers = new[] {
            "Activity ID",
            "Activity Name",
            "Box Tag",
            "Project Name",
            "Assigned Team",
            "Status",
            "Progress %",
            "Planned Start Date",
            "Planned End Date",
            "Actual Start Date",
            "Actual End Date",
            "Actual Duration",
            "Delay Days"
        };

        var excelBytes = _excelService.ExportToExcel(allActivities, headers);
        var stream = new MemoryStream(excelBytes);

        return Result.Success((Stream)stream);
    }

    // Helper DTO for export - properties must be in the same order as headers
    // ExcelService maps properties by position, so order matters
    private class ActivityExportDto
    {
        // Properties in exact order of headers array
        public object ActivityId { get; set; } = string.Empty;
        public object ActivityName { get; set; } = string.Empty;
        public object BoxTag { get; set; } = string.Empty;
        public object ProjectName { get; set; } = string.Empty;
        public object AssignedTeam { get; set; } = string.Empty;
        public object Status { get; set; } = string.Empty;
        public object ProgressPercentage { get; set; } = string.Empty;
        public object PlannedStartDate { get; set; } = string.Empty;
        public object PlannedEndDate { get; set; } = string.Empty;
        public object ActualStartDate { get; set; } = string.Empty;
        public object ActualEndDate { get; set; } = string.Empty;
        public object ActualDuration { get; set; } = string.Empty;
        public object DelayDays { get; set; } = string.Empty;
    }

    private IQueryable<BoxActivity> BuildBaseQuery(ExportActivitiesReportQuery request)
    {
        var baseQuery = _dbContext.BoxActivities
            .AsNoTracking()
            .Where(ba => ba.IsActive);

        // Apply filters (same as GetActivitiesReportQueryHandler)
        if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
        {
            baseQuery = baseQuery.Where(ba => ba.Box.ProjectId == request.ProjectId.Value);
        }

        if (request.TeamId.HasValue && request.TeamId.Value != Guid.Empty)
        {
            baseQuery = baseQuery.Where(ba => ba.TeamId == request.TeamId.Value);
        }

        if (request.Status.HasValue)
        {
            var status = (BoxStatusEnum)request.Status.Value;
            baseQuery = baseQuery.Where(ba => ba.Status == status);
        }

        if (request.PlannedStartDateFrom.HasValue)
        {
            baseQuery = baseQuery.Where(ba => ba.PlannedStartDate.HasValue &&
                ba.PlannedStartDate >= request.PlannedStartDateFrom.Value);
        }

        if (request.PlannedStartDateTo.HasValue)
        {
            baseQuery = baseQuery.Where(ba => ba.PlannedStartDate.HasValue &&
                ba.PlannedStartDate <= request.PlannedStartDateTo.Value);
        }

        if (request.PlannedEndDateFrom.HasValue)
        {
            baseQuery = baseQuery.Where(ba => ba.PlannedEndDate.HasValue &&
                ba.PlannedEndDate >= request.PlannedEndDateFrom.Value);
        }

        if (request.PlannedEndDateTo.HasValue)
        {
            baseQuery = baseQuery.Where(ba => ba.PlannedEndDate.HasValue &&
                ba.PlannedEndDate <= request.PlannedEndDateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.Trim().ToLowerInvariant();
            baseQuery = baseQuery.Where(ba =>
                ba.ActivityMaster.ActivityName.ToLower().Contains(searchTerm) ||
                ba.Box.BoxTag.ToLower().Contains(searchTerm) ||
                ba.Box.Project.ProjectCode.ToLower().Contains(searchTerm));
        }

        return baseQuery;
    }
}

