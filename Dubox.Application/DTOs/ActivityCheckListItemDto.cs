namespace Dubox.Application.DTOs;

public record ActivityCheckListItemDto(
    Guid ActivityCheckListItemId,
    Guid? ActivityMasterId,
    Guid? ActivityTemplateActivityId,
    Guid PredefinedChecklistItemId,
    int Sequence,
    bool IsMandatory,
    bool IsActive,
    DateTime CreatedDate,
    string? CreatedBy
);

public record ActivityCheckListItemDetailsDto(
    Guid ActivityCheckListItemId,
    Guid? ActivityMasterId,
    string? ActivityMasterName,
    string? ActivityMasterCode,
    Guid? ActivityTemplateActivityId,
    string? ActivityTemplateActivityName,
    string? ActivityTemplateActivityCode,
    Guid PredefinedChecklistItemId,
    string CheckpointDescription,
    string? Reference,
    int Sequence,
    bool IsMandatory,
    bool IsActive,
    DateTime CreatedDate,
    string? CreatedBy,
    DateTime? ModifiedDate,
    string? ModifiedBy
);

public record CreateActivityCheckListItemCommand(
    Guid? ActivityMasterId,
    Guid? ActivityTemplateActivityId,
    Guid PredefinedChecklistItemId,
    int Sequence,
    bool IsMandatory = true,
    bool IsActive = true
);

public record UpdateActivityCheckListItemCommand(
    Guid ActivityCheckListItemId,
    int Sequence,
    bool IsMandatory,
    bool IsActive
);

public record BulkCreateActivityCheckListItemsCommand(
    Guid? ActivityMasterId,
    Guid? ActivityTemplateActivityId,
    List<ActivityCheckListItemData> ChecklistItems
);

public record ActivityCheckListItemData(
    Guid PredefinedChecklistItemId,
    int Sequence,
    bool IsMandatory = true
);
