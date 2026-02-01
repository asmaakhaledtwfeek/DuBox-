using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.IssueComments.Commands
{
    /// <summary>
    /// Command to send notifications for a comment (new or updated)
    /// </summary>
    public record SendCommentNotificationsCommand(
        Guid CommentId,
        bool IsUpdate
    ) : IRequest<Result>;
}

