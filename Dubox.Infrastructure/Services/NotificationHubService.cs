using Dubox.Domain.Services;
using Dubox.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Dubox.Infrastructure.Services
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToUserAsync(Guid userId, object notification)
        {
            await _hubContext.Clients
                .Group($"user_{userId}")
                .SendAsync("ReceiveNotification", notification);
        }

        public async Task SendNotificationCountUpdateAsync(Guid userId, int unreadCount)
        {
            await _hubContext.Clients
                .Group($"user_{userId}")
                .SendAsync("NotificationCountUpdated", unreadCount);
        }
    }
}

