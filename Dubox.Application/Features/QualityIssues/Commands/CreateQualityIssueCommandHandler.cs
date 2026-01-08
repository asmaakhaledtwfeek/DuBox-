using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using System.Diagnostics;

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
            var module = PermissionModuleEnum.QualityIssues;
            var action = PermissionActionEnum.Create;
            var canCreate = await _visibilityService.CanPerformAsync(module , action, cancellationToken);
            if (!canCreate)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. Viewer role has read-only access and cannot create quality issues.");

            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);
            if (box is null)
                return Result.Failure<QualityIssueDetailsDto>("Box not found.");

            // Verify user has access to the project this box belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to create quality issues for this box.");

            var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(box.ProjectId, "create quality issues", cancellationToken);

            if (!projectStatusValidation.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(projectStatusValidation.Error!);

            var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(box.BoxId, "create quality issues", cancellationToken);

            if (!boxStatusValidation.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(projectStatusValidation.Error!);
            
            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId);

            var newIssue = request.Adapt<QualityIssue>();
          
            newIssue.CreatedDate = DateTime.UtcNow;
            newIssue.CreatedBy = currentUserId;
            newIssue.WIRId = null; 
            newIssue.Status = QualityIssueStatusEnum.Open;
            newIssue.ReportedBy = user?.FullName?? string.Empty;

            await _unitOfWork.Repository<QualityIssue>().AddAsync(newIssue, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
           
            (bool, string) imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<QualityIssueImage>(
                newIssue.IssueId, 
                request.Files, 
                request.ImageUrls, 
                cancellationToken,
                fileNames: request.FileNames);
            if (!imagesProcessResult.Item1)
                return Result.Failure<QualityIssueDetailsDto>(imagesProcessResult.Item2);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Create audit log for quality issue creation
            var auditLog = new AuditLog
            {
                TableName = nameof(QualityIssue),
                RecordId = newIssue.IssueId,
                Action = "INSERT",
                OldValues = null,
                NewValues = $"IssueType: {newIssue.IssueType}, Severity: {newIssue.Severity}, Status: {newIssue.Status}, IssueDescription: {newIssue.IssueDescription ?? "N/A"}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Quality Issue created for Box {box.BoxTag ?? box.BoxName}. Type: {newIssue.IssueType}, Severity: {newIssue.Severity}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(new QualityIssueDetailsDto());
        }
       
    }

}

