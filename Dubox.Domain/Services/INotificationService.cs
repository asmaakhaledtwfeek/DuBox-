namespace Dubox.Domain.Services
{
    /// <summary>
    /// Service for creating and managing system notifications
    /// </summary>
    public interface INotificationService
    {
        Task<bool> NotifyPanelIssueResolvedAsync(
            Guid recipientUserId, 
            string panelName, 
            string boxTag, 
            string issueNumber, 
            CancellationToken cancellationToken = default);
    }
}

