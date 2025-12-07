namespace Dubox.Application.DTOs;

/// <summary>
/// DTO for Team Performance Item in Teams Performance Report
/// </summary>
public record TeamPerformanceItemDto
{
    public Guid TeamId { get; init; }
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public int MembersCount { get; init; }
    public int TotalAssignedActivities { get; init; }
    public int Completed { get; init; }
    public int InProgress { get; init; }
    public int Pending { get; init; }
    public int Delayed { get; init; }
    public decimal AverageTeamProgress { get; init; }
    public string WorkloadLevel { get; init; } = "Normal"; // Low, Normal, Overloaded
}

/// <summary>
/// Paginated response for Teams Performance Report
/// </summary>
public record PaginatedTeamsPerformanceResponseDto : PaginatedResponse<TeamPerformanceItemDto>
{
    // Inherits all properties from PaginatedResponse<TeamPerformanceItemDto>
}

/// <summary>
/// Summary/KPIs DTO for Teams Performance Report
/// </summary>
public record TeamsPerformanceSummaryDto
{
    public int TotalTeams { get; init; }
    public int TotalTeamMembers { get; init; }
    public int TotalAssignedActivities { get; init; }
    public int CompletedActivities { get; init; }
    public int InProgressActivities { get; init; }
    public int DelayedActivities { get; init; }
    public decimal AverageTeamProgress { get; init; }
    public decimal TeamWorkloadIndicator { get; init; } // activities per team
}

/// <summary>
/// DTO for Team Activity Detail (for drill-down)
/// </summary>
public record TeamActivityDetailDto
{
    public Guid ActivityId { get; init; }
    public string ActivityName { get; init; } = string.Empty;
    public string BoxTag { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public decimal ProgressPercentage { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualStartDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public int? Duration { get; init; }
    public Guid BoxId { get; init; }
    public Guid ProjectId { get; init; }
}

/// <summary>
/// Response DTO for Team Activities (drill-down)
/// </summary>
public record TeamActivitiesResponseDto
{
    public Guid TeamId { get; init; }
    public string TeamName { get; init; } = string.Empty;
    public List<TeamActivityDetailDto> Activities { get; init; } = new();
    public int TotalCount { get; init; }
}

