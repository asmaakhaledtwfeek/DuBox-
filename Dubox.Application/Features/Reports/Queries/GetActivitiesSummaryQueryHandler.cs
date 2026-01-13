using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;


public class GetActivitiesSummaryQueryHandler : IRequestHandler<GetActivitiesSummaryQuery, Result<ActivitiesSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetActivitiesSummaryQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<ActivitiesSummaryDto>> Handle(GetActivitiesSummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            var result = _unitOfWork.Repository<Domain.Entities.BoxActivity>().GetWithSpec(new ActivitiesReportSpecification(request, accessibleProjectIds));

            var query = result.Data.AsNoTracking();

            var today = DateTime.UtcNow.Date;
            var daysUntilSaturday = 6 - (int)today.DayOfWeek;
            var endOfWeek = today.AddDays(daysUntilSaturday);

            var kpiData = await query
                .GroupBy(ba => 1)
                .Select(g => new
                {
                    TotalActivities = g.Count(),
                    Completed = g.Count(ba => ba.Status == BoxStatusEnum.Completed),
                    InProgress = g.Count(ba => ba.Status == BoxStatusEnum.InProgress),
                    Pending = g.Count(ba => ba.Status == BoxStatusEnum.NotStarted),
                    Delayed = g.Count(ba => ba.Status == BoxStatusEnum.Delayed),
                    AverageProgress = (decimal?)g.Average(ba => ba.ProgressPercentage) ?? 0,
                    Overdue = g.Count(ba =>
                        ba.PlannedEndDate.HasValue &&
                        !ba.ActualEndDate.HasValue &&
                        ba.PlannedEndDate < today),
                    DueThisWeek = g.Count(ba =>
                        ba.PlannedEndDate.HasValue &&
                        !ba.ActualEndDate.HasValue &&
                        ba.PlannedEndDate >= today &&
                        ba.PlannedEndDate <= endOfWeek)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (kpiData == null)
            {
                return Result.Success(new ActivitiesSummaryDto
                {
                    TotalActivities = 0,
                    Completed = 0,
                    InProgress = 0,
                    Pending = 0,
                    Delayed = 0,
                    AverageProgress = 0,
                    Overdue = 0,
                    DueThisWeek = 0
                });
            }

            var summary = new ActivitiesSummaryDto
            {
                TotalActivities = kpiData.TotalActivities,
                Completed = kpiData.Completed,
                InProgress = kpiData.InProgress,
                Pending = kpiData.Pending,
                Delayed = kpiData.Delayed,
                AverageProgress = kpiData.AverageProgress,
                Overdue = kpiData.Overdue,
                DueThisWeek = kpiData.DueThisWeek
            };

            return Result.Success(summary);
        }
        catch (Exception ex)
        {
            return Result.Failure<ActivitiesSummaryDto>(
                $"Failed to generate activities summary: {ex.Message}");
        }
    }
}

