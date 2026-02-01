using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;

namespace Dubox.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationHubService _notificationHubService;

        public NotificationService(
            IUnitOfWork unitOfWork,
            INotificationHubService notificationHubService)
        {
            _unitOfWork = unitOfWork;
            _notificationHubService = notificationHubService;
        }

        public async Task<bool> NotifyPanelIssueResolvedAsync(
            Guid recipientUserId, 
            string panelName, 
            string boxTag, 
            string issueNumber, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var message = $"Quality issue {issueNumber} for panel '{panelName}' in box '{boxTag}' has been resolved and is ready for re-approval.";

                var notification = new Notification
                {
                    RecipientUserId = recipientUserId,
                    Title = "Panel Issue Resolved",
                    Message = message,
                    NotificationType = "PanelWorkflow",
                    Priority = "High",
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Notification>().AddAsync(notification, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                // Send real-time notification via SignalR
                await _notificationHubService.SendNotificationToUserAsync(recipientUserId, new
                {
                    notificationId = notification.NotificationId,
                    title = notification.Title,
                    message = notification.Message,
                    type = notification.NotificationType,
                    createdAt = notification.CreatedDate
                });

                // Update unread count
                var unreadCount = (await _unitOfWork.Repository<Notification>()
                    .FindAsync(n => n.RecipientUserId == recipientUserId && !n.IsRead, cancellationToken))
                    .Count();
                
                await _notificationHubService.SendNotificationCountUpdateAsync(recipientUserId, unreadCount);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

