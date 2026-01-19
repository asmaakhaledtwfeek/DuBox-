using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("Notifications")]
    public class Notification
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid NotificationId { get; set; }

        [MaxLength(50)]
        public string? NotificationType { get; set; } // Alert, Warning, Info, Breakdown, CommentAdded, CommentUpdated

        [MaxLength(20)]
        public string? Priority { get; set; } // High, Medium, Low

        [MaxLength(200)]
        public string? Title { get; set; }

        public string? Message { get; set; }

        [ForeignKey(nameof(RelatedBox))]
        public Guid? RelatedBoxId { get; set; }

        [ForeignKey(nameof(RelatedActivity))]
        public Guid? RelatedActivityId { get; set; }

        /// <summary>
        /// Related quality issue for issue-based notifications
        /// </summary>
        [ForeignKey(nameof(RelatedIssue))]
        public Guid? RelatedIssueId { get; set; }

        /// <summary>
        /// Related comment for comment-based notifications
        /// </summary>
        [ForeignKey(nameof(RelatedComment))]
        public Guid? RelatedCommentId { get; set; }

        /// <summary>
        /// Direct link to the related resource
        /// </summary>
        [MaxLength(500)]
        public string? DirectLink { get; set; }

        /// <summary>
        /// The user who receives this notification
        /// </summary>
        [ForeignKey(nameof(RecipientUser))]
        public Guid? RecipientUserId { get; set; }

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
        public virtual QualityIssue? RelatedIssue { get; set; }
        public virtual IssueComment? RelatedComment { get; set; }
        public virtual User? RecipientUser { get; set; }

        [NotMapped]
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate < DateTime.UtcNow;
    }

}
