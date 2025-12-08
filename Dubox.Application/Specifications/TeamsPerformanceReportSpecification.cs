using Dubox.Application.Features.Reports.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class TeamsPerformanceReportSpecification : Specification<Team>
{
    public TeamsPerformanceReportSpecification(GetTeamsPerformanceReportQuery query)
        : this(query.TeamId, query.Search)
    {
    }

    public TeamsPerformanceReportSpecification(ExportTeamsPerformanceReportQuery query)
        : this(query.TeamId, query.Search)
    {
    }

    public TeamsPerformanceReportSpecification(GetTeamsPerformanceSummaryQuery query)
        : this(query.TeamId, query.Search)
    {
    }

    private TeamsPerformanceReportSpecification(Guid? teamId, string? search)
    {
        // Include necessary navigation properties
        AddInclude(nameof(Team.Members));
        AddInclude(nameof(Team.AssignedActivities));
        AddInclude($"{nameof(Team.AssignedActivities)}.{nameof(BoxActivity.Box)}");
        AddInclude($"{nameof(Team.AssignedActivities)}.{nameof(BoxActivity.Box)}.{nameof(Box.Project)}");
        
        // Enable split query to avoid Cartesian explosion with collection includes
        

        // Base criteria: only active teams
        AddCriteria(t => t.IsActive);

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

