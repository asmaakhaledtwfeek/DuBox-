using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    /// <summary>
    /// Represents a comment on a quality issue with support for threaded conversations (parent-child relationships)
    /// </summary>
    [Table("IssueComments")]
    public class IssueComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CommentId { get; set; }

        /// <summary>
        /// The quality issue this comment belongs to
        /// </summary>
        [Required]
        [ForeignKey(nameof(QualityIssue))]
        public Guid IssueId { get; set; }

        /// <summary>
        /// For threaded comments: The parent comment ID if this is a reply
        /// </summary>
        [ForeignKey(nameof(ParentComment))]
        public Guid? ParentCommentId { get; set; }

        /// <summary>
        /// The user who created this comment
        /// </summary>
        [Required]
        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }

        /// <summary>
        /// The comment text content
        /// </summary>
        [Required]
        public string CommentText { get; set; } = string.Empty;

        /// <summary>
        /// When the comment was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the comment was last updated (for edit functionality)
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// User who last updated the comment
        /// </summary>
        [ForeignKey(nameof(UpdatedByUser))]
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// When the comment was deleted
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Whether this comment was added during a status update
        /// </summary>
        public bool IsStatusUpdateComment { get; set; } = false;

        /// <summary>
        /// The status that was set when this comment was added (if IsStatusUpdateComment is true)
        /// </summary>
        public Dubox.Domain.Enums.QualityIssueStatusEnum? RelatedStatus { get; set; }

        // Navigation properties
        public virtual QualityIssue QualityIssue { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
        public virtual User? UpdatedByUser { get; set; }
        public virtual IssueComment? ParentComment { get; set; }
        
        /// <summary>
        /// Child replies to this comment
        /// </summary>
        public virtual ICollection<IssueComment> Replies { get; set; } = new List<IssueComment>();

        // Calculated properties
        [NotMapped]
        public bool IsReply => ParentCommentId.HasValue;

        [NotMapped]
        public bool IsEdited => UpdatedDate.HasValue;
    }
}

