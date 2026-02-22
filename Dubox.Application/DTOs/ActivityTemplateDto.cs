namespace Dubox.Application.DTOs;

public record ActivityTemplateDto(
    Guid ActivityTemplateId,
    string TemplateName,
    string? Description,
    int StageCount,
    bool IsActive,
    int ActivityCount,
    DateTime CreatedDate,
    string? CreatedBy
);

public record ActivityTemplateDetailsDto(
    Guid ActivityTemplateId,
    string TemplateName,
    string? Description,
    int StageCount,
    bool IsActive,
    List<ActivityTemplateActivityDto> Activities,
    DateTime CreatedDate,
    string? CreatedBy,
    DateTime? ModifiedDate,
    string? ModifiedBy
);

public record ActivityTemplateActivityDto(
    Guid ActivityTemplateActivityId,
    Guid ActivityTemplateId,
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
    Guid? AssignedTeamId,
    List<ActivityChecklistItemDto>? SelectedChecklistItems
);

public record ActivityChecklistItemDto(
    Guid PredefinedChecklistItemId,
    int Sequence,
    bool IsMandatory
);

public record CreateActivityTemplateCommand(
    string TemplateName,
    string? Description,
    List<CreateActivityTemplateActivityCommand> Activities
);

public record CreateActivityTemplateActivityCommand(
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
    List<ActivityChecklistItemDto>? SelectedChecklistItems
);

public record UpdateActivityTemplateCommand(
    Guid ActivityTemplateId,
    string TemplateName,
    string? Description,
    bool IsActive
);

public record AddActivityToTemplateCommand(
    Guid ActivityTemplateId,
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
    List<ActivityChecklistItemDto>? SelectedChecklistItems
);

public record RemoveActivityFromTemplateCommand(
    Guid ActivityTemplateId,
    Guid ActivityTemplateActivityId
);
