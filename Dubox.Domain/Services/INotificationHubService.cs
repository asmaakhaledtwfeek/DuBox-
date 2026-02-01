namespace Dubox.Domain.Services
{
    /// <summary>
    /// Service for sending real-time notifications via SignalR
    /// </summary>
    public interface INotificationHubService
    {
        Task SendNotificationToUserAsync(Guid userId, object notification);
        Task SendNotificationCountUpdateAsync(Guid userId, int unreadCount);
    }
}

