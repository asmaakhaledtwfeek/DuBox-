using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

/// <summary>
/// Query to get overall report summary statistics
/// </summary>
public record GetReportSummaryQuery(Guid? ProjectId = null) : IRequest<Result<ReportSummaryDto>>;

public class GetReportSummaryQueryHandler : IRequestHandler<GetReportSummaryQuery, Result<ReportSummaryDto>>
{
    private readonly IDbContext _dbContext;

    public GetReportSummaryQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ReportSummaryDto>> Handle(GetReportSummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get boxes query
            var boxesQuery = _dbContext.Boxes.AsQueryable();
            if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
            {
                boxesQuery = boxesQuery.Where(b => b.ProjectId == request.ProjectId.Value);
            }

            // Get activities query
            var activitiesQuery = _dbContext.BoxActivities.AsQueryable();
            if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
            {
                activitiesQuery = activitiesQuery
                    .Include(a => a.Box)
                    .Where(a => a.Box.ProjectId == request.ProjectId.Value);
            }

            // Get teams query
            var teamsQuery = _dbContext.Teams
                .Where(t => t.IsActive)
                .AsQueryable();

            // Get projects query
            var projectsQuery = _dbContext.Projects.AsQueryable();
            if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
            {
                projectsQuery = projectsQuery.Where(p => p.ProjectId == request.ProjectId.Value);
            }

            // Execute all queries in parallel
            var boxes = await boxesQuery.ToListAsync(cancellationToken);
            var activities = await activitiesQuery.ToListAsync(cancellationToken);
            var totalProjects = await projectsQuery.CountAsync(cancellationToken);

            // Calculate summary statistics
            var totalBoxes = boxes.Count;
            var averageProgress = boxes.Any() 
                ? Math.Round((decimal)boxes.Average(b => b.ProgressPercentage), 2) 
                : 0;

            var pendingActivities = activities.Count(a => 
                a.Status == BoxStatusEnum.NotStarted && a.IsActive);

            var completedActivities = activities.Count(a => 
                a.Status == BoxStatusEnum.Completed && a.IsActive);

            // For active teams, count teams that have activities assigned
            var activeTeamIds = activities
                .Where(a => a.TeamId.HasValue && a.IsActive)
                .Select(a => a.TeamId!.Value)
                .Distinct()
                .Count();

            var summary = new ReportSummaryDto
            {
                TotalBoxes = totalBoxes,
                AverageProgress = averageProgress,
                PendingActivities = pendingActivities,
                ActiveTeams = activeTeamIds,
                TotalProjects = totalProjects,
                CompletedActivities = completedActivities
            };

            return Result<ReportSummaryDto>.Success(summary);
        }
        catch (Exception ex)
        {
            return Result.Failure<ReportSummaryDto>($"Failed to generate report summary: {ex.Message}");
        }
    }
}

