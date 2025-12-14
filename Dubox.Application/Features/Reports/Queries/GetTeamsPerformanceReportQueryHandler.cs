using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetTeamsPerformanceReportQueryHandler : IRequestHandler<GetTeamsPerformanceReportQuery, Result<PaginatedTeamsPerformanceResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetTeamsPerformanceReportQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<PaginatedTeamsPerformanceResponseDto>> Handle(GetTeamsPerformanceReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            var accessibleTeamIds = await _visibilityService.GetAccessibleTeamIdsAsync(cancellationToken);

            var (page, pageSize) = new PaginatedRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            }.GetNormalizedPagination();

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

            var teamPerformanceItems = new List<TeamPerformanceItemDto>();

            foreach (var team in teams)
            {
                var teamActivities = activitiesByTeamId.TryGetValue(team.TeamId, out var activities)
                    ? activities
                    : new List<Domain.Entities.BoxActivity>();

                if (teamActivities.Count == 0 && (request.ProjectId.HasValue || request.Status.HasValue))
                    continue;

                var completed = teamActivities.Count(ba => ba.Status == BoxStatusEnum.Completed);
                var inProgress = teamActivities.Count(ba => ba.Status == BoxStatusEnum.InProgress);
                var pending = teamActivities.Count(ba => ba.Status == BoxStatusEnum.NotStarted);
                var delayed = teamActivities.Count(ba => ba.Status == BoxStatusEnum.Delayed);
                var averageProgress = teamActivities.Any()
                    ? teamActivities.Average(ba => ba.ProgressPercentage)
                    : 0;

                var membersCount = team.Members.Count(m => m.IsActive);
                var activitiesPerMember = membersCount > 0 ? (double)teamActivities.Count / membersCount : 0;
                var workloadLevel = activitiesPerMember < 3 ? "Low" : activitiesPerMember > 7 ? "Overloaded" : "Normal";

                teamPerformanceItems.Add(new TeamPerformanceItemDto
                {
                    TeamId = team.TeamId,
                    TeamCode = team.TeamCode,
                    TeamName = team.TeamName,
                    MembersCount = membersCount,
                    TotalAssignedActivities = teamActivities.Count,
                    Completed = completed,
                    InProgress = inProgress,
                    Pending = pending,
                    Delayed = delayed,
                    AverageTeamProgress = averageProgress,
                    WorkloadLevel = workloadLevel
                });
            }

            var totalCount = teamPerformanceItems.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var paginatedItems = teamPerformanceItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new PaginatedTeamsPerformanceResponseDto
            {
                Items = paginatedItems,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatedTeamsPerformanceResponseDto>(
                $"Failed to generate teams performance report: {ex.Message}");
        }
    }
}

