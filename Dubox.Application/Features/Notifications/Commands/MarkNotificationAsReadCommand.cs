using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Notifications.Commands
{
    /// <summary>
    /// Command to mark a notification as read
    /// </summary>
    public record MarkNotificationAsReadCommand(
        Guid NotificationId
    ) : IRequest<Result>;
}

