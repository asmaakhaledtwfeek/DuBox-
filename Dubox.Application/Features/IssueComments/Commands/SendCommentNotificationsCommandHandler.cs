using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.IssueComments.Commands
{
    public class SendCommentNotificationsCommandHandler : IRequestHandler<SendCommentNotificationsCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationHubService _notificationHubService;

        public SendCommentNotificationsCommandHandler(
            IUnitOfWork unitOfWork,
            INotificationHubService notificationHubService)
        {
            _unitOfWork = unitOfWork;
            _notificationHubService = notificationHubService;
        }

        public async Task<Result> Handle(SendCommentNotificationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the comment
                var comment = await _unitOfWork.Repository<IssueComment>()
                    .GetByIdAsync(request.CommentId, cancellationToken);

                if (comment == null)
                {
                    return Result.Failure("Comment not found");
                }

                // Get the related issue with its relationships
                var issue = await _unitOfWork.Repository<QualityIssue>()
                    .GetByIdAsync(comment.IssueId, cancellationToken);

                if (issue == null)
                {
                    return Result.Failure("Related issue not found");
                }

                // Get comment author
                var author = await _unitOfWork.Repository<User>()
                    .GetByIdAsync(comment.AuthorId, cancellationToken);
                
                // Collect users to notify (avoid duplicates)
                var usersToNotify = new HashSet<Guid>();

                // 1. Add assigned user
                if (issue.CreatedBy.HasValue)
                {
                    usersToNotify.Add(issue.CreatedBy.Value);
                }

                // 2. Add CC'd user
                if (issue.CCUserId.HasValue)
                {
                    usersToNotify.Add(issue.CCUserId.Value);
                }

                // 3. Add assigned member user
                if (issue.AssignedToMemberId.HasValue)
                {
                    var assignedMember = await _unitOfWork.Repository<TeamMember>()
                        .GetByIdAsync(issue.AssignedToMemberId.Value, cancellationToken);
                    if (assignedMember?.UserId.HasValue == true)
                    {
                        usersToNotify.Add(assignedMember.UserId.Value);
                    }
                }

                // 4. Remove the comment author (don't notify themselves)
                usersToNotify.Remove(comment.AuthorId);

                // Prepare notification details
                var actionType = request.IsUpdate ? "Comment Updated" : "New Comment";
                var commentPreview = comment.CommentText.Length > 150
                    ? comment.CommentText.Substring(0, 150) + "..."
                    : comment.CommentText;

                var title = $"{actionType} on Issue {issue.IssueNumber}";
                var message = $"{author?.FullName ?? "Someone"} {(request.IsUpdate ? "updated a comment" : "added a comment")}: \"{commentPreview}\"";
                
                // Direct link to the issue (frontend will handle scrolling to comment)
                var directLink = $"/projects/{issue.BoxId}/quality-issues/{issue.IssueId}?commentId={comment.CommentId}";

                // Create notifications for each user
                var notifications = new List<Notification>();
                foreach (var userId in usersToNotify)
                {
                    notifications.Add(new Notification
                    {
                        NotificationType = request.IsUpdate ? "CommentUpdated" : "CommentAdded",
                        Priority = "Medium",
                        Title = title,
                        Message = message,
                        RelatedIssueId = issue.IssueId,
                        RelatedCommentId = comment.CommentId,
                        DirectLink = directLink,
                        RecipientUserId = userId,
                        IsRead = false,
                        CreatedDate = DateTime.UtcNow,
                        ExpiryDate = DateTime.UtcNow.AddDays(30) // Expire after 30 days
                    });
                }

                // Save notifications
                if (notifications.Any())
                {
                    foreach (var notification in notifications)
                    {
                        await _unitOfWork.Repository<Notification>().AddAsync(notification, cancellationToken);
                    }
                    await _unitOfWork.CompleteAsync(cancellationToken);

                    // Send real-time notifications via SignalR
                    foreach (var notification in notifications)
                    {
                        if (notification.RecipientUserId.HasValue)
                        {
                            try
                            {
                                // Send notification
                                await _notificationHubService.SendNotificationToUserAsync(
                                    notification.RecipientUserId.Value,
                                    new
                                    {
                                        notification.NotificationId,
                                        notification.NotificationType,
                                        notification.Title,
                                        notification.Message,
                                        notification.DirectLink,
                                        notification.CreatedDate
                                    });

                                // Get updated unread count for the user (exclude expired notifications)
                                var unreadCount = await _unitOfWork.Repository<Notification>()
                                    .CountAsync(n => n.RecipientUserId == notification.RecipientUserId.Value 
                                        && !n.IsRead 
                                        && (!n.ExpiryDate.HasValue || n.ExpiryDate >= DateTime.UtcNow), 
                                        cancellationToken);

                                // Send count update
                                await _notificationHubService.SendNotificationCountUpdateAsync(
                                    notification.RecipientUserId.Value,
                                    unreadCount);
                            }
                            catch (Exception ex)
                            {
                                // Log error but don't fail the operation
                                Console.WriteLine($"Error sending SignalR notification: {ex.Message}");
                            }
                        }
                    }
                }

                return Result.Success($"Sent {notifications.Count} notification(s)");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error sending notifications: {ex.Message}");
            }
        }
    }
}

