namespace Dubox.Application.DTOs;

public record TeamDto
{
    public int TeamId { get; init; }
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public string DepartmentName { get; init; } = string.Empty;
    public string? Trade { get; init; }
    public string? TeamLeaderName { get; init; }
    public int? TeamSize { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
}

public record CreateTeamDto
{
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public string? Department { get; init; }
    public string? Trade { get; init; }
    public string? TeamLeaderName { get; init; }
    public int? TeamSize { get; init; }
}

public record UpdateTeamDto
{
    public int TeamId { get; init; }
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public string? Department { get; init; }
    public string? Trade { get; init; }
    public string? TeamLeaderName { get; init; }
    public int? TeamSize { get; init; }
    public bool IsActive { get; init; }
}

public record TeamProductivityDto
{
    public int TeamId { get; init; }
    public string TeamName { get; init; } = string.Empty;
    public int TotalActivitiesAssigned { get; init; }
    public int CompletedActivities { get; init; }
    public int InProgressActivities { get; init; }
    public decimal AverageProgressPercentage { get; init; }
    public int TotalUpdatesToday { get; init; }
    public int TotalBoxesWorkedOn { get; init; }
}

