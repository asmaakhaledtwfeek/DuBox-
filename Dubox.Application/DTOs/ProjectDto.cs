namespace Dubox.Application.DTOs;

public record ProjectDto
{
    public Guid ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string? ClientName { get; init; }
    public string? Location { get; init; }
    public int? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public int? Duration { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public DateTime? CompressionStartDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int TotalBoxes { get; init; }
    public decimal ProgressPercentage { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public string? CreatedBy { get; init; }
}

public record CreateProjectDto
{
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string? ClientName { get; init; }
    public string? Location { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public string? Description { get; init; }
}

public record UpdateProjectDto
{
    public Guid ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string? ClientName { get; init; }
    public string? Location { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}

