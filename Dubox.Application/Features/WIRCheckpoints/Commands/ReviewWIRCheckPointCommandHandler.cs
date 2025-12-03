using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public class ReviewWIRCheckPointCommandHandler
    : IRequestHandler<ReviewWIRCheckPointCommand, Result<WIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ReviewWIRCheckPointCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<WIRCheckpointDto>> Handle(ReviewWIRCheckPointCommand request, CancellationToken cancellationToken)
        {
            var wir = _unitOfWork.Repository<WIRCheckpoint>().
                GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            if (wir is null)
                return Result.Failure<WIRCheckpointDto>("WIRCheckpoint not found.");

            var invalidIds = request.Items
                .Select(i => i.ChecklistItemId)
                .Except(wir.ChecklistItems.Select(c => c.ChecklistItemId))
                .ToList();

            if (invalidIds.Any())
                return Result.Failure<WIRCheckpointDto>("Some ChecklistItemIds do not belong to this WIRCheckpoint.");

            foreach (var item in request.Items)
            {
                var checklistItem = wir.ChecklistItems.First(c => c.ChecklistItemId == item.ChecklistItemId);
                checklistItem.Status = item.Status;
                checklistItem.Remarks = item.Remarks;
            }

            wir.InspectionDate = DateTime.UtcNow;

            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;
            if (currentUserId != Guid.Empty)
            {
                var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId);
                if (user != null)
                {
                    wir.InspectorName = user.FullName;
                }
            }

            if (!string.IsNullOrWhiteSpace(request.InspectorRole))
            {
                wir.InspectorRole = request.InspectorRole.Trim();
            }

            wir.Status = request.Status;
            if (request.Status == WIRCheckpointStatusEnum.Approved || request.Status == WIRCheckpointStatusEnum.ConditionalApproval)
                wir.ApprovalDate = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(request.Comment))
                wir.Comments = request.Comment;

            if (!string.IsNullOrWhiteSpace(request.AttachmentPath))
                wir.AttachmentPath = request.AttachmentPath.Trim();

            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = wir.Adapt<WIRCheckpointDto>();

            return Result.Success(dto);
        }
    }

}
