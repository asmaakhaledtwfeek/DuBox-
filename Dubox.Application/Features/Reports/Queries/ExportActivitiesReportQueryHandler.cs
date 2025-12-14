using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class ExportActivitiesReportQueryHandler : IRequestHandler<ExportActivitiesReportQuery, Result<Stream>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelService _excelService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private const int BatchSize = 1000;

    public ExportActivitiesReportQueryHandler(IUnitOfWork unitOfWork, IExcelService excelService, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
        _visibilityService = visibilityService;
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
        // Apply visibility filtering
        var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

        var baseQuery = _unitOfWork.Repository<BoxActivity>().GetWithSpec(new ActivitiesReportSpecification(request, accessibleProjectIds)).Data;

        var allActivities = new List<ActivityExportDto>();
        int skip = 0;
        bool hasMoreData = true;

        while (hasMoreData)
        {
            var batch = await baseQuery
                .AsNoTracking()
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


}

