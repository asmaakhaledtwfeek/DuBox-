using Dubox.Application.Features.Reports.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class TeamsPerformanceReportSpecification : Specification<Team>
{
    public TeamsPerformanceReportSpecification(GetTeamsPerformanceReportQuery query, List<Guid>? accessibleTeamIds)
        : this(query.TeamId, query.Search, accessibleTeamIds)
    {
    }

    public TeamsPerformanceReportSpecification(ExportTeamsPerformanceReportQuery query, List<Guid>? accessibleTeamIds)
        : this(query.TeamId, query.Search, accessibleTeamIds)
    {
    }

    public TeamsPerformanceReportSpecification(GetTeamsPerformanceSummaryQuery query, List<Guid>? accessibleTeamIds)
        : this(query.TeamId, query.Search, accessibleTeamIds)
    {
    }

    private TeamsPerformanceReportSpecification(Guid? teamId, string? search, List<Guid>? accessibleTeamIds)
    {
        // Include necessary navigation properties
        AddInclude(nameof(Team.Members));
        AddInclude(nameof(Team.AssignedActivities));
        AddInclude($"{nameof(Team.AssignedActivities)}.{nameof(BoxActivity.Box)}");
        AddInclude($"{nameof(Team.AssignedActivities)}.{nameof(BoxActivity.Box)}.{nameof(Box.Project)}");
        
        // Enable split query to avoid Cartesian explosion with collection includes
        EnableSplitQuery();

        // Base criteria: only active teams
        AddCriteria(t => t.IsActive);

        // Apply team visibility filtering
        if (accessibleTeamIds != null)
        {
            AddCriteria(t => accessibleTeamIds.Contains(t.TeamId));
        }

        // Apply team filter
        if (teamId.HasValue && teamId.Value != Guid.Empty)
        {
            AddCriteria(t => t.TeamId == teamId.Value);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim().ToLowerInvariant();
            AddCriteria(t => t.TeamName.ToLower().Contains(searchTerm));
        }
    }
}

