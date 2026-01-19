using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Notifications.Queries
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, Result<NotificationResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetUserNotificationsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<NotificationResponseDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get current user ID
                if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var currentUserId))
                {
                    return Result.Failure<NotificationResponseDto>("User not authenticated");
                }

                // Get user's notifications using specification
                var spec = new GetUserNotificationsSpecification(currentUserId, request.UnreadOnly);
                var (notificationsQuery, _) = _unitOfWork.Repository<Notification>().GetWithSpec(spec);

                var allNotifications = notificationsQuery.ToList();
                
                // Use the actual count from the materialized list
                var actualTotalCount = allNotifications.Count;
                
                var notifications = allNotifications
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(n => new
                    {
                        n.NotificationId,
                        n.NotificationType,
                        n.Priority,
                        n.Title,
                        n.Message,
                        n.DirectLink,
                        n.IsRead,
                        n.ReadDate,
                        n.CreatedDate,
                        RelatedIssueId = n.RelatedIssueId,
                        RelatedIssueNumber = n.RelatedIssue != null ? n.RelatedIssue.IssueNumber : null,
                        RelatedCommentId = n.RelatedCommentId,
                        CommentAuthorName = n.RelatedComment != null ? n.RelatedComment.Author.FullName : null
                    })
                    .ToList();

                var result = new NotificationResponseDto
                {
                    Notifications = notifications,
                    TotalCount = actualTotalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = (int)Math.Ceiling(actualTotalCount / (double)request.PageSize)
                };

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<NotificationResponseDto>($"Error retrieving notifications: {ex.Message}");
            }
        }
    }
}

