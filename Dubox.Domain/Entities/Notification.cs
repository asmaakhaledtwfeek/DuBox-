using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("Notifications")]
    public class Notification
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        [MaxLength(50)]
        public string? NotificationType { get; set; } // Alert, Warning, Info, Breakdown

        [MaxLength(20)]
        public string? Priority { get; set; } // High, Medium, Low

        [MaxLength(200)]
        public string? Title { get; set; }

        public string? Message { get; set; }

        [ForeignKey(nameof(RelatedBox))]
        public Guid? RelatedBoxId { get; set; }

        [ForeignKey(nameof(RelatedActivity))]
        public int? RelatedActivityId { get; set; }

        [MaxLength(100)]
        public string? TargetRole { get; set; }

        [MaxLength(200)]
        public string? TargetUser { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryDate { get; set; }

        // Navigation properties
        public virtual Box? RelatedBox { get; set; }
        public virtual BoxActivity? RelatedActivity { get; set; }

        [NotMapped]
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate < DateTime.UtcNow;
    }

}
