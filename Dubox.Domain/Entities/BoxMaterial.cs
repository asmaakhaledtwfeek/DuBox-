using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    /// <summary>
    /// Tracks materials required for each box with arrival status and notification tracking
    /// </summary>
    [Table("BoxMaterials")]
    [Index(nameof(BoxId))]
    [Index(nameof(MaterialId))]
    [Index(nameof(IsArrived))]
    [Index(nameof(RequiredByDate))]
    public class BoxMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BoxMaterialId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [Required]
        [ForeignKey(nameof(Material))]
        public Guid MaterialId { get; set; }

        [ForeignKey(nameof(ProjectMaterial))]
        public Guid? ProjectMaterialId { get; set; }

        /// <summary>
        /// Number of days before box planned start date that material must arrive
        /// Typically 7 (one week) or 30 (one month)
        /// </summary>
        [Required]
        public int RequiredBeforeDays { get; set; }

        /// <summary>
        /// Calculated deadline: Box.PlannedStartDate - RequiredBeforeDays
        /// </summary>
        [Required]
        public DateTime RequiredByDate { get; set; }

        /// <summary>
        /// Whether the material has arrived for this box
        /// </summary>
        public bool IsArrived { get; set; } = false;

        /// <summary>
        /// Date when material was marked as arrived
        /// </summary>
        public DateTime? ArrivedDate { get; set; }

        /// <summary>
        /// User who marked the material as arrived
        /// </summary>
        [ForeignKey(nameof(ArrivedByUser))]
        public Guid? ArrivedBy { get; set; }

        /// <summary>
        /// Last date a notification was sent about this material
        /// </summary>
        public DateTime? LastNotificationDate { get; set; }

        /// <summary>
        /// Total number of notifications sent for this material
        /// </summary>
        public int NotificationsSentCount { get; set; } = 0;

        /// <summary>
        /// Quality issue created if material is delayed beyond deadline
        /// </summary>
        [ForeignKey(nameof(QualityIssue))]
        public Guid? QualityIssueId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public int? DeliveredQuantity { get; set; }


        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;
        public virtual ProjectMaterial? ProjectMaterial { get; set; }
        public virtual QualityIssue? QualityIssue { get; set; }
        public virtual User? ArrivedByUser { get; set; }

        // Calculated properties
        [NotMapped]
        public int DaysUntilRequired
        {
            get
            {
                var daysRemaining = (RequiredByDate.Date - DateTime.UtcNow.Date).Days;
                return daysRemaining;
            }
        }

        [NotMapped]
        public bool IsOverdue => !IsArrived && DateTime.UtcNow > RequiredByDate;

        [NotMapped]
        public bool IsApproachingDeadline => !IsArrived && DaysUntilRequired <= 10 && DaysUntilRequired >= 0;

        [NotMapped]
        public string Status
        {
            get
            {
                if (IsArrived) return "Arrived";
                if (IsOverdue) return "Overdue";
                if (IsApproachingDeadline) return "Approaching";
                return "Pending";
            }
        }
    }
}






