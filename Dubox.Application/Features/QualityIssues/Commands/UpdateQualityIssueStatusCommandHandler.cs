using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public class UpdateQualityIssueStatusCommandHandler : IRequestHandler<UpdateQualityIssueStatusCommand, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public UpdateQualityIssueStatusCommandHandler(
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

        public async Task<Result<QualityIssueDetailsDto>> Handle(UpdateQualityIssueStatusCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<QualityIssueDetailsDto>("Access denied. Viewer role has read-only access and cannot modify quality issues.");
            }

            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found.");

            // Verify user has access to the project this quality issue belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(issue.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to modify this quality issue.");
            }
            
            issue.Status = request.Status;

            if (request.Status == QualityIssueStatusEnum.Resolved ||
                request.Status == QualityIssueStatusEnum.Closed)
            {
                issue.ResolutionDescription = request.ResolutionDescription;
                issue.ResolutionDate = DateTime.UtcNow;
            }
            else
            {
                issue.ResolutionDescription = null;
                issue.ResolutionDate = null;
            }

            _unitOfWork.Repository<QualityIssue>().Update(issue);
            await _unitOfWork.CompleteAsync(cancellationToken); // Save to ensure IssueId is available

            // Process images
            int sequence = 0;
            var imagesToAdd = new List<QualityIssueImage>();

            // Get existing images to determine next sequence
            var existingImages = await _unitOfWork.Repository<QualityIssueImage>()
                .FindAsync(img => img.IssueId == issue.IssueId, cancellationToken);
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
                            IssueId = issue.IssueId,
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
                    return Result.Failure<QualityIssueDetailsDto>("Error processing uploaded files: " + ex.Message);
                }
            }

            // Process image URLs - download and convert to base64
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
                                return Result.Failure<QualityIssueDetailsDto>($"Failed to download image from URL: {trimmedUrl}");
                            }

                            var base64 = Convert.ToBase64String(imageBytes);
                            imageData = $"data:image/jpeg;base64,{base64}";
                            fileSize = imageBytes.Length;
                        }

                        // Cap original name length to avoid DB truncation issues
                        // Use a fallback name for data URLs to keep it short
                        var originalName = trimmedUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase)
                            ? "data-url-image"
                            : (trimmedUrl.Length > 500 ? trimmedUrl.Substring(0, 500) : trimmedUrl);

                        var image = new QualityIssueImage
                        {
                            IssueId = issue.IssueId,
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
                        return Result.Failure<QualityIssueDetailsDto>($"Error processing image from URL '{url}': {ex.Message}");
                    }
                }
            }

            // Add all images
            if (imagesToAdd.Count > 0)
            {
                foreach (var image in imagesToAdd)
                {
                    await _unitOfWork.Repository<QualityIssueImage>().AddAsync(image, cancellationToken);
                }
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            // Reload the issue with images to ensure the DTO has the updated data
            issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));
            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found after update.");

            var dto = issue.Adapt<QualityIssueDetailsDto>();
            // Manually map images to ensure they're included
            dto.Images = issue.Images
                .OrderBy(img => img.Sequence)
                .Select(img => new QualityIssueImageDto
                {
                    QualityIssueImageId = img.QualityIssueImageId,
                    IssueId = img.IssueId,
                    ImageData = img.ImageData,
                    ImageType = img.ImageType,
                    OriginalName = img.OriginalName,
                    FileSize = img.FileSize,
                    Sequence = img.Sequence,
                    CreatedDate = img.CreatedDate
                }).ToList();

            return Result.Success(dto);
        }
    }

}
