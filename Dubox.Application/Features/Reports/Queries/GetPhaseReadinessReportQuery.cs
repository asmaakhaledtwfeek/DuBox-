using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

/// <summary>
/// Query to get phase readiness report - tracks phase completion and blocking issues
/// </summary>
public record GetPhaseReadinessReportQuery(Guid? ProjectId = null) : IRequest<Result<List<PhaseReadinessReportDto>>>;

public class GetPhaseReadinessReportQueryHandler : IRequestHandler<GetPhaseReadinessReportQuery, Result<List<PhaseReadinessReportDto>>>
{
    private readonly IDbContext _dbContext;

    public GetPhaseReadinessReportQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<PhaseReadinessReportDto>>> Handle(GetPhaseReadinessReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all boxes with their activities
            var boxesQuery = _dbContext.Boxes
                .Include(b => b.BoxActivities)
                .AsQueryable();

            if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
            {
                boxesQuery = boxesQuery.Where(b => b.ProjectId == request.ProjectId.Value);
            }

            var boxes = await boxesQuery.ToListAsync(cancellationToken);

            if (!boxes.Any())
            {
                return Result.Success(new List<PhaseReadinessReportDto>());
            }

            // Define phases based on progress percentage ranges
            var phases = new[]
            {
                new { Name = "Assembly Phase", MinProgress = 0m, MaxProgress = 20m },
                new { Name = "Backing Phase", MinProgress = 20m, MaxProgress = 40m },
                new { Name = "1st Fix (MEP Phase 1)", MinProgress = 40m, MaxProgress = 60m },
                new { Name = "2nd Fix (Finishing)", MinProgress = 60m, MaxProgress = 80m },
                new { Name = "3rd Fix (MEP Phase 2)", MinProgress = 80m, MaxProgress = 100m }
            };

            var phaseReadiness = phases.Select(phase =>
            {
                var boxesInPhase = boxes.Where(b =>
                    b.ProgressPercentage >= phase.MinProgress &&
                    b.ProgressPercentage < phase.MaxProgress
                ).ToList();

                var readyBoxes = boxesInPhase.Count(b =>
                    b.Status != BoxStatusEnum.OnHold &&
                    b.Status != BoxStatusEnum.Delayed
                );

                var pendingBoxes = boxesInPhase.Count - readyBoxes;

                // Identify blocking issues
                var blockingIssues = new List<string>();
                var onHoldCount = boxesInPhase.Count(b => b.Status == BoxStatusEnum.OnHold);
                var delayedCount = boxesInPhase.Count(b => b.Status == BoxStatusEnum.Delayed);

                if (onHoldCount > 0)
                    blockingIssues.Add($"{onHoldCount} boxes on hold");
                if (delayedCount > 0)
                    blockingIssues.Add($"{delayedCount} boxes delayed");

                var readinessPercentage = boxesInPhase.Count > 0
                    ? Math.Round((decimal)readyBoxes / boxesInPhase.Count * 100, 2)
                    : 100m;

                return new PhaseReadinessReportDto
                {
                    PhaseName = phase.Name,
                    TotalBoxes = boxesInPhase.Count,
                    ReadyBoxes = readyBoxes,
                    PendingBoxes = pendingBoxes,
                    ReadinessPercentage = readinessPercentage,
                    BlockingIssues = blockingIssues
                };
            }).ToList();

            return Result.Success(phaseReadiness);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<PhaseReadinessReportDto>>($"Failed to generate phase readiness report: {ex.Message}");
        }
    }
}


