namespace Dubox.Application.DTOs;

public record DashboardStatisticsDto
{
    public int TotalProjects { get; init; }
    public int ActiveProjects { get; init; }
    public int TotalBoxes { get; init; }
    public int BoxesNotStarted { get; init; }
    public int BoxesInProgress { get; init; }
    public int BoxesCompleted { get; init; }
    public int BoxesDelayed { get; init; }
    public decimal OverallProgress { get; init; }
    public int PendingWIRs { get; init; }
    public int TotalActivities { get; init; }
    public int CompletedActivities { get; init; }
}

public record ProjectDashboardDto
{
    public Guid ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public int TotalBoxes { get; init; }
    public int BoxesNotStarted { get; init; }
    public int BoxesInProgress { get; init; }
    public int BoxesCompleted { get; init; }
    public decimal ProgressPercentage { get; init; }
    public int PendingWIRs { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public string Status { get; init; } = string.Empty;
}

public record BoxProgressSummaryDto
{
    public Guid BoxId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public string BoxType { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public decimal ProgressPercentage { get; init; }
    public int TotalActivities { get; init; }
    public int CompletedActivities { get; init; }
    public string? CurrentStage { get; init; }
    public DateTime? LastUpdateDate { get; init; }
}

