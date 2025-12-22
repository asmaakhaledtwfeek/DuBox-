using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs
{
    public class QualityIssueDetailsDto
    {
        public Guid IssueId { get; set; }
        public DateTime IssueDate { get; set; }
        public IssueTypeEnum? IssueType { get; set; }
        public SeverityEnum? Severity { get; set; }
        public string? IssueDescription { get; set; }
        public string? ReportedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public string? AssignedTeamName { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public DateTime? DueDate { get; set; }
        public QualityIssueStatusEnum Status { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string? ResolutionDescription { get; set; }
        [Obsolete("Use Images list instead. Kept for backward compatibility.")]
        public string? PhotoPath { get; set; }
        public List<QualityIssueImageDto> Images { get; set; } = new();

        public bool IsOverdue { get; set; }
        public int OverdueDays { get; set; }

        public Guid BoxId { get; set; }
        public string BoxName { get; set; } = string.Empty;
        public string BoxTag { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public Guid? WIRId { get; set; }
        public string? WIRNumber { get; set; }
        public string? WIRName { get; set; }
        public WIRCheckpointStatusEnum? WIRStatus { get; set; }
        public DateTime? WIRRequestedDate { get; set; }
        public string? InspectorName { get; set; }
    }

    public record PaginatedQualityIssuesResponseDto : PaginatedResponse<QualityIssueDetailsDto>;

    /// <summary>
    /// Quality issue image info - ImageData is only populated in detail views.
    /// Use /api/images/QualityIssue/{QualityIssueImageId} to fetch full image.
    /// </summary>
    public class QualityIssueImageDto
    {
        public Guid QualityIssueImageId { get; set; }
        public Guid IssueId { get; set; }
        /// <summary>
        /// Base64 image data - only populated when fetching single image details.
        /// For listings, this will be null and ImageUrl should be used instead.
        /// </summary>
        public string? ImageData { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public string? OriginalName { get; set; }
        public long? FileSize { get; set; }
        public int Sequence { get; set; }
        public int Version { get; set; } = 1; // Version number for files with same name
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// URL to fetch full image: /api/images/QualityIssue/{QualityIssueImageId}
        /// </summary>
        public string? ImageUrl { get; set; }
    }

}
