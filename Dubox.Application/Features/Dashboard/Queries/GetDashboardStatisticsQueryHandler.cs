using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Dashboard.Queries;

public class GetDashboardStatisticsQueryHandler : IRequestHandler<GetDashboardStatisticsQuery, Result<DashboardStatisticsDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetDashboardStatisticsQueryHandler(IDbContext dbContext, IProjectTeamVisibilityService visibilityService)
    {
        _dbContext = dbContext;
        _visibilityService = visibilityService;
    }

    public async Task<Result<DashboardStatisticsDto>> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken)
    {
        // Apply visibility filtering
        var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

        var projectsQuery = _dbContext.Projects.AsQueryable();
        var boxesQuery = _dbContext.Boxes.AsQueryable();
        var wirQuery = _dbContext.WIRRecords.Include(w => w.BoxActivity).ThenInclude(ba => ba.Box).AsQueryable();
        var activitiesQuery = _dbContext.BoxActivities.Include(ba => ba.Box).AsQueryable();

        // Apply visibility filtering to all queries
        if (accessibleProjectIds != null)
        {
            projectsQuery = projectsQuery.Where(p => accessibleProjectIds.Contains(p.ProjectId));
            boxesQuery = boxesQuery.Where(b => accessibleProjectIds.Contains(b.ProjectId));
            wirQuery = wirQuery.Where(w => accessibleProjectIds.Contains(w.BoxActivity.Box.ProjectId));
            activitiesQuery = activitiesQuery.Where(ba => accessibleProjectIds.Contains(ba.Box.ProjectId));
        }

        var totalProjects = await projectsQuery.CountAsync(cancellationToken);
        var activeProjects = await projectsQuery.CountAsync(p => p.Status == ProjectStatusEnum.Active, cancellationToken);

        var totalBoxes = await boxesQuery.CountAsync(cancellationToken);
        var boxesNotStarted = await boxesQuery.CountAsync(b => b.Status == BoxStatusEnum.NotStarted, cancellationToken);
        var boxesInProgress = await boxesQuery.CountAsync(b => b.Status == BoxStatusEnum.InProgress, cancellationToken);
        var boxesCompleted = await boxesQuery.CountAsync(b => b.Status == BoxStatusEnum.Completed, cancellationToken);
        var boxesDelayed = await boxesQuery.CountAsync(b => b.Status == BoxStatusEnum.Delayed, cancellationToken);

        var overallProgress = totalBoxes > 0
            ? await boxesQuery.AverageAsync(b => (double)b.ProgressPercentage, cancellationToken)
            : 0;

        var pendingWIRs = await wirQuery.CountAsync(w => w.Status == WIRRecordStatusEnum.Pending, cancellationToken);

        var totalActivities = await activitiesQuery.CountAsync(ba => ba.IsActive, cancellationToken);
        var completedActivities = await activitiesQuery.CountAsync(ba => ba.IsActive && ba.Status == BoxStatusEnum.Completed, cancellationToken);

        var statistics = new DashboardStatisticsDto
        {
            TotalProjects = totalProjects,
            ActiveProjects = activeProjects,
            TotalBoxes = totalBoxes,
            BoxesNotStarted = boxesNotStarted,
            BoxesInProgress = boxesInProgress,
            BoxesCompleted = boxesCompleted,
            BoxesDelayed = boxesDelayed,
            OverallProgress = (decimal)overallProgress,
            PendingWIRs = pendingWIRs,
            TotalActivities = totalActivities,
            CompletedActivities = completedActivities
        };

        return Result.Success(statistics);
    }
}

