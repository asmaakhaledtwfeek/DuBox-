using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
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

            (bool, string) imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<QualityIssueImage>(newIssue.IssueId, request.Files, request.ImageUrls, cancellationToken);
            if (!imagesProcessResult.Item1)
            {
                return Result.Failure<QualityIssueDetailsDto>(imagesProcessResult.Item2);
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

            return Result.Success(new QualityIssueDetailsDto());
        }
        private async Task<(bool, string)> ImagesProcessing(Guid issueId, List<byte[]>? files, List<string>? imageUrls, CancellationToken cancellationToken)
        {
            var imagesToAdd = new List<QualityIssueImage>();
            int sequence = 0;

            if (files != null && files.Count > 0)
            {
                try
                {
                    foreach (var fileBytes in files.Where(f => f != null && f.Length > 0))
                    {
                        var base64 = Convert.ToBase64String(fileBytes);
                        var imageData = $"data:image/jpeg;base64,{base64}";

                        var image = new QualityIssueImage
                        {
                            IssueId = issueId,
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
                    return (false, $"Error processing uploaded files: {ex.Message}");
                }
            }

            if (imageUrls != null && imageUrls.Count > 0)
            {
                foreach (var url in imageUrls.Where(url => !string.IsNullOrWhiteSpace(url)))
                {
                    try
                    {
                        string imageData;
                        long fileSize;
                        byte[]? imageBytes = null;
                        string imageType = "url";
                        var trimmedUrl = url.Trim();

                        if (trimmedUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                        {
                            imageData = trimmedUrl;
                            imageType = "file";
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
                                IssueId = issueId,
                                ImageData = imageData,
                                ImageType = imageType,
                                FileSize = fileSize,
                                Sequence = sequence++,
                                CreatedDate = DateTime.UtcNow
                            };

                            imagesToAdd.Add(image);
                        }
                        else
                        {
                            imageBytes = await _imageProcessingService.DownloadImageFromUrlAsync(trimmedUrl, cancellationToken);

                            if (imageBytes == null || imageBytes.Length == 0)
                            {
                                return (false, $"Failed to download image from URL: {trimmedUrl}");
                            }

                            var base64 = Convert.ToBase64String(imageBytes);
                            imageData = $"data:image/jpeg;base64,{base64}";
                            fileSize = imageBytes.Length;

                            string? originalName = trimmedUrl.Length > 500 ? trimmedUrl.Substring(0, 500) : trimmedUrl;

                            var image = new QualityIssueImage
                            {
                                IssueId = issueId,
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
                        return (false, $"Error processing image from URL '{url}': {ex.Message}");
                    }
                }
            }

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
                    return (false, $"Error adding images to database: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
                }
            }
            return (true, string.Empty);

        }
    }

}

