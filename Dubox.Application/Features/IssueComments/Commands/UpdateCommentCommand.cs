using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.IssueComments.Commands
{
    /// <summary>
    /// Command to update an existing comment
    /// </summary>
    public record UpdateCommentCommand(
        Guid CommentId,
        string CommentText
    ) : IRequest<Result>;
}

