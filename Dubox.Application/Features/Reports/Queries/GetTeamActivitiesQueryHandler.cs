using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetTeamActivitiesQueryHandler : IRequestHandler<GetTeamActivitiesQuery, Result<TeamActivitiesResponseDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetTeamActivitiesQueryHandler(IDbContext dbContext, IProjectTeamVisibilityService visibilityService)
    {
        _dbContext = dbContext;
        _visibilityService = visibilityService;
    }

    public async Task<Result<TeamActivitiesResponseDto>> Handle(GetTeamActivitiesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user has access to this team
            var canAccessTeam = await _visibilityService.CanAccessTeamAsync(request.TeamId, cancellationToken);
            if (!canAccessTeam)
            {
                return Result.Failure<TeamActivitiesResponseDto>("Access denied. You do not have permission to view this team's activities.");
            }

            // Get accessible project IDs for filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            // Get team
            var team = await _dbContext.Teams
                .AsNoTracking()
                .Where(t => t.TeamId == request.TeamId && t.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (team == null)
            {
                return Result.Failure<TeamActivitiesResponseDto>("Team not found");
            }

            // Get activities for this team
            var activitiesQuery = _dbContext.BoxActivities
                .AsNoTracking()
                .Where(ba => ba.TeamId == request.TeamId && ba.IsActive)
                .Include(ba => ba.Box)
                    .ThenInclude(b => b.Project)
                .Include(ba => ba.ActivityMaster)
                .AsQueryable();

            // Apply project visibility filter
            if (accessibleProjectIds != null)
            {
                activitiesQuery = activitiesQuery.Where(ba => accessibleProjectIds.Contains(ba.Box.ProjectId));
            }

            // Apply project filter
            if (request.ProjectId.HasValue && request.ProjectId.Value != Guid.Empty)
            {
                activitiesQuery = activitiesQuery.Where(ba => ba.Box.ProjectId == request.ProjectId.Value);
            }

            // Apply status filter
            if (request.Status.HasValue)
            {
                var statusEnum = (BoxStatusEnum)request.Status.Value;
                activitiesQuery = activitiesQuery.Where(ba => ba.Status == statusEnum);
            }

            var activities = await activitiesQuery
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var activityDtos = activities.Select(ba => new TeamActivityDetailDto
            {
                ActivityId = ba.BoxActivityId,
                ActivityName = ba.ActivityMaster.ActivityName,
                BoxTag = ba.Box.BoxTag,
                ProjectName = ba.Box.Project.ProjectName,
                Status = ba.Status.ToString(),
                ProgressPercentage = ba.ProgressPercentage,
                PlannedStartDate = ba.PlannedStartDate,
                PlannedEndDate = ba.PlannedEndDate,
                ActualStartDate = ba.ActualStartDate,
                ActualEndDate = ba.ActualEndDate,
                Duration = ba.Duration ?? (ba.PlannedStartDate.HasValue && ba.PlannedEndDate.HasValue
                    ? (int?)(ba.PlannedEndDate.Value - ba.PlannedStartDate.Value).Days
                    : null),
                BoxId = ba.BoxId,
                ProjectId = ba.Box.ProjectId
            }).ToList();

            var response = new TeamActivitiesResponseDto
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Activities = activityDtos,
                TotalCount = activityDtos.Count
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<TeamActivitiesResponseDto>(
                $"Failed to get team activities: {ex.Message}");
        }
    }
}

