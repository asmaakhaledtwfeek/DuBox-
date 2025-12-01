using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Dashboard.Queries;

public class GetProjectDashboardQueryHandler : IRequestHandler<GetProjectDashboardQuery, Result<ProjectDashboardDto>>
{
    private readonly IDbContext _dbContext;

    public GetProjectDashboardQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ProjectDashboardDto>> Handle(GetProjectDashboardQuery request, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects
            .FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectDashboardDto>("Project not found");

        var totalBoxes = await _dbContext.Boxes.CountAsync(b => b.ProjectId == request.ProjectId, cancellationToken);
        var boxesNotStarted = await _dbContext.Boxes.CountAsync(b => b.ProjectId == request.ProjectId && b.Status == BoxStatusEnum.NotStarted, cancellationToken);
        var boxesInProgress = await _dbContext.Boxes.CountAsync(b => b.ProjectId == request.ProjectId && b.Status == BoxStatusEnum.InProgress, cancellationToken);
        var boxesCompleted = await _dbContext.Boxes.CountAsync(b => b.ProjectId == request.ProjectId && b.Status == BoxStatusEnum.Completed, cancellationToken);

        var progressPercentage = totalBoxes > 0
            ? await _dbContext.Boxes
                .Where(b => b.ProjectId == request.ProjectId)
                .AverageAsync(b => (double)b.ProgressPercentage, cancellationToken)
            : 0;

        var pendingWIRs = await _dbContext.WIRRecords
            .Include(w => w.BoxActivity)
            .Where(w => w.BoxActivity.Box.ProjectId == request.ProjectId && w.Status == WIRRecordStatusEnum.Pending)
            .CountAsync(cancellationToken);

        var projectDashboard = new ProjectDashboardDto
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
        };

        return Result.Success(projectDashboard);
    }
}

