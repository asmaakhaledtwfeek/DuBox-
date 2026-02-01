using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.IssueComments.Commands
{
    /// <summary>
    /// Command to add a new comment to a quality issue
    /// </summary>
    public record AddCommentCommand(
        Guid IssueId,
        Guid? ParentCommentId,
        string CommentText,
        bool IsStatusUpdateComment = false,
        QualityIssueStatusEnum? RelatedStatus = null
    ) : IRequest<Result>;
}

