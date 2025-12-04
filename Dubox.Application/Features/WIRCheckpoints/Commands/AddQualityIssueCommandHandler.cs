using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public class AddQualityIssueCommandHandler
        : IRequestHandler<AddQualityIssueCommand, Result<WIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageProcessingService _imageProcessingService;
        
        public AddQualityIssueCommandHandler(
            IUnitOfWork unitOfWork, 
            ICurrentUserService currentUserService,
            IImageProcessingService imageProcessingService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _imageProcessingService = imageProcessingService;
        }

        public async Task<Result<WIRCheckpointDto>> Handle(AddQualityIssueCommand request, CancellationToken cancellationToken)
        {
            var wir = _unitOfWork.Repository<WIRCheckpoint>()
                .GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            if (wir is null)
                return Result.Failure<WIRCheckpointDto>("WIRCheckpoint not found.");

            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;
            
            var reportedBy = string.Empty;
            if (currentUserId != Guid.Empty)
            {
                var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId);
                if (user != null)
                {
                    reportedBy = user.FullName;
                }
            }

            // Create a single quality issue
            var newIssue = new QualityIssue
            {
                WIRId = wir.WIRId,
                BoxId = wir.BoxId,
                IssueType = request.IssueType,
                Severity = request.Severity,
                IssueDescription = request.IssueDescription,
                AssignedTo = request.AssignedTo,
                DueDate = request.DueDate,
                Status = QualityIssueStatusEnum.Open,
                IssueDate = DateTime.UtcNow,
                ReportedBy = reportedBy
            };

            await _unitOfWork.Repository<QualityIssue>().AddAsync(newIssue, cancellationToken);
            
            try
            {
                await _unitOfWork.CompleteAsync(cancellationToken); // Save to ensure IssueId is available
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                return Result.Failure<WIRCheckpointDto>($"Database error while saving quality issue: {dbEx.Message}. Inner exception: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result.Failure<WIRCheckpointDto>($"Error saving quality issue: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
            }

            // Process images for this issue
            var imagesToAdd = new List<QualityIssueImage>();
            int sequence = 0;

            // Get existing images to determine next sequence
            var existingImages = await _unitOfWork.Repository<QualityIssueImage>()
                .FindAsync(img => img.IssueId == newIssue.IssueId, cancellationToken);
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

                        var image = new QualityIssueImage
                        {
                            IssueId = newIssue.IssueId,
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
                    return Result.Failure<WIRCheckpointDto>($"Error processing uploaded files: {ex.Message}");
                }
            }

            // Process image URLs (base64 data URLs or regular URLs)
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
                            // Already a base64 data URL
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

                            var image = new QualityIssueImage
                            {
                                IssueId = newIssue.IssueId,
                                ImageData = imageData,
                                ImageType = "file", // Camera or file upload
                                FileSize = fileSize,
                                Sequence = sequence++,
                                CreatedDate = DateTime.UtcNow
                            };

                            imagesToAdd.Add(image);
                        }
                        else
                        {
                            // Regular URL - download and convert to base64
                            imageBytes = await _imageProcessingService.DownloadImageFromUrlAsync(trimmedUrl, cancellationToken);

                            if (imageBytes == null || imageBytes.Length == 0)
                            {
                                return Result.Failure<WIRCheckpointDto>($"Failed to download image from URL: {trimmedUrl}");
                            }

                            var base64 = Convert.ToBase64String(imageBytes);
                            imageData = $"data:image/jpeg;base64,{base64}";
                            fileSize = imageBytes.Length;

                            // For OriginalName, truncate if too long (max 500 chars per migration)
                            string? originalName = trimmedUrl.Length > 500 ? trimmedUrl.Substring(0, 500) : trimmedUrl;

                            var image = new QualityIssueImage
                            {
                                IssueId = newIssue.IssueId,
                                ImageData = imageData,
                                ImageType = "url",
                                OriginalName = originalName,
                                FileSize = fileSize,
                                Sequence = sequence++,
                                CreatedDate = DateTime.UtcNow
                            };

                            imagesToAdd.Add(image);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Result.Failure<WIRCheckpointDto>($"Error processing image from URL '{url}': {ex.Message}");
                    }
                }
            }

            // Add all images for this issue
            if (imagesToAdd.Count > 0)
            {
                try
                {
                    foreach (var image in imagesToAdd)
                    {
                        await _unitOfWork.Repository<QualityIssueImage>().AddAsync(image, cancellationToken);
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
                return Result.Failure<WIRCheckpointDto>($"Database error while saving images: {dbEx.Message}. Inner exception: {dbEx.InnerException?.Message}. " +
                    $"Make sure the QualityIssueImages table exists and the IssueId is valid.");
            }
            catch (Exception ex)
            {
                return Result.Failure<WIRCheckpointDto>($"Error saving images: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
            }

            // Reload checkpoint with images to include them in DTO
            wir = _unitOfWork.Repository<WIRCheckpoint>()
                .GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            var dto = wir.Adapt<WIRCheckpointDto>();

            return Result.Success(dto);
        }
    }
}

