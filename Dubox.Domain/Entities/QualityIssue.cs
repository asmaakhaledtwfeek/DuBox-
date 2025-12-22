using Dubox.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("QualityIssues")]
    [Index(nameof(BoxId))]
    [Index(nameof(Status))]
    public class QualityIssue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid IssueId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [ForeignKey(nameof(WIRCheckpoint))]
        public Guid? WIRId { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;


        public IssueTypeEnum? IssueType { get; set; } // Defect, Non-Conformance, Observation


        public SeverityEnum? Severity { get; set; } // Critical, Major, Minor

        public string? IssueDescription { get; set; }

        [MaxLength(200)]
        public string? ReportedBy { get; set; }
        [ForeignKey(nameof(AssignedToTeam))]
        public Guid? AssignedTo { get; set; }
        [ForeignKey(nameof(AssignedToUser))]
        public Guid? AssignedToUserId { get; set; }
        
        public DateTime? DueDate { get; set; }

        public QualityIssueStatusEnum Status { get; set; } = QualityIssueStatusEnum.Open; // Open, In Progress, Resolved, Closed

        public DateTime? ResolutionDate { get; set; }

        public string? ResolutionDescription { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual Team? AssignedToTeam { get; set; }
        public virtual User? AssignedToUser { get; set; }
        public virtual WIRCheckpoint? WIRCheckpoint { get; set; }

        public List<QualityIssueImage> Images { get; set; } = new();

        // Calculated properties
        [NotMapped]
        public bool IsOverdue => DueDate.HasValue &&
                                 Status != QualityIssueStatusEnum.Resolved &&
                                 Status != QualityIssueStatusEnum.Closed &&
                                 DueDate < DateTime.Today;

        [NotMapped]
        public int OverdueDays => IsOverdue ? (DateTime.Today - DueDate!.Value).Days : 0;
    }
}
