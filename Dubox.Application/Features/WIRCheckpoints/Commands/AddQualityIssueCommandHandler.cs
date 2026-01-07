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
    public class AddQualityIssueCommandHandler
        : IRequestHandler<AddQualityIssueCommand, Result<WIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public AddQualityIssueCommandHandler(
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

        public async Task<Result<WIRCheckpointDto>> Handle(AddQualityIssueCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<WIRCheckpointDto>("Access denied. Viewer role has read-only access and cannot add quality issues.");
            }

            var wir = _unitOfWork.Repository<WIRCheckpoint>()
                .GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            if (wir is null)
                return Result.Failure<WIRCheckpointDto>("WIRCheckpoint not found.");

            // Verify user has access to the project this WIR checkpoint belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(wir.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<WIRCheckpointDto>("Access denied. You do not have permission to add quality issues to this WIR checkpoint.");
            }

            // Check if box is dispatched - no actions allowed on dispatched boxes
            if (wir.Box.Status == BoxStatusEnum.Dispatched)
            {
                return Result.Failure<WIRCheckpointDto>("Cannot add quality issue. The box is dispatched and no actions are allowed on checkpoints. Only viewing is permitted.");
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

            // Create a single quality issue
            var newIssue = new QualityIssue
            {
                WIRId = wir.WIRId,
                BoxId = wir.BoxId,
                IssueType = request.IssueType,
                Severity = request.Severity,
                IssueDescription = request.IssueDescription,
                AssignedToTeamId = request.AssignedTo,
                AssignedToMemberId = request.AssignedToUserId,
                DueDate = request.DueDate,
                Status = QualityIssueStatusEnum.Open,
                IssueDate = DateTime.UtcNow,
                ReportedBy = reportedBy,
                CreatedBy = currentUserId,
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

            (bool, string) imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<QualityIssueImage>(newIssue.IssueId, request.Files, request.ImageUrls, cancellationToken, fileNames: request.FileNames);
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
                return Result.Failure<WIRCheckpointDto>($"Database error while saving images: {dbEx.Message}. Inner exception: {dbEx.InnerException?.Message}. " +
                    $"Make sure the QualityIssueImages table exists and the IssueId is valid.");
            }
            catch (Exception ex)
            {
                return Result.Failure<WIRCheckpointDto>($"Error saving images: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
            }

            // Create audit log for quality issue creation
            var auditLog = new AuditLog
            {
                TableName = nameof(QualityIssue),
                RecordId = newIssue.IssueId,
                Action = "INSERT",
                OldValues = null,
                NewValues = $"WIRId: {wir.WIRCode}, IssueType: {newIssue.IssueType}, Severity: {newIssue.Severity}, Status: {newIssue.Status}, IssueDescription: {newIssue.IssueDescription ?? "N/A"}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Quality Issue added to WIR Checkpoint {wir.WIRCode}. Type: {newIssue.IssueType}, Severity: {newIssue.Severity}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Reload checkpoint with images to include them in DTO
            wir = _unitOfWork.Repository<WIRCheckpoint>()
                .GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            var dto = wir.Adapt<WIRCheckpointDto>();

            return Result.Success(dto);
        }
    }
}

