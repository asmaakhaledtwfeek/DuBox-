using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public class CreateQualityIssueCommandHandler
        : IRequestHandler<CreateQualityIssueCommand, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public CreateQualityIssueCommandHandler(
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

        public async Task<Result<QualityIssueDetailsDto>> Handle(CreateQualityIssueCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<QualityIssueDetailsDto>("Access denied. Viewer role has read-only access and cannot create quality issues.");
            }

            // Verify the box exists
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);
            if (box is null)
            {
                return Result.Failure<QualityIssueDetailsDto>("Box not found.");
            }

            // Verify user has access to the project this box belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to create quality issues for this box.");
            }

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

            // Create quality issue without WIR checkpoint (WIRId is null)
            var newIssue = new QualityIssue
            {
                BoxId = request.BoxId,
                WIRId = null, // No WIR checkpoint - standalone quality issue
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
                return Result.Failure<QualityIssueDetailsDto>($"Database error while saving quality issue: {dbEx.Message}. Inner exception: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result.Failure<QualityIssueDetailsDto>($"Error saving quality issue: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
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
                    return Result.Failure<QualityIssueDetailsDto>($"Error processing uploaded files: {ex.Message}");
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
                                return Result.Failure<QualityIssueDetailsDto>($"Failed to download image from URL: {trimmedUrl}");
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
                        return Result.Failure<QualityIssueDetailsDto>($"Error processing image from URL '{url}': {ex.Message}");
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
                    return Result.Failure<QualityIssueDetailsDto>($"Error adding images to database: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
                }
            }

            try
            {
                await _unitOfWork.CompleteAsync(cancellationToken);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                return Result.Failure<QualityIssueDetailsDto>($"Database error while saving images: {dbEx.Message}. Inner exception: {dbEx.InnerException?.Message}. " +
                    $"Make sure the QualityIssueImages table exists and the IssueId is valid.");
            }
            catch (Exception ex)
            {
                return Result.Failure<QualityIssueDetailsDto>($"Error saving images: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
            }

            // Reload the quality issue with all related data to return complete DTO
            var qualityIssue = _unitOfWork.Repository<QualityIssue>()
                .GetEntityWithSpec(new GetQualityIssueByIdSpecification(newIssue.IssueId));

            if (qualityIssue == null)
            {
                return Result.Failure<QualityIssueDetailsDto>("Failed to retrieve created quality issue.");
            }

            var dto = qualityIssue.Adapt<QualityIssueDetailsDto>();

            return Result.Success(dto);
        }
    }
}

