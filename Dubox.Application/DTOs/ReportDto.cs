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
/// DTO for Projects Summary Report - aggregated information about all projects
/// </summary>
public record ProjectsSummaryReportResponseDto
{
    public List<ProjectSummaryItemDto> Items { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public ProjectsSummaryReportKpisDto Kpis { get; init; } = new();
    public Dictionary<string, int> StatusDistribution { get; init; } = new();
}

/// <summary>
/// DTO for Projects Summary Report KPIs
/// </summary>
public record ProjectsSummaryReportKpisDto
{
    public int TotalProjects { get; init; }
    public int ActiveProjects { get; init; }
    public int InactiveProjects { get; init; }
    public int TotalBoxes { get; init; }
    public decimal AverageProgressPercentage { get; init; }
    public string AverageProgressPercentageFormatted { get; init; } = string.Empty;
}

/// <summary>
/// DTO for individual project in the summary report
/// </summary>
public record ProjectSummaryItemDto
{
    public Guid ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string? ClientName { get; init; }
    public string? Location { get; init; }
    public string Status { get; init; } = string.Empty;
    public int TotalBoxes { get; init; }
    public decimal ProgressPercentage { get; init; }
    public string ProgressPercentageFormatted { get; init; } = string.Empty;
    public bool IsActive { get; init; }
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

/// <summary>
/// DTO for Box Summary Report - individual box entry in the summary report
/// </summary>
public record BoxSummaryReportItemDto
{
    public Guid BoxId { get; init; }
    public Guid ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string BoxTag { get; init; } = string.Empty;
    public string? SerialNumber { get; init; }
    public string? BoxName { get; init; }
    public string BoxType { get; init; } = string.Empty;
    public string? Floor { get; init; }
    public string? Building { get; init; }
    public string? Zone { get; init; }
    public decimal ProgressPercentage { get; init; }
    public string ProgressPercentageFormatted { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public Guid? CurrentLocationId { get; init; }
    public string? CurrentLocationName { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualStartDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public int? Duration { get; init; }
    public DateTime? LastUpdateDate { get; init; }
    public int ActivitiesCount { get; init; }
    public int AssetsCount { get; init; }
    public int QualityIssuesCount { get; init; }
}

/// <summary>
/// DTO for Box Summary Report KPIs
/// </summary>
public record BoxSummaryReportKpisDto
{
    public int TotalBoxes { get; init; }
    public int InProgressCount { get; init; }
    public int CompletedCount { get; init; }
    public int NotStartedCount { get; init; }
    public decimal AverageProgress { get; init; }
    public string AverageProgressFormatted { get; init; } = string.Empty;
}

/// <summary>
/// DTO for Box Summary Report aggregations
/// </summary>
public record BoxSummaryReportAggregationsDto
{
    public Dictionary<string, int> StatusDistribution { get; init; } = new();
    public Dictionary<string, int> ProgressRangeDistribution { get; init; } = new();
    public List<ProjectBoxCountDto> TopProjects { get; init; } = new();
}

/// <summary>
/// DTO for project box count (for top projects chart)
/// </summary>
public record ProjectBoxCountDto
{
    public Guid ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public int BoxCount { get; init; }
}

/// <summary>
/// DTO for paginated Box Summary Report response
/// </summary>
public record PaginatedBoxSummaryReportResponseDto
{
    public List<BoxSummaryReportItemDto> Items { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public BoxSummaryReportKpisDto Kpis { get; init; } = new();
    public BoxSummaryReportAggregationsDto Aggregations { get; init; } = new();
}

