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
           var module= PermissionModuleEnum.QualityIssues;
            var action = PermissionActionEnum.UpdateStatus;
            var canModify = await _visibilityService.CanPerformAsync(module, action,cancellationToken);
            if (!canModify)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to modify quality issues.");

            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found.");

            // Verify user has access to the project this quality issue belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(issue.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to modify this quality issue.");

            var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(issue.Box.ProjectId, "update quality issue", cancellationToken);
            if (!projectStatusValidation.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(projectStatusValidation.Error!);

            var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(issue.BoxId, "update quality issues", cancellationToken);
            if (!boxStatusValidation.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(boxStatusValidation.Error!);
            
            // Capture old values for audit log
            var oldStatus = issue.Status.ToString();
            var oldResolutionDescription = issue.ResolutionDescription ?? "N/A";
            var oldResolutionDate = issue.ResolutionDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A";

            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;

            issue.Status = request.Status;
            issue.UpdatedBy = currentUserId;
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
           var imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<QualityIssueImage>
                (issue.IssueId, request.Files, request.ImageUrls, cancellationToken, sequence, fileNames: request.FileNames,
                existingImagesForVersioning: existingImages.ToList());
            if (!imagesProcessResult.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(imagesProcessResult.Item2);

           await _unitOfWork.CompleteAsync();
            issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));
            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found after update.");

            // Create audit log for status update
            var newResolutionDescription = issue.ResolutionDescription ?? "N/A";
            var newResolutionDate = issue.ResolutionDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A";
            
            var auditLog = new AuditLog
            {
                TableName = nameof(QualityIssue),
                RecordId = issue.IssueId,
                Action = "UPDATE",
                OldValues = $"Status: {oldStatus}, ResolutionDescription: {oldResolutionDescription}, ResolutionDate: {oldResolutionDate}",
                NewValues = $"Status: {issue.Status}, ResolutionDescription: {newResolutionDescription}, ResolutionDate: {newResolutionDate}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Quality Issue status updated from {oldStatus} to {issue.Status}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = issue.Adapt<QualityIssueDetailsDto>();
            dto.AssignedToUserName = issue.AssignedToMember?.EmployeeName;
            // Manually map images to ensure they're included
            dto.Images = issue.Images
                .OrderBy(img => img.Sequence)
                .Select(img => new QualityIssueImageDto
                {
                    QualityIssueImageId = img.QualityIssueImageId,
                    IssueId = img.IssueId,
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
