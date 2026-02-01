using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs
{
    /// <summary>
    /// DTO for issue comment with threaded conversation support
    /// </summary>
    public class IssueCommentDto
    {
        public Guid CommentId { get; set; }
        public Guid IssueId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorAvatar { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsStatusUpdateComment { get; set; }
        public QualityIssueStatusEnum? RelatedStatus { get; set; }
        public bool IsReply { get; set; }
        public bool IsEdited { get; set; }
        public List<IssueCommentDto> Replies { get; set; } = new();
        public int ReplyCount { get; set; }
    }

    /// <summary>
    /// Request for creating a new comment
    /// </summary>
    public class CreateCommentRequest
    {
        public Guid IssueId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public bool IsStatusUpdateComment { get; set; }
        public QualityIssueStatusEnum? RelatedStatus { get; set; }
    }

    /// <summary>
    /// Request for updating an existing comment
    /// </summary>
    public class UpdateCommentRequest
    {
        public Guid CommentId { get; set; }
        public string CommentText { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request for deleting a comment
    /// </summary>
    public class DeleteCommentRequest
    {
        public Guid CommentId { get; set; }
    }
}

