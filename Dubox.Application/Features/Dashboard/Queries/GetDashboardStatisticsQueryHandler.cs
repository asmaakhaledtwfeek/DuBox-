using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Dashboard.Queries;

public class GetDashboardStatisticsQueryHandler : IRequestHandler<GetDashboardStatisticsQuery, Result<DashboardStatisticsDto>>
{
    private readonly IDbContext _dbContext;

    public GetDashboardStatisticsQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DashboardStatisticsDto>> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken)
    {
        var totalProjects = await _dbContext.Projects.CountAsync(cancellationToken);
        var activeProjects = await _dbContext.Projects.CountAsync(p => p.Status == ProjectStatusEnum.Active, cancellationToken);

        var totalBoxes = await _dbContext.Boxes.CountAsync(cancellationToken);
        var boxesNotStarted = await _dbContext.Boxes.CountAsync(b => b.Status == BoxStatusEnum.NotStarted, cancellationToken);
        var boxesInProgress = await _dbContext.Boxes.CountAsync(b => b.Status == BoxStatusEnum.InProgress, cancellationToken);
        var boxesCompleted = await _dbContext.Boxes.CountAsync(b => b.Status == BoxStatusEnum.Completed, cancellationToken);
        var boxesDelayed = await _dbContext.Boxes.CountAsync(b => b.Status == BoxStatusEnum.Delayed, cancellationToken);

        var overallProgress = totalBoxes > 0
            ? await _dbContext.Boxes.AverageAsync(b => (double)b.ProgressPercentage, cancellationToken)
            : 0;

        var pendingWIRs = await _dbContext.WIRRecords.CountAsync(w => w.Status == "Pending", cancellationToken);

        var totalActivities = await _dbContext.BoxActivities.CountAsync(ba => ba.IsActive, cancellationToken);
        var completedActivities = await _dbContext.BoxActivities.CountAsync(ba => ba.IsActive && ba.Status == BoxStatusEnum.Completed, cancellationToken);

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

