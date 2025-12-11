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
            var existingImages = await _unitOfWork.Repository<QualityIssueImage>()
                .FindAsync(img => img.IssueId == issue.IssueId, cancellationToken);
            if (existingImages.Any())
            {
                sequence = existingImages.Max(img => img.Sequence) + 1;
            }
            (bool, string) imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<QualityIssueImage>(issue.IssueId, request.Files, request.ImageUrls, cancellationToken, sequence);
            if (!imagesProcessResult.Item1)
            {
                return Result.Failure<QualityIssueDetailsDto>(imagesProcessResult.Item2);
            }
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
