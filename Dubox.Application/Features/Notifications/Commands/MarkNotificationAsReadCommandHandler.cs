using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Notifications.Commands
{
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public MarkNotificationAsReadCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get current user ID
                if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var currentUserId))
                {
                    return Result.Failure("User not authenticated");
                }

                var notification = await _unitOfWork.Repository<Notification>()
                    .GetByIdAsync(request.NotificationId, cancellationToken);

                if (notification == null)
                {
                    return Result.Failure("Notification not found");
                }

                // Verify ownership
                if (notification.RecipientUserId != currentUserId)
                {
                    return Result.Failure("You can only mark your own notifications as read");
                }

                notification.IsRead = true;
                notification.ReadDate = DateTime.UtcNow;

                _unitOfWork.Repository<Notification>().Update(notification);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success("Notification marked as read");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error marking notification as read: {ex.Message}");
            }
        }
    }
}

