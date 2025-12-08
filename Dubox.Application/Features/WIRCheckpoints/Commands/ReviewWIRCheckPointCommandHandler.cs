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
        private readonly IImageProcessingService _imageProcessingService;

        public ReviewWIRCheckPointCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IImageProcessingService imageProcessingService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _imageProcessingService = imageProcessingService;
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

            // Process images - save to WIRCheckpointImage table (same logic as ProgressUpdate)
            int sequence = 0;
            var imagesToAdd = new List<WIRCheckpointImage>();

            // Get existing images to determine next sequence
            var existingImages = await _unitOfWork.Repository<WIRCheckpointImage>()
                .FindAsync(img => img.WIRId == wir.WIRId, cancellationToken);
            if (existingImages.Any())
            {
                sequence = existingImages.Max(img => img.Sequence) + 1;
            }

            // Process uploaded files - convert to base64
            if (request.Files != null && request.Files.Count > 0)
            {
                try
                {
                    foreach (var fileBytes in request.Files.Where(f => f != null && f.Length > 0))
                    {
                        var base64 = Convert.ToBase64String(fileBytes);
                        var imageData = $"data:image/jpeg;base64,{base64}";

                        var image = new WIRCheckpointImage
                        {
                            WIRId = wir.WIRId,
                            ImageData = imageData,
                            ImageType = "file",
                            FileSize = fileBytes.Length,
                            Sequence = sequence++,
                            CreatedDate = DateTime.UtcNow
                        };

                        imagesToAdd.Add(image);
                    }
                }
                catch (Exception ex)
                {
                    return Result.Failure<WIRCheckpointDto>("Error processing uploaded files: " + ex.Message);
                }
            }

            if (request.ImageUrls != null && request.ImageUrls.Count > 0)
            {
                foreach (var url in request.ImageUrls.Where(url => !string.IsNullOrWhiteSpace(url)))
                {
                    try
                    {
                        string imageData;
                        long fileSize;
                        byte[]? imageBytes = null;

                        var trimmedUrl = url.Trim();

                        if (trimmedUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                        {
                            imageData = trimmedUrl;

                            var base64Index = trimmedUrl.IndexOf(',');
                            if (base64Index >= 0 && base64Index < trimmedUrl.Length - 1)
                            {
                                var base64Part = trimmedUrl.Substring(base64Index + 1);
                                imageBytes = Convert.FromBase64String(base64Part);
                                fileSize = imageBytes.Length;
                            }
                            else
                            {
                                fileSize = 0;
                            }
                        }
                        else
                        {
                            imageBytes = await _imageProcessingService.DownloadImageFromUrlAsync(trimmedUrl, cancellationToken);

                            if (imageBytes == null || imageBytes.Length == 0)
                            {
                                return Result.Failure<WIRCheckpointDto>($"Failed to download image from URL: {trimmedUrl}");
                            }

                            var base64 = Convert.ToBase64String(imageBytes);
                            imageData = $"data:image/jpeg;base64,{base64}";
                            fileSize = imageBytes.Length;
                        }

                        // For OriginalName, truncate if too long (max 500 chars per migration)
                        // For data URLs, don't store the full base64 string
                        string? originalName = null;
                        if (trimmedUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                        {
                            // For data URLs, just store a short identifier
                            originalName = "data-url";
                        }
                        else
                        {
                            // For regular URLs, truncate to 500 characters if needed
                            originalName = trimmedUrl.Length > 500 ? trimmedUrl.Substring(0, 500) : trimmedUrl;
                        }

                        var image = new WIRCheckpointImage
                        {
                            WIRId = wir.WIRId,
                            ImageData = imageData,
                            ImageType = "url",
                            OriginalName = originalName,
                            FileSize = fileSize,
                            Sequence = sequence++,
                            CreatedDate = DateTime.UtcNow
                        };

                        imagesToAdd.Add(image);
                    }
                    catch (Exception ex)
                    {
                        return Result.Failure<WIRCheckpointDto>($"Error processing image from URL '{url}': {ex.Message}");
                    }
                }
            }
            // Add all images to the repository
            if (imagesToAdd.Count > 0)
            {
                try
                {
                    foreach (var image in imagesToAdd)
                    {
                        await _unitOfWork.Repository<WIRCheckpointImage>().AddAsync(image, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    return Result.Failure<WIRCheckpointDto>($"Error adding images to database: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
                }
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
