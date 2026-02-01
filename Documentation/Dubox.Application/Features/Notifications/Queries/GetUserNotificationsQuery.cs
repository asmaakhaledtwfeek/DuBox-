using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Notifications.Queries
{
    /// <summary>
    /// Query to get notifications for the current user
    /// </summary>
    public record GetUserNotificationsQuery(
        bool UnreadOnly = false,
        int PageNumber = 1,
        int PageSize = 50
    ) : IRequest<Result<NotificationResponseDto>>;
}

