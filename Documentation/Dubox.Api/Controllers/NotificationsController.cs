using Dubox.Application.Features.Notifications.Commands;
using Dubox.Application.Features.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers
{
    /// <summary>
    /// Controller for managing user notifications
    /// </summary>
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get notifications for the current user
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNotifications(
            [FromQuery] bool unreadOnly = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50,
            CancellationToken cancellationToken = default)
        {
            var query = new GetUserNotificationsQuery(unreadOnly, pageNumber, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Mark a notification as read
        /// </summary>
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(
            Guid notificationId,
            CancellationToken cancellationToken = default)
        {
            var command = new MarkNotificationAsReadCommand(notificationId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get unread notification count
        /// </summary>
        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken = default)
        {
            var query = new GetUserNotificationsQuery(UnreadOnly: true, PageNumber: 1, PageSize: 1000);
            var result = await _mediator.Send(query, cancellationToken);
            
            if (result.IsSuccess && result.Data != null)
            {
                var data = result.Data as dynamic;
                return Ok(new { count = data?.TotalCount ?? 0 });
            }
            
            return Ok(new { count = 0 });
        }
    }
}

