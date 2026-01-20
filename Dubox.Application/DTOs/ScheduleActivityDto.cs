namespace Dubox.Application.DTOs;

public record ScheduleActivityDto(
    Guid ScheduleActivityId,
    string ActivityName,
    string ActivityCode,
    string? Description,
    DateTime PlannedStartDate,
    DateTime PlannedFinishDate,
    DateTime? ActualStartDate,
    DateTime? ActualFinishDate,
    string Status,
    decimal PercentComplete,
    Guid? ProjectId,
    string? ProjectName,
    List<AssignedTeamDto> AssignedTeams,
    List<AssignedMaterialDto> AssignedMaterials
);

public record ScheduleActivityListDto(
    Guid ScheduleActivityId,
    string ActivityName,
    string ActivityCode,
    DateTime PlannedStartDate,
    DateTime PlannedFinishDate,
    string Status,
    decimal PercentComplete,
    int TeamCount,
    int MaterialCount
);

public record CreateScheduleActivityCommand(
    string ActivityName,
    string ActivityCode,
    string? Description,
    DateTime PlannedStartDate,
    DateTime PlannedFinishDate,
    Guid? ProjectId
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




