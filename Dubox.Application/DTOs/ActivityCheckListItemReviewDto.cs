using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs;

/// <summary>
/// DTO for Activity Checklist Item with its review status
/// </summary>
public record ActivityCheckListItemWithReviewDto(
    Guid ActivityCheckListItemId,
    Guid? ActivityMasterId,
    Guid? ActivityTemplateActivityId,
    Guid PredefinedChecklistItemId,
    string CheckpointDescription,
    string? Reference,
    int Sequence,
    bool IsMandatory,
    bool IsActive,
    // Grouping information
    Guid? ChecklistSectionId,
    string? SectionTitle,
    int? SectionOrder,
    Guid? ChecklistId,
    string? ChecklistName,
    string? ChecklistCode,
    // Review information
    Guid? ReviewId,
    CheckListItemStatusEnum ReviewStatus,
    string? Remarks,
    Guid? ReviewedBy,
    string? ReviewedByName,
    DateTime? ReviewedDate
);

/// <summary>
/// DTO for creating or updating a review
/// </summary>
public record ReviewActivityCheckListItemDto(
    Guid BoxActivityId,
    Guid ActivityCheckListItemId,
    CheckListItemStatusEnum Status,
    string? Remarks
);

/// <summary>
/// Response DTO after submitting all reviews for an activity
/// </summary>
public record SubmitActivityChecklistReviewDto(
    Guid BoxActivityId,
    List<ReviewActivityCheckListItemDto> Reviews
);

/// <summary>
/// DTO for getting checklist items by BoxActivity
/// </summary>
public record GetActivityChecklistByBoxActivityDto(
    Guid BoxActivityId,
    string ActivityName,
    string ActivityCode,
    decimal ProgressPercentage,
    List<ActivityCheckListItemWithReviewDto> ChecklistItems
);

/// <summary>
/// DTO for activity review summary (counts)
/// </summary>
public record ActivityReviewsSummaryDto(
    int TotalReviews,
    int PendingReviews,
    int PassedReviews,
    int FailedReviews,
    int NaReviews
);

/// <summary>
/// DTO for individual activity review item in the list
/// </summary>
public record ActivityReviewItemDto(
    Guid BoxActivityId,
    string ActivityName,
    string ActivityCode,
    Guid BoxId,
    string BoxTag,
    Guid ProjectId,
    string ProjectCode,
    string ProjectName,
    int TotalItems,
    int ReviewedItems,
    int PendingItems,
    int PassedItems,
    int FailedItems,
    string ReviewStatus, // "Pending", "Completed", "Partial"
    DateTime? LastReviewedDate,
    string? LastReviewedBy,
    decimal ProgressPercentage
);

/// <summary>
/// DTO for paginated activity reviews response
/// </summary>
public record PaginatedActivityReviewsDto(
    List<ActivityReviewItemDto> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);





