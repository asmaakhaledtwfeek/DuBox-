using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.IssueComments.Commands
{
    /// <summary>
    /// Command to delete (soft delete) a comment
    /// </summary>
    public record DeleteCommentCommand(
        Guid CommentId
    ) : IRequest<Result>;
}

