using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Dubox.Application.Features.IssueComments.Commands
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AddCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IServiceScopeFactory serviceScopeFactory)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Result> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get current user ID
                if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var currentUserId))
                {
                    return Result.Failure("User not authenticated");
                }

                // Verify the issue exists
                var issue = await _unitOfWork.Repository<QualityIssue>()
                    .GetByIdAsync(request.IssueId, cancellationToken);

                if (issue == null)
                {
                    return Result.Failure("Quality issue not found");
                }

                // If this is a reply, verify parent comment exists
                if (request.ParentCommentId.HasValue)
                {
                    var parentComment = await _unitOfWork.Repository<IssueComment>()
                        .GetByIdAsync(request.ParentCommentId.Value, cancellationToken);

                    if (parentComment == null)
                    {
                        return Result.Failure("Parent comment not found");
                    }

                    if (parentComment.IssueId != request.IssueId)
                    {
                        return Result.Failure("Parent comment does not belong to this issue");
                    }
                }

                // Create the comment
                var comment = new IssueComment
                {
                    IssueId = request.IssueId,
                    ParentCommentId = request.ParentCommentId,
                    AuthorId = currentUserId,
                    CommentText = request.CommentText,
                    IsStatusUpdateComment = request.IsStatusUpdateComment,
                    RelatedStatus = request.RelatedStatus,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<IssueComment>().AddAsync(comment, cancellationToken);
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
                            IsUpdate: false
                        ), CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        // Log the error but don't fail the command
                        Console.WriteLine($"Error sending comment notifications: {ex.Message}");
                    }
                }, CancellationToken.None);

                return Result.Success("Comment added successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error adding comment: {ex.Message}");
            }
        }
    }
}

