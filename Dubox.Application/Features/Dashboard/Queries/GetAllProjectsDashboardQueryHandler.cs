using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Dashboard.Queries;

public class GetAllProjectsDashboardQueryHandler : IRequestHandler<GetAllProjectsDashboardQuery, Result<List<ProjectDashboardDto>>>
{
    private readonly IDbContext _dbContext;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetAllProjectsDashboardQueryHandler(IDbContext dbContext, IProjectTeamVisibilityService visibilityService)
    {
        _dbContext = dbContext;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<ProjectDashboardDto>>> Handle(GetAllProjectsDashboardQuery request, CancellationToken cancellationToken)
    {
        // Apply visibility filtering
        var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

        var projectsQuery = _dbContext.Projects.Where(p => p.IsActive);

        // Apply visibility filtering
        if (accessibleProjectIds != null)
        {
            projectsQuery = projectsQuery.Where(p => accessibleProjectIds.Contains(p.ProjectId));
        }

        var projects = await projectsQuery.ToListAsync(cancellationToken);

        var projectDashboards = new List<ProjectDashboardDto>();

        foreach (var project in projects)
        {
            var totalBoxes = await _dbContext.Boxes.CountAsync(b => b.ProjectId == project.ProjectId, cancellationToken);
            var boxesNotStarted = await _dbContext.Boxes.CountAsync(b => b.ProjectId == project.ProjectId && b.Status == BoxStatusEnum.NotStarted, cancellationToken);
            var boxesInProgress = await _dbContext.Boxes.CountAsync(b => b.ProjectId == project.ProjectId && b.Status == BoxStatusEnum.InProgress, cancellationToken);
            var boxesCompleted = await _dbContext.Boxes.CountAsync(b => b.ProjectId == project.ProjectId && b.Status == BoxStatusEnum.Completed, cancellationToken);

            var progressPercentage = totalBoxes > 0
                ? await _dbContext.Boxes
                    .Where(b => b.ProjectId == project.ProjectId)
                    .AverageAsync(b => (double)b.ProgressPercentage, cancellationToken)
                : 0;

            var pendingWIRs = await _dbContext.WIRRecords
                .Include(w => w.BoxActivity)
                .Where(w => w.BoxActivity.Box.ProjectId == project.ProjectId && w.Status == WIRRecordStatusEnum.Pending)
                .CountAsync(cancellationToken);

            projectDashboards.Add(new ProjectDashboardDto
            {
                ProjectId = project.ProjectId,
                ProjectCode = project.ProjectCode,
                ProjectName = project.ProjectName,
                TotalBoxes = totalBoxes,
                BoxesNotStarted = boxesNotStarted,
                BoxesInProgress = boxesInProgress,
                BoxesCompleted = boxesCompleted,
                ProgressPercentage = (decimal)progressPercentage,
                PendingWIRs = pendingWIRs,
                StartDate = project.ActualStartDate ?? project.CompressionStartDate ?? project.PlannedStartDate,
                PlannedEndDate = project.PlannedEndDate,
                Status = project.Status
            });
        }

        return Result.Success(projectDashboards.OrderByDescending(p => p.StartDate).ThenBy(p => p.ProjectCode).ToList());
    }
}

