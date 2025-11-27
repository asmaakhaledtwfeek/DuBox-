namespace Dubox.Application.DTOs;

/// <summary>
/// DTO for Box Progress Report - shows progress distribution across buildings
/// </summary>
public record BoxProgressReportDto
{
    public string Building { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public int NonAssembled { get; set; }
    public int Backing { get; set; }
    public int Released1stFix { get; set; }
    public int Released2ndFix { get; set; }
    public int Released3rdFix { get; set; }
    public int Total { get; set; }
    public decimal ProgressPercentage { get; set; }
}

/// <summary>
/// DTO for Team Productivity Report - shows team performance metrics
/// </summary>
public record TeamProductivityReportDto
{
    public string TeamId { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public int TotalActivities { get; set; }
    public int CompletedActivities { get; set; }
    public int InProgress { get; set; }
    public int Pending { get; set; }
    public decimal AverageCompletionTime { get; set; } // in days
    public decimal Efficiency { get; set; } // percentage
}

/// <summary>
/// DTO for Report Summary - dashboard statistics
/// </summary>
public record ReportSummaryDto
{
    public int TotalBoxes { get; set; }
    public decimal AverageProgress { get; set; }
    public int PendingActivities { get; set; }
    public int ActiveTeams { get; set; }
    public int TotalProjects { get; set; }
    public int CompletedActivities { get; set; }
}

/// <summary>
/// DTO for Phase Readiness Report (future implementation)
/// </summary>
public record PhaseReadinessReportDto
{
    public string PhaseName { get; set; } = string.Empty;
    public int TotalBoxes { get; set; }
    public int ReadyBoxes { get; set; }
    public int PendingBoxes { get; set; }
    public decimal ReadinessPercentage { get; set; }
    public List<string> BlockingIssues { get; set; } = new();
}

/// <summary>
/// DTO for Missing Materials Report (future implementation)
/// </summary>
public record MissingMaterialsReportDto
{
    public string MaterialName { get; set; } = string.Empty;
    public string MaterialCode { get; set; } = string.Empty;
    public int RequiredQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public int ShortageQuantity { get; set; }
    public int AffectedBoxes { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime? ExpectedDeliveryDate { get; set; }
}

