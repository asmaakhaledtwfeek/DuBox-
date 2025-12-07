using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs;

/// <summary>
/// Filter DTO for Activities Report
/// </summary>
public record ActivitiesFilterDto : PaginatedRequest
{
    public Guid? ProjectId { get; init; }
    public Guid? TeamId { get; init; }
    public BoxStatusEnum? Status { get; init; }
    public DateTime? PlannedStartDateFrom { get; init; }
    public DateTime? PlannedStartDateTo { get; init; }
    public DateTime? PlannedEndDateFrom { get; init; }
    public DateTime? PlannedEndDateTo { get; init; }
    public string? Search { get; init; }
}

/// <summary>
/// Lightweight DTO for Activity Report - optimized for reporting
/// </summary>
public record ReportActivityDto
{
    public Guid ActivityId { get; init; }
    public string ActivityName { get; init; } = string.Empty;
    public string BoxTag { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string? AssignedTeam { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal ProgressPercentage { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualStartDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public int? ActualDuration { get; init; }
    public int? DelayDays { get; init; }
    public Guid BoxId { get; init; }
    public Guid ProjectId { get; init; }
}

/// <summary>
/// Paginated response for Activities Report
/// </summary>
public record PaginatedActivitiesReportResponseDto : PaginatedResponse<ReportActivityDto>
{
    // Inherits all properties from PaginatedResponse<ReportActivityDto>
}

/// <summary>
/// KPIs/Summary DTO for Activities Report
/// </summary>
public record ActivitiesSummaryDto
{
    public int TotalActivities { get; init; }
    public int Completed { get; init; }
    public int InProgress { get; init; }
    public int Pending { get; init; }
    public int Delayed { get; init; }
    public decimal AverageProgress { get; init; }
    public int Overdue { get; init; }
    public int DueThisWeek { get; init; }
}

