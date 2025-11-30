using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

/// <summary>
/// Query to get team productivity report with performance metrics
/// </summary>
public record GetTeamProductivityReportQuery(Guid? ProjectId = null) : IRequest<Result<List<TeamProductivityReportDto>>>;

public class GetTeamProductivityReportQueryHandler : IRequestHandler<GetTeamProductivityReportQuery, Result<List<TeamProductivityReportDto>>>
{
    private readonly IDbContext _dbContext;

    public GetTeamProductivityReportQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<TeamProductivityReportDto>>> Handle(GetTeamProductivityReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all teams with their activities
            var teamsQuery = _dbContext.Teams
                .Include(t => t.AssignedActivities)
                    .ThenInclude(a => a.Box)
                        .ThenInclude(b => b.Project)
                .Where(t => t.IsActive)
                .AsQueryable();

            var teams = await teamsQuery.ToListAsync(cancellationToken);

            if (!teams.Any())
            {
                return Result<List<TeamProductivityReportDto>>.Success(new List<TeamProductivityReportDto>());
            }

            var reportData = new List<TeamProductivityReportDto>();

            foreach (var team in teams)
            {
                // Filter activities by project if specified
                var activities = team.AssignedActivities
                    .Where(a => a.IsActive && 
                               (!request.ProjectId.HasValue || 
                                request.ProjectId.Value == Guid.Empty || 
                                a.Box.ProjectId == request.ProjectId.Value))
                    .ToList();

                if (!activities.Any())
                {
                    // Include teams with no activities
                    reportData.Add(new TeamProductivityReportDto
                    {
                        TeamId = team.TeamId.ToString(),
                        TeamName = team.TeamName,
                        TotalActivities = 0,
                        CompletedActivities = 0,
                        InProgress = 0,
                        Pending = 0,
                        AverageCompletionTime = 0,
                        Efficiency = 0
                    });
                    continue;
                }

                var totalActivities = activities.Count;
                var completedActivities = activities.Count(a => a.Status == BoxStatusEnum.Completed);
                var inProgressActivities = activities.Count(a => a.Status == BoxStatusEnum.InProgress);
                var pendingActivities = activities.Count(a => a.Status == BoxStatusEnum.NotStarted);

                // Calculate average completion time for completed activities
                var completedWithDates = activities
                    .Where(a => a.Status == BoxStatusEnum.Completed && 
                               a.ActualStartDate.HasValue && 
                               a.ActualEndDate.HasValue)
                    .ToList();

                decimal averageCompletionTime = 0;
                if (completedWithDates.Any())
                {
                    var totalDays = completedWithDates
                        .Sum(a => (a.ActualEndDate!.Value - a.ActualStartDate!.Value).TotalDays);
                    averageCompletionTime = (decimal)(totalDays / completedWithDates.Count);
                }

                // Calculate efficiency as percentage of completed activities
                decimal efficiency = totalActivities > 0 
                    ? Math.Round((decimal)completedActivities / totalActivities * 100, 2) 
                    : 0;

                reportData.Add(new TeamProductivityReportDto
                {
                    TeamId = team.TeamId.ToString(),
                    TeamName = team.TeamName,
                    TotalActivities = totalActivities,
                    CompletedActivities = completedActivities,
                    InProgress = inProgressActivities,
                    Pending = pendingActivities,
                    AverageCompletionTime = Math.Round(averageCompletionTime, 2),
                    Efficiency = efficiency
                });
            }

            // Order by efficiency descending
            reportData = reportData.OrderByDescending(r => r.Efficiency).ToList();

            return Result<List<TeamProductivityReportDto>>.Success(reportData);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<TeamProductivityReportDto>>($"Failed to generate team productivity report: {ex.Message}");
        }
    }
}

