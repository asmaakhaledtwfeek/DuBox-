using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class ExportTeamsPerformanceReportQueryHandler : IRequestHandler<ExportTeamsPerformanceReportQuery, Result<Stream>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelService _excelService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public ExportTeamsPerformanceReportQueryHandler(IUnitOfWork unitOfWork, IExcelService excelService, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<Stream>> Handle(ExportTeamsPerformanceReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            var accessibleTeamIds = await _visibilityService.GetAccessibleTeamIdsAsync(cancellationToken);

            // Use specification to get filtered activities at database level (only activities with TeamId)
            var activitiesSpec = new ActivitiesReportSpecification(
                projectId: request.ProjectId,
                status: request.Status,
                accessibleProjectIds: accessibleProjectIds,
                enablePaging: false,
                onlyWithTeamId: true
            );

            var activitiesResult = _unitOfWork.Repository<Domain.Entities.BoxActivity>().GetWithSpec(activitiesSpec);
            var filteredActivities = await activitiesResult.Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var teamsResult = _unitOfWork.Repository<Domain.Entities.Team>().GetWithSpec(new TeamsPerformanceReportSpecification(request, accessibleTeamIds));
            var teams = await teamsResult.Data
                .Include(t => t.Members)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Group activities by TeamId for efficient lookup (TeamId is guaranteed to have value by specification)
            var activitiesByTeamId = filteredActivities
                .GroupBy(ba => ba.TeamId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var exportData = new List<TeamPerformanceExportDto>();

            foreach (var team in teams)
            {
                var teamActivities = activitiesByTeamId.TryGetValue(team.TeamId, out var activities)
                    ? activities
                    : new List<Domain.Entities.BoxActivity>();

                if (teamActivities.Count == 0 && (request.ProjectId.HasValue || request.Status.HasValue))
                {
                    continue;
                }

                var completed = teamActivities.Count(ba => ba.Status == BoxStatusEnum.Completed);
                var inProgress = teamActivities.Count(ba => ba.Status == BoxStatusEnum.InProgress);
                var pending = teamActivities.Count(ba => ba.Status == BoxStatusEnum.NotStarted);
                var delayed = teamActivities.Count(ba => ba.Status == BoxStatusEnum.Delayed);
                var averageProgress = teamActivities.Any()
                    ? teamActivities.Average(ba => ba.ProgressPercentage)
                    : 0;

                var membersCount = team.Members.Count(m => m.IsActive);
                var activitiesPerMember = membersCount > 0 ? (double)teamActivities.Count / membersCount : 0;
                var workloadLevel = activitiesPerMember < 3 ? "Low" : activitiesPerMember > 7 ? "Overloaded" : "Normal";

                exportData.Add(new TeamPerformanceExportDto
                {
                    TeamCode = team.TeamCode,
                    TeamName = team.TeamName,
                    MembersCount = membersCount,
                    TotalAssignedActivities = teamActivities.Count,
                    Completed = completed,
                    InProgress = inProgress,
                    Pending = pending,
                    Delayed = delayed,
                    AverageTeamProgress = averageProgress,
                    WorkloadLevel = workloadLevel
                });
            }

            var headers = new[]
            {
                "Team Code",
                "Team Name",
                "Members Count",
                "Total Assigned Activities",
                "Completed",
                "In-Progress",
                "Pending",
                "Delayed",
                "Average Team Progress (%)",
                "Workload Level"
            };

            var excelBytes = _excelService.ExportToExcel(exportData, headers);
            var stream = new MemoryStream(excelBytes);
            return Result.Success((Stream)stream);
        }
        catch (Exception ex)
        {
            return Result.Failure<Stream>($"Failed to export teams performance report: {ex.Message}");
        }
    }
}

