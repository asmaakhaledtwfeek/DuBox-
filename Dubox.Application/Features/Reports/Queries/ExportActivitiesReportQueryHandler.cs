using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Application.Utilities;
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
                .ToListAsync(cancellationToken);

            if (batch.Count == 0)
            {
                hasMoreData = false;
                break;
            }

            foreach (var item in batch)
            {
                var actualDurationValues = DurationFormatter.CalculateDurationValues(item.ActualStartDate, item.ActualEndDate);
                var actualDurationFormatted = DurationFormatter.FormatDuration(item.ActualStartDate, item.ActualEndDate);
                
                int? delayDays = null;
                string delayDaysFormatted = string.Empty;
                if (item.Duration.HasValue && item.ActualStartDate.HasValue && item.ActualEndDate.HasValue && actualDurationValues != null)
                {
                    // Convert planned duration (days) to hours
                    var plannedHours = item.Duration.Value * 24.0;
                    var actualHours = actualDurationValues.TotalHours;
                    
                    // Calculate delay in hours
                    var delayHours = actualHours - plannedHours;
                    
                    // Only calculate delay if actual exceeds planned
                    if (delayHours > 0)
                    {
                        // Convert delay hours to days (round down for integer days)
                        delayDays = (int)Math.Floor(delayHours / 24.0);
                        var remainingHours = (int)Math.Round(delayHours % 24);
                        
                        // If remaining hours is 24, it means it's a full extra day
                        if (remainingHours == 24)
                        {
                            delayDays++;
                            remainingHours = 0;
                        }
                        
                        // Format delay: "X days Y hours" or just "X days" if no hours
                        if (remainingHours == 0)
                        {
                            delayDaysFormatted = delayDays == 1 ? "1 day" : $"{delayDays} days";
                        }
                        else
                        {
                            var daysText = delayDays == 1 ? "1 day" : $"{delayDays} days";
                            var hoursText = remainingHours == 1 ? "1 hour" : $"{remainingHours} hours";
                            delayDaysFormatted = $"{daysText} {hoursText}";
                        }
                    }
                }

                allActivities.Add(new ActivityExportDto
                {
                    ActivityId = item.BoxActivityId,
                    ActivityName = item.ActivityMaster.ActivityName,
                    BoxTag = item.Box.BoxTag,
                    ProjectName = item.Box.Project.ProjectName,
                    AssignedTeam = item.Team != null ? item.Team.TeamName : string.Empty,
                    Status = item.Status.ToString(),
                    ProgressPercentage = item.ProgressPercentage,
                    PlannedStartDate = item.PlannedStartDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    PlannedEndDate = item.PlannedEndDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    ActualStartDate = item.ActualStartDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    ActualEndDate = item.ActualEndDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    ActualDuration = actualDurationFormatted ?? (actualDurationValues != null ? DurationFormatter.CalculateDurationInDays(item.ActualStartDate, item.ActualEndDate)?.ToString() : string.Empty) ?? string.Empty,
                    DelayDays = delayDaysFormatted ?? delayDays?.ToString() ?? string.Empty
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

