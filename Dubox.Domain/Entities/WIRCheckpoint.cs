using Dubox.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("WIRCheckpoints")]
    [Index(nameof(BoxId))]
    public class WIRCheckpoint
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WIRId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [Required]
        [MaxLength(20)]
        public string WIRCode { get; set; } = string.Empty; // WIR-1, WIR-2, WIR-3, WIR-4, WIR-5, WIR-6

        [MaxLength(1000)]
        public string? WIRName { get; set; }

        [MaxLength(1000)]
        public string? WIRDescription { get; set; }

        public DateTime? RequestedDate { get; set; }

        [MaxLength(200)]
        public string? RequestedBy { get; set; }

        public DateTime? InspectionDate { get; set; }

        [MaxLength(200)]
        public string? InspectorName { get; set; }

        [MaxLength(100)]
        public string? InspectorRole { get; set; } // QC Engineer-Civil, QC Engineer-MEP, etc.

        [MaxLength(50)]
        public WIRCheckpointStatusEnum Status { get; set; } = WIRCheckpointStatusEnum.Pending; // Pending, Approved, Rejected, Conditional Approval

        public DateTime? ApprovalDate { get; set; }

        public string? Comments { get; set; }

        public string? Photo { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? CreatedBy { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual ICollection<WIRChecklistItem> ChecklistItems { get; set; } = new List<WIRChecklistItem>();
        public virtual ICollection<QualityIssue> QualityIssues { get; set; } = new List<QualityIssue>();
        public virtual ICollection<WIRCheckpointImage> Images { get; set; } = new List<WIRCheckpointImage>();

        // Calculated properties
        [NotMapped]
        public int PendingDays => RequestedDate.HasValue && !ApprovalDate.HasValue
            ? (DateTime.Today - RequestedDate.Value.Date).Days
            : 0;

        [NotMapped]
        public bool IsOverdue => PendingDays > 3; // Example: 3 days SLA
    }
}
