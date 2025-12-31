using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Application.Utilities;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetActivitiesReportQueryHandler : IRequestHandler<GetActivitiesReportQuery, Result<PaginatedActivitiesReportResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetActivitiesReportQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<PaginatedActivitiesReportResponseDto>> Handle(GetActivitiesReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            var (page, pageSize) = new PaginatedRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            }.GetNormalizedPagination();

            var paginatedResult = _unitOfWork.Repository<Domain.Entities.BoxActivity>().GetWithSpec(new ActivitiesReportSpecification(request, accessibleProjectIds, enablePaging: true));

            var countSpec = new ActivitiesReportSpecification(request, accessibleProjectIds, enablePaging: false);
            var countResult = _unitOfWork.Repository<Domain.Entities.BoxActivity>().GetWithSpec(countSpec);
            var totalCount = countResult.Count > 0
                ? countResult.Count
                : await countResult.Data.AsNoTracking().CountAsync(cancellationToken);

            var activities = await paginatedResult.Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var items = activities.Adapt<List<ReportActivityDto>>();
            
            // Calculate delay days and formatted delay for each activity
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var activity = activities.FirstOrDefault(a => a.BoxActivityId == item.ActivityId);
                if (activity != null && activity.Duration.HasValue && activity.ActualStartDate.HasValue && activity.ActualEndDate.HasValue)
                {
                    var durationValues = DurationFormatter.CalculateDurationValues(activity.ActualStartDate, activity.ActualEndDate);
                    if (durationValues != null)
                    {
                        var plannedHours = activity.Duration.Value * 24.0;
                        var actualHours = durationValues.TotalHours;
                        var delayHours = actualHours - plannedHours;
                        
                        if (delayHours > 0)
                        {
                            var delayDays = (int)Math.Floor(delayHours / 24.0);
                            var remainingHours = (int)Math.Round(delayHours % 24);
                            
                            // If remaining hours is 24, it means it's a full extra day
                            if (remainingHours == 24)
                            {
                                delayDays++;
                                remainingHours = 0;
                            }
                            
                            // Format delay: "X days Y hours" or just "X days" if no hours
                            string? delayFormatted = null;
                            if (remainingHours == 0)
                            {
                                delayFormatted = delayDays == 1 ? "1 day" : $"{delayDays} days";
                            }
                            else
                            {
                                var daysText = delayDays == 1 ? "1 day" : $"{delayDays} days";
                                var hoursText = remainingHours == 1 ? "1 hour" : $"{remainingHours} hours";
                                delayFormatted = $"{daysText} {hoursText}";
                            }
                            
                            items[i] = item with { DelayDays = delayDays, DelayDaysFormatted = delayFormatted };
                        }
                    }
                }
            }

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PaginatedActivitiesReportResponseDto
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatedActivitiesReportResponseDto>(
                $"Failed to generate activities report: {ex.Message}");
        }
    }
}

