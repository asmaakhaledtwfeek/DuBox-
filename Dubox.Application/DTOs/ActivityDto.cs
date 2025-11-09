namespace Dubox.Application.DTOs;

public record ActivityMasterDto
{
    public Guid ActivityMasterId { get; init; }
    public string ActivityCode { get; init; } = string.Empty;
    public string ActivityName { get; init; } = string.Empty;
    public string Stage { get; init; } = string.Empty;
    public int StageNumber { get; init; }
    public int SequenceInStage { get; init; }
    public int OverallSequence { get; init; }
    public string? Description { get; init; }
    public int EstimatedDurationDays { get; init; }
    public bool IsWIRCheckpoint { get; init; }
    public string? WIRCode { get; init; }
    public string? ApplicableBoxTypes { get; init; }
    public bool IsActive { get; init; }
}

public record BoxActivityDto
{
    public Guid BoxActivityId { get; init; }
    public Guid BoxId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public Guid ActivityMasterId { get; init; }
    public string ActivityCode { get; init; } = string.Empty;
    public string ActivityName { get; init; } = string.Empty;
    public string Stage { get; init; } = string.Empty;
    public int Sequence { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal ProgressPercentage { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualStartDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public string? WorkDescription { get; init; }
    public string? IssuesEncountered { get; init; }
    public string? AssignedTeam { get; init; }
    public bool MaterialsAvailable { get; init; }
    public bool IsWIRCheckpoint { get; init; }
    public string? WIRCode { get; init; }
}

public record UpdateBoxActivityDto
{
    public Guid BoxActivityId { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal ProgressPercentage { get; init; }
    public string? WorkDescription { get; init; }
    public string? IssuesEncountered { get; init; }
    public string? AssignedTeam { get; init; }
    public bool MaterialsAvailable { get; init; }
}

