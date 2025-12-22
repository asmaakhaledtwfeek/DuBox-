using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
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
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public ReviewWIRCheckPointCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IImageProcessingService imageProcessingService,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _imageProcessingService = imageProcessingService;
            _visibilityService = visibilityService;
        }

        public async Task<Result<WIRCheckpointDto>> Handle(ReviewWIRCheckPointCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<WIRCheckpointDto>("Access denied. Viewer role has read-only access and cannot review WIR checkpoints.");
            }

            var wir = _unitOfWork.Repository<WIRCheckpoint>().
                GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            if (wir is null)
                return Result.Failure<WIRCheckpointDto>("WIRCheckpoint not found.");

            // Verify user has access to the project this WIR checkpoint belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(wir.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<WIRCheckpointDto>("Access denied. You do not have permission to review this WIR checkpoint.");
            }

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

            // Process images - save to WIRCheckpointImage table (same logic as ProgressUpdate)
            int sequence = 0;
            var existingImages = await _unitOfWork.Repository<WIRCheckpointImage>()
                .FindAsync(img => img.WIRId == wir.WIRId, cancellationToken);
            if (existingImages.Any())
            {
                sequence = existingImages.Max(img => img.Sequence) + 1;
            }

            (bool, string) imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<WIRCheckpointImage>(wir.WIRId, request.Files, request.ImageUrls, cancellationToken, sequence, request.FileNames);
            if (!imagesProcessResult.Item1)
            {
                return Result.Failure<WIRCheckpointDto>(imagesProcessResult.Item2);
            }

            try
            {
                await _unitOfWork.CompleteAsync(cancellationToken);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                return Result.Failure<WIRCheckpointDto>($"Database error: {dbEx.Message}. Inner exception: {dbEx.InnerException?.Message}. " +
                    $"Make sure the WIRCheckpointImages table exists and the WIRId '{wir.WIRId}' is valid.");
            }
            catch (Exception ex)
            {
                return Result.Failure<WIRCheckpointDto>($"Error saving changes: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
            }

            // Reload checkpoint with images to include them in DTO
            wir = _unitOfWork.Repository<WIRCheckpoint>()
                .GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            var dto = wir.Adapt<WIRCheckpointDto>();

            return Result.Success(dto);
        }
    }

}
