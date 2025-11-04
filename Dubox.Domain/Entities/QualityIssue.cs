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
        public int IssueId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [ForeignKey(nameof(WIRCheckpoint))]
        public int? WIRId { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? IssueType { get; set; } // Defect, Non-Conformance, Observation

        [MaxLength(50)]
        public string? Severity { get; set; } // Critical, Major, Minor

        public string? IssueDescription { get; set; }

        [MaxLength(200)]
        public string? ReportedBy { get; set; }

        [MaxLength(200)]
        public string? AssignedTo { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Open"; // Open, In Progress, Resolved, Closed

        public DateTime? ResolutionDate { get; set; }

        public string? ResolutionDescription { get; set; }

        [MaxLength(500)]
        public string? PhotoPath { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual WIRCheckpoint? WIRCheckpoint { get; set; }

        // Calculated properties
        [NotMapped]
        public bool IsOverdue => DueDate.HasValue &&
                                 Status != "Resolved" &&
                                 Status != "Closed" &&
                                 DueDate < DateTime.Today;

        [NotMapped]
        public int OverdueDays => IsOverdue ? (DateTime.Today - DueDate!.Value).Days : 0;
    }
}
