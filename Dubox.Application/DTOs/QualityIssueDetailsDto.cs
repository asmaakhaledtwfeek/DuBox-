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
        public string? AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public QualityIssueStatusEnum Status { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string? ResolutionDescription { get; set; }
        public string? PhotoPath { get; set; }

        public bool IsOverdue { get; set; }
        public int OverdueDays { get; set; }

        public Guid BoxId { get; set; }
        public string BoxName { get; set; } = string.Empty;
        public string BoxTag { get; set; } = string.Empty;

        public Guid? WIRId { get; set; }
        public string? WIRNumber { get; set; }
        public string? WIRName { get; set; }
        public WIRCheckpointStatusEnum? WIRStatus { get; set; }
        public DateTime? WIRRequestedDate { get; set; }
        public string? InspectorName { get; set; }
    }

}
