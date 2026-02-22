namespace Dubox.Application.DTOs;

public record ScheduleActivityDto(
    Guid ScheduleActivityId,
    Guid? SourceActivityMasterId,
    bool IsCustomActivity,
    string ActivityCode,
    string ActivityName,
    string Stage,
    int StageNumber,
    int SequenceInStage,
    int OverallSequence,
    string? Description,
    int EstimatedDurationDays,
    bool IsWIRCheckpoint,
    string? WIRCode,
    string? ApplicableBoxTypes,
    string? DependsOnActivities,
    DateTime PlannedStartDate,
    DateTime PlannedFinishDate,
    DateTime? ActualStartDate,
    DateTime? ActualFinishDate,
    string Status,
    decimal PercentComplete,
    decimal Weight,
    Guid? ProjectId,
    string? ProjectName,
    List<AssignedTeamDto> AssignedTeams,
    List<AssignedMaterialDto> AssignedMaterials
);

public record ScheduleActivityListDto(
    Guid ScheduleActivityId,
    string ActivityCode,
    string ActivityName,
    string Stage,
    int StageNumber,
    bool IsCustomActivity,
    DateTime PlannedStartDate,
    DateTime PlannedFinishDate,
    DateTime? ActualStartDate,
    DateTime? ActualFinishDate,
    string Status,
    decimal PercentComplete,
    decimal Weight,
    int TeamCount,
    int MaterialCount,
    Guid? ParentActivityId,
    List<ScheduleActivityListDto> Children
);

public record CreateScheduleActivityCommand(
    Guid? SourceActivityMasterId,
    bool IsCustomActivity,
    string ActivityCode,
    string ActivityName,
    string Stage,
    int StageNumber,
    int SequenceInStage,
    int OverallSequence,
    string? Description,
    int EstimatedDurationDays,
    bool IsWIRCheckpoint,
    string? WIRCode,
    string? ApplicableBoxTypes,
    string? DependsOnActivities,
    DateTime PlannedStartDate,
    DateTime PlannedFinishDate,
    Guid? ProjectId,
     DateTime ActualFinishDate,
    DateTime ActualStartDate
);

public record AssignedTeamDto(
    Guid ScheduleActivityTeamId,
    Guid TeamId,
    string TeamName,
    DateTime AssignedDate,
    string? Notes
);

public record AssignedMaterialDto(
    Guid ScheduleActivityMaterialId,
    string MaterialName,
    string? MaterialCode,
    decimal Quantity,
    string? Unit,
    string? Notes
);

public record AssignTeamCommand(
    Guid ScheduleActivityId,
    Guid TeamId,
    string? Notes
);

public record AssignMaterialCommand(
    Guid ScheduleActivityId,
    string MaterialName,
    string? MaterialCode,
    decimal Quantity,
    string? Unit,
    string? Notes
);















