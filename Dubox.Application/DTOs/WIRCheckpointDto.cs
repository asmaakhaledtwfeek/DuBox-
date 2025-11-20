using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs
{

    public class WIRCheckpointDto
    {
        public Guid WIRId { get; set; }
        public Guid BoxId { get; set; }
        public string BoxName { get; set; } = string.Empty;
        public string BoxTag { get; set; } = string.Empty;

        public string WIRNumber { get; set; } = string.Empty;
        public string? WIRName { get; set; }
        public string? WIRDescription { get; set; }

        public DateTime? RequestedDate { get; set; }
        public string? RequestedBy { get; set; }

        public DateTime? InspectionDate { get; set; }
        public string? InspectorName { get; set; }
        public string? InspectorRole { get; set; }

        public WIRCheckpointStatusEnum Status { get; set; }
        public DateTime? ApprovalDate { get; set; }

        public string? Comments { get; set; }
        public string? AttachmentPath { get; set; }

        public DateTime CreatedDate { get; set; }

        // Calculated fields
        public int PendingDays { get; set; }
        public bool IsOverdue { get; set; }

        // Navigation DTOs
        public BoxDto? Box { get; set; }
        public List<WIRChecklistItemDto> ChecklistItems { get; set; } = new();
        public List<QualityIssueDto> QualityIssues { get; set; } = new();
    }
    public class WIRChecklistItemDto
    {
        public Guid ChecklistItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public string? Comments { get; set; }
    }
    public class QualityIssueDto
    {
        public Guid IssueId { get; set; }
        public string IssueTitle { get; set; } = string.Empty;
        public string? IssueDescription { get; set; }
        public string? Severity { get; set; }
        public DateTime CreatedDate { get; set; }
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
