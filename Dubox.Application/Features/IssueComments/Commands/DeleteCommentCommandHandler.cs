using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.IssueComments.Commands
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get current user ID
                if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var currentUserId))
                {
                    return Result.Failure("User not authenticated");
                }

                // Get the comment
                var comment = await _unitOfWork.Repository<IssueComment>()
                    .GetByIdAsync(request.CommentId, cancellationToken);

                if (comment == null)
                {
                    return Result.Failure("Comment not found");
                }

                // Verify ownership (only author can delete)
                if (comment.AuthorId != currentUserId)
                {
                    return Result.Failure("You can only delete your own comments");
                }

                // Soft delete
                comment.IsDeleted = true;
                comment.DeletedDate = DateTime.UtcNow;

                _unitOfWork.Repository<IssueComment>().Update(comment);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success("Comment deleted successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deleting comment: {ex.Message}");
            }
        }
    }
}

