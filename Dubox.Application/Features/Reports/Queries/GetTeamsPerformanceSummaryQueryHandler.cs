using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetTeamsPerformanceSummaryQueryHandler : IRequestHandler<GetTeamsPerformanceSummaryQuery, Result<TeamsPerformanceSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetTeamsPerformanceSummaryQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<TeamsPerformanceSummaryDto>> Handle(GetTeamsPerformanceSummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            var accessibleTeamIds = await _visibilityService.GetAccessibleTeamIdsAsync(cancellationToken);

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

            var teamsSpec = new TeamsPerformanceReportSpecification(request, accessibleTeamIds);
            var teamsResult = _unitOfWork.Repository<Domain.Entities.Team>().GetWithSpec(teamsSpec);
            var teams = await teamsResult.Data
                .Include(t => t.Members)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var activitiesByTeamId = filteredActivities
                .GroupBy(ba => ba.TeamId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var allActivities = new List<Domain.Entities.BoxActivity>();
            var teamProgresses = new List<decimal>();

            foreach (var team in teams)
            {
                var teamActivities = activitiesByTeamId.TryGetValue(team.TeamId, out var activities)
                    ? activities
                    : new List<Domain.Entities.BoxActivity>();

                allActivities.AddRange(teamActivities);

                if (teamActivities.Any())
                {
                    var avgProgress = teamActivities.Average(ba => ba.ProgressPercentage);
                    teamProgresses.Add(avgProgress);
                }
            }

            var totalTeams = teams.Count;
            var totalTeamMembers = teams.Sum(t => t.Members.Count(m => m.IsActive));
            var totalAssignedActivities = allActivities.Count;
            var completedActivities = allActivities.Count(ba => ba.Status == BoxStatusEnum.Completed);
            var inProgressActivities = allActivities.Count(ba => ba.Status == BoxStatusEnum.InProgress);
            var delayedActivities = allActivities.Count(ba => ba.Status == BoxStatusEnum.Delayed);
            var averageTeamProgress = teamProgresses.Any() ? teamProgresses.Average() : 0;
            var teamWorkloadIndicator = totalTeams > 0 ? (decimal)totalAssignedActivities / totalTeams : 0;

            var summary = new TeamsPerformanceSummaryDto
            {
                TotalTeams = totalTeams,
                TotalTeamMembers = totalTeamMembers,
                TotalAssignedActivities = totalAssignedActivities,
                CompletedActivities = completedActivities,
                InProgressActivities = inProgressActivities,
                DelayedActivities = delayedActivities,
                AverageTeamProgress = averageTeamProgress,
                TeamWorkloadIndicator = teamWorkloadIndicator
            };

            return Result.Success(summary);
        }
        catch (Exception ex)
        {
            return Result.Failure<TeamsPerformanceSummaryDto>(
                $"Failed to generate teams performance summary: {ex.Message}");
        }
    }
}

