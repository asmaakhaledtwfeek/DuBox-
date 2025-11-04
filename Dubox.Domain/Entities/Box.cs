using Dubox.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("Boxes")]
    [Index(nameof(QRCode), IsUnique = true)]
    [Index(nameof(BoxTag), IsUnique = true)]
    public class Box : IAuditableEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BoxId { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BoxTag { get; set; } = string.Empty; // e.g., "B1-GF-BED-01"

        [MaxLength(200)]
        public string? BoxName { get; set; }

        [Required]
        [MaxLength(50)]
        public string BoxType { get; set; } = string.Empty; // Bedroom, Living, Kitchen, Bathroom, Stair, Balcony

        [Required]
        [MaxLength(50)]
        public string Floor { get; set; } = string.Empty;// GF, FF, SF, etc.

        [MaxLength(50)]
        public string? Building { get; set; }

        [MaxLength(50)]
        public string? Zone { get; set; }

        [Required]
        [MaxLength(500)]
        public string QRCode { get; set; } = string.Empty;

        public byte[]? QRCodeImage { get; set; }

        [MaxLength(100)]
        public string? RFIDTag { get; set; }

        [MaxLength(50)]
        public string CurrentStatus { get; set; } = "Not Started"; // Not Started, In Progress, Completed, On Hold, Delayed

        [MaxLength(200)]
        public string? CurrentLocation { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; } = 0;

        public DateTime? PlannedStartDate { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? PlannedEndDate { get; set; }

        public DateTime? ActualEndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<BoxAsset> Assets { get; set; } = new List<BoxAsset>();
        public virtual ICollection<BoxActivity> Activities { get; set; } = new List<BoxActivity>();
        public virtual ICollection<ProgressUpdate> ProgressUpdates { get; set; } = new List<ProgressUpdate>();
        public virtual ICollection<WIRCheckpoint> WIRCheckpoints { get; set; } = new List<WIRCheckpoint>();
        public virtual ICollection<BoxMaterial> BoxMaterials { get; set; } = new List<BoxMaterial>();
        public virtual ICollection<QualityIssue> QualityIssues { get; set; } = new List<QualityIssue>();
        public virtual ICollection<BoxLocationHistory> LocationHistory { get; set; } = new List<BoxLocationHistory>();

        // Calculated properties
        [NotMapped]
        public int TotalActivities => Activities?.Count ?? 0;

        [NotMapped]
        public int CompletedActivities => Activities?.Count(a => a.Status == "Completed") ?? 0;

        [NotMapped]
        public bool IsDelayed => PlannedEndDate.HasValue &&
                                 !ActualEndDate.HasValue &&
                                 PlannedEndDate < DateTime.Today;

        [NotMapped]
        public int DelayDays => IsDelayed ?
            (DateTime.Today - PlannedEndDate!.Value).Days : 0;
    }
}
