using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs
{

    public class WIRCheckpointDto
    {
        public Guid WIRId { get; set; }
        public Guid BoxId { get; set; }
        public Guid? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectCode { get; set; }
        public string? Client { get; set; }
        public Guid? BoxActivityId { get; set; }
        public string BoxName { get; set; } = string.Empty;
        public string BoxTag { get; set; } = string.Empty;

        public string WIRNumber { get; set; } = string.Empty;
        public string? WIRName { get; set; }
        public string? WIRDescription { get; set; }

        public DateTime? RequestedDate { get; set; }
        public string? RequestedBy { get; set; }

        public DateTime? InspectionDate { get; set; }
        public string? InspectorName { get; set; }
        public Guid? InspectorId { get; set; }

        public string? InspectorRole { get; set; }

        public WIRCheckpointStatusEnum Status { get; set; }
        public DateTime? ApprovalDate { get; set; }

        public string? Comments { get; set; }
        public string? AttachmentPath { get; set; }

        public DateTime CreatedDate { get; set; }
        
        // Versioning fields
        public int Version { get; set; } = 1;
        public Guid? ParentWIRId { get; set; }
        
        // Helper fields for version creation response
        public Guid? NewVersionId { get; set; }
        public int? NewVersionNumber { get; set; }

        // Calculated fields
        public int PendingDays { get; set; }
        public bool IsOverdue { get; set; }

        // Navigation DTOs
        public BoxDto? Box { get; set; }
        public List<WIRChecklistItemDto> ChecklistItems { get; set; } = new();
        public List<QualityIssueDto> QualityIssues { get; set; } = new();
        public List<WIRCheckpointImageDto> Images { get; set; } = new();
    }

    public record PaginatedWIRCheckpointsResponseDto : PaginatedResponse<WIRCheckpointDto>
    {
        public WIRCheckpointsSummary? Summary { get; init; }
    }

    /// <summary>
    /// Summary statistics for all WIR checkpoints (not just current page)
    /// </summary>
    public class WIRCheckpointsSummary
    {
        public int TotalCheckpoints { get; set; }
        public int PendingReviews { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int ConditionalApproval { get; set; }
    }

    /// <summary>
    /// Lightweight image info without base64 data - used in listings.
    /// Use /api/images/WIRCheckpoint/{WIRCheckpointImageId} to fetch full image.
    /// </summary>
    public class WIRCheckpointImageDto
    {
        public Guid WIRCheckpointImageId { get; set; }
        public Guid WIRId { get; set; }
        /// <summary>
        /// Base64 image data - only populated when fetching single image details.
        /// For listings, this will be null and ImageUrl should be used instead.
        /// </summary>
        public string? ImageData { get; set; }
        public string ImageType { get; set; } = "file";
        public string? OriginalName { get; set; }
        public long? FileSize { get; set; }
        public int Sequence { get; set; }
        public int Version { get; set; } = 1; // Version number for files with same name
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// URL to fetch full image: /api/images/WIRCheckpoint/{WIRCheckpointImageId}
        /// </summary>
        public string? ImageUrl { get; set; }
    }
    public class WIRChecklistItemDto
    {
        public Guid ChecklistItemId { get; set; }
        public Guid WIRId { get; set; }
        public string CheckpointDescription { get; set; } = string.Empty;
        public string? ReferenceDocument { get; set; }
        public string? Remarks { get; set; }
        public CheckListItemStatusEnum Status { get; set; }
        public int Sequence { get; set; }
        public Guid? PredefinedItemId { get; set; } // Reference to the predefined item this was cloned from
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        // Section information
        public Guid? SectionId { get; set; }
        public string? SectionName { get; set; }
        public int? SectionOrder { get; set; }

        // Checklist information
        public Guid? ChecklistId { get; set; }
        public string? ChecklistName { get; set; }
        public string? ChecklistCode { get; set; }
    }
    public class QualityIssueDto
    {
        public Guid IssueId { get; set; }
        public string IssueType { get; set; } = string.Empty;
        public string? IssueDescription { get; set; }
        public string? IssueNumber { get; set; }

        public string? CcUserName { get; set; }
        public string? AssignedUserName { get; set; }

        public string? AssignedTeam { get; set; }
        public string? Severity { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? ReportedBy { get; set; }
        public QualityIssueStatusEnum Status { get; set; }
        public List<QualityIssueImageDto> Images { get; set; } = new();
        /// <summary>
        /// Number of images (useful when Images list is empty but images exist)
        /// </summary>
        public int ImageCount { get; set; }
    }

    public class PredefinedChecklistItemDto
    {
        public Guid PredefinedItemId { get; set; }
        public string WIRNumber { get; set; } = string.Empty;
        public string? ItemNumber { get; set; }
        public string CheckpointDescription { get; set; } = string.Empty;
        public Guid? CategoryId { get; set; }
        public string? Category { get; set; } // CategoryName
        public string? CategoryName { get; set; } // For consistency
        public Guid? ReferenceId { get; set; }
        public string? ReferenceDocument { get; set; } // ReferenceName
        public string? ReferenceName { get; set; } // For consistency
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreateWIRCheckpointDto
    {
        public Guid WIRId { get; set; }
        public Guid BoxId { get; set; }

        public string WIRNumber { get; set; } = string.Empty;
        public string? WIRName { get; set; }
        public string? WIRDescription { get; set; }

        public DateTime? RequestedDate { get; set; }
        public string? RequestedBy { get; set; }
        public string? Comments { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
