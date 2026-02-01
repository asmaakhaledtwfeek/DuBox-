using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Dubox.Application.Features.IssueComments.Commands
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UpdateCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IServiceScopeFactory serviceScopeFactory)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
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

                // Verify ownership (only author can edit)
                if (comment.AuthorId != currentUserId)
                {
                    return Result.Failure("You can only edit your own comments");
                }

                // Verify not deleted
                if (comment.IsDeleted)
                {
                    return Result.Failure("Cannot edit a deleted comment");
                }

                // Update the comment
                comment.CommentText = request.CommentText;
                comment.UpdatedDate = DateTime.UtcNow;
                comment.UpdatedBy = currentUserId;

                _unitOfWork.Repository<IssueComment>().Update(comment);
                await _unitOfWork.CompleteAsync(cancellationToken);

                // Send notifications asynchronously with proper scoping
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // Create a new scope for the background task
                        using var scope = _serviceScopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        
                        await mediator.Send(new SendCommentNotificationsCommand(
                            comment.CommentId,
                            IsUpdate: true
                        ), CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending comment update notifications: {ex.Message}");
                    }
                }, CancellationToken.None);

                return Result.Success("Comment updated successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating comment: {ex.Message}");
            }
        }
    }
}

