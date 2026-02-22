using Dubox.Application.DTOs;
using Dubox.Application.Features.IssueComments.Commands;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public class UpdateQualityIssueStatusCommandHandler : IRequestHandler<UpdateQualityIssueStatusCommand, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly IMediator _mediator;

        public UpdateQualityIssueStatusCommandHandler(
            IUnitOfWork unitOfWork,
            IDbContext dbContext,
            ICurrentUserService currentUserService,
            IImageProcessingService imageProcessingService,
            IProjectTeamVisibilityService visibilityService,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _imageProcessingService = imageProcessingService;
            _visibilityService = visibilityService;
            _mediator = mediator;
        }

        public async Task<Result<QualityIssueDetailsDto>> Handle(UpdateQualityIssueStatusCommand request, CancellationToken cancellationToken)
        {
           var module= PermissionModuleEnum.QualityIssues;
            var action = PermissionActionEnum.Edit;
            var canModify = await _visibilityService.CanPerformAsync(module, action,cancellationToken);
            if (!canModify)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to modify quality issues.");

            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found.");

            // Check if issue is read-only (cannot be manually modified)
            if (issue.IsReadOnly)
                return Result.Failure<QualityIssueDetailsDto>("This quality issue is read-only and cannot be modified manually. It will be automatically updated when the linked exchange request is processed.");

            // Verify user has access to the project this quality issue belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(issue.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to modify this quality issue.");

            var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(issue.Box.ProjectId, "update quality issue", cancellationToken);
            if (!projectStatusValidation.IsSuccess)
                return Result.Failure<QualityIssueDetailsDto>(projectStatusValidation.Error!);

            var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(issue.BoxId.Value, "update quality issues", cancellationToken);
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

            // When quality issue is resolved/closed, auto-update linked panel to In Progress
            if (request.Status == QualityIssueStatusEnum.Resolved ||
                request.Status == QualityIssueStatusEnum.Closed)
            {
                var linkedPanels = await _dbContext.BoxPanels
                    .Include(p => p.Box)
                    .Where(p => p.QualityIssueId == issue.IssueId)
                    .ToListAsync(cancellationToken);
                foreach (var panel in linkedPanels)
                {
                    // Only update if box is not dispatched (read-only)
                    if (panel.Box.Status != BoxStatusEnum.Dispatched)
                    {
                        if(panel.CurrentStage == 0)
                        {
                            panel.PanelStatus = PanelStatusEnum.NotStarted;
                            panel.WorkflowStatus = "NotStarted";
                            panel.QualityIssueId = null;
                        }
                        // Reset panel to InProgress workflow status
                        else
                        {
                            panel.PanelStatus = PanelStatusEnum.InProgress;
                            panel.WorkflowStatus = "InProgress";
                            panel.QualityIssueId = null;
                        }

                        // Convert rejected approval status to approved once issue is resolved
                        // If FirstApprovalStatus was "Rejected", automatically approve it
                        if (panel.FirstApprovalStatus == "Rejected")
                        {
                            panel.FirstApprovalStatus = "Approved";
                            panel.FirstApprovalBy = currentUserId;
                            panel.FirstApprovalDate = DateTime.UtcNow;
                            panel.FirstApprovalNotes = "Auto-approved after quality issue resolved";
                        }
                        
                        // If SecondApprovalStatus was "Rejected", automatically approve it
                        // Note: This should only happen if FirstApproval was previously Approved
                        if (panel.SecondApprovalStatus == "Rejected")
                        {
                            panel.SecondApprovalStatus = "Approved";
                            panel.SecondApprovalBy = currentUserId;
                            panel.SecondApprovalDate = DateTime.UtcNow;
                            panel.SecondApprovalNotes = "Auto-approved after quality issue resolved";
                        }
                        
                        panel.ModifiedDate = DateTime.UtcNow;
                        panel.ModifiedBy = currentUserId;
                    }
                }
                await _unitOfWork.CompleteAsync(cancellationToken);

                // Handle BoxExchange requests when resolved/closed
                if (issue.IssueType == IssueTypeEnum.ExchangeRequest)
                {
                    var boxExchange =  _unitOfWork.Repository<BoxExchange>()
                        .FindAsync(be => be.QualityIssueId == issue.IssueId, cancellationToken).Result.FirstOrDefault();

                    if (boxExchange != null && boxExchange.Status == ExchangeRequestStatusEnum.Pending)
                    {
                        // Get the primary box
                        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxExchange.BoxId);
                        if (box != null)
                        {
                            // Capture old values for audit
                            var oldBoxTag = box.BoxTag;
                            var oldBuilding = box.BuildingNumber;
                            var oldFloor = box.Floor;

                            // Check if this is a two-way exchange (with target box)
                            if (boxExchange.ExchangedWithBoxId.HasValue)
                            {
                                // Two-way exchange: Swap building and floor between boxes
                                var targetBox = await _unitOfWork.Repository<Box>().GetByIdAsync(boxExchange.ExchangedWithBoxId.Value);
                                if (targetBox != null)
                                {
                                    // Capture target box old values
                                    var targetOldBoxTag = targetBox.BoxTag;
                                    var targetOldBuilding = targetBox.BuildingNumber;
                                    var targetOldFloor = targetBox.Floor;

                                    // Swap building and floor
                                    var tempBuilding = box.BuildingNumber;
                                    var tempFloor = box.Floor;
                                    
                                    box.BuildingNumber = targetBox.BuildingNumber;
                                    box.Floor = targetBox.Floor;
                                    box.ModifiedDate = DateTime.UtcNow;
                                    box.ModifiedBy = currentUserId;

                                    targetBox.BuildingNumber = tempBuilding;
                                    targetBox.Floor = tempFloor;
                                    targetBox.ModifiedDate = DateTime.UtcNow;
                                    targetBox.ModifiedBy = currentUserId;

                                    // Update box tags for both boxes
                                    var project = await _unitOfWork.Repository<Project>().GetByIdAsync(box.ProjectId);
                                    if (project != null)
                                    {
                                        // Update primary box tag
                                        var boxTypeCode = "";
                                        var boxSubTypeCode = "";
                                        
                                        if (box.ProjectBoxTypeId.HasValue)
                                        {
                                            var boxType = await _unitOfWork.Repository<ProjectBoxType>()
                                                .GetByIdAsync(box.ProjectBoxTypeId.Value);
                                            if (boxType != null)
                                                boxTypeCode = boxType.Abbreviation ?? boxType.TypeName;
                                        }
                                        
                                        if (box.ProjectBoxSubTypeId.HasValue)
                                        {
                                            var boxSubType = await _unitOfWork.Repository<ProjectBoxSubType>()
                                                .GetByIdAsync(box.ProjectBoxSubTypeId.Value);
                                            if (boxSubType != null)
                                                boxSubTypeCode = boxSubType.Abbreviation ?? boxSubType.SubTypeName;
                                        }
                                        
                                        var parts = new List<string> { project.ProjectCode ?? "" };
                                        if (!string.IsNullOrWhiteSpace(box.BuildingNumber))
                                            parts.Add(box.BuildingNumber);
                                        if (!string.IsNullOrWhiteSpace(box.Floor))
                                            parts.Add(box.Floor);
                                        if (!string.IsNullOrWhiteSpace(boxTypeCode))
                                            parts.Add(boxTypeCode);
                                        if (!string.IsNullOrWhiteSpace(boxSubTypeCode))
                                            parts.Add(boxSubTypeCode);
                                        
                                        box.BoxTag = string.Join("-", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                                        // Update target box tag
                                        var targetParts = new List<string> { project.ProjectCode ?? "" };
                                        if (!string.IsNullOrWhiteSpace(targetBox.BuildingNumber))
                                            targetParts.Add(targetBox.BuildingNumber);
                                        if (!string.IsNullOrWhiteSpace(targetBox.Floor))
                                            targetParts.Add(targetBox.Floor);
                                        if (!string.IsNullOrWhiteSpace(boxTypeCode))
                                            targetParts.Add(boxTypeCode);
                                        if (!string.IsNullOrWhiteSpace(boxSubTypeCode))
                                            targetParts.Add(boxSubTypeCode);
                                        
                                        targetBox.BoxTag = string.Join("-", targetParts.Where(p => !string.IsNullOrWhiteSpace(p)));
                                    }

                                    _unitOfWork.Repository<Box>().Update(box);
                                    _unitOfWork.Repository<Box>().Update(targetBox);

                                    // Update secondary quality issue to resolved/closed
                                    if (boxExchange.SecondaryQualityIssueId.HasValue)
                                    {
                                        var secondaryIssue = await _unitOfWork.Repository<QualityIssue>()
                                            .GetByIdAsync(boxExchange.SecondaryQualityIssueId.Value);
                                        if (secondaryIssue != null)
                                        {
                                            secondaryIssue.Status = request.Status; // Same status as primary issue
                                            secondaryIssue.ResolutionDescription = $"Auto-resolved: Exchange with {box.BoxTag} was completed.";
                                            secondaryIssue.ResolutionDate = DateTime.UtcNow;
                                            secondaryIssue.UpdatedBy = currentUserId;
                                            _unitOfWork.Repository<QualityIssue>().Update(secondaryIssue);
                                        }
                                    }

                                    // Create audit logs for both boxes
                                    var boxAuditLog = new AuditLog
                                    {
                                        TableName = nameof(Box),
                                        RecordId = box.BoxId,
                                        Action = "UPDATE",
                                        OldValues = $"BoxTag: {oldBoxTag}, BuildingNumber: {oldBuilding ?? "N/A"}, Floor: {oldFloor}",
                                        NewValues = $"BoxTag: {box.BoxTag}, BuildingNumber: {box.BuildingNumber ?? "N/A"}, Floor: {box.Floor}",
                                        ChangedBy = currentUserId,
                                        ChangedDate = DateTime.UtcNow,
                                        Description = $"Box exchanged with {targetBox.BoxTag}. Quality Issue {issue.IssueNumber} was resolved."
                                    };
                                    await _unitOfWork.Repository<AuditLog>().AddAsync(boxAuditLog, cancellationToken);

                                    var targetBoxAuditLog = new AuditLog
                                    {
                                        TableName = nameof(Box),
                                        RecordId = targetBox.BoxId,
                                        Action = "UPDATE",
                                        OldValues = $"BoxTag: {targetOldBoxTag}, BuildingNumber: {targetOldBuilding ?? "N/A"}, Floor: {targetOldFloor}",
                                        NewValues = $"BoxTag: {targetBox.BoxTag}, BuildingNumber: {targetBox.BuildingNumber ?? "N/A"}, Floor: {targetBox.Floor}",
                                        ChangedBy = currentUserId,
                                        ChangedDate = DateTime.UtcNow,
                                        Description = $"Box exchanged with {box.BoxTag}. Building and floor swapped."
                                    };
                                    await _unitOfWork.Repository<AuditLog>().AddAsync(targetBoxAuditLog, cancellationToken);
                                }
                            }
                            else
                            {
                                // One-way exchange: Update box with new values
                                if (!string.IsNullOrWhiteSpace(boxExchange.NewBuildingNumber))
                                    box.BuildingNumber = boxExchange.NewBuildingNumber;

                                if (!string.IsNullOrWhiteSpace(boxExchange.NewFloor))
                                    box.Floor = boxExchange.NewFloor;

                                if (!string.IsNullOrWhiteSpace(boxExchange.NewBoxTag))
                                    box.BoxTag = boxExchange.NewBoxTag;

                                box.ModifiedDate = DateTime.UtcNow;
                                box.ModifiedBy = currentUserId;

                                _unitOfWork.Repository<Box>().Update(box);

                                // Create audit log for box update
                                var boxAuditLog = new AuditLog
                                {
                                    TableName = nameof(Box),
                                    RecordId = box.BoxId,
                                    Action = "UPDATE",
                                    OldValues = $"BoxTag: {oldBoxTag}, BuildingNumber: {oldBuilding ?? "N/A"}, Floor: {oldFloor}",
                                    NewValues = $"BoxTag: {box.BoxTag}, BuildingNumber: {box.BuildingNumber ?? "N/A"}, Floor: {box.Floor}",
                                    ChangedBy = currentUserId,
                                    ChangedDate = DateTime.UtcNow,
                                    Description = $"Box updated via exchange request. Quality Issue {issue.IssueNumber} was resolved."
                                };
                                await _unitOfWork.Repository<AuditLog>().AddAsync(boxAuditLog, cancellationToken);
                            }

                            // Update BoxExchange status
                            boxExchange.Status = ExchangeRequestStatusEnum.Applied;
                            boxExchange.AppliedDate = DateTime.UtcNow;
                            boxExchange.AppliedBy = currentUserId;

                            _unitOfWork.Repository<BoxExchange>().Update(boxExchange);

                            // Create audit log for box exchange update
                            var exchangeAuditLog = new AuditLog
                            {
                                TableName = nameof(BoxExchange),
                                RecordId = boxExchange.BoxExchangeId,
                                Action = "UPDATE",
                                OldValues = $"Status: {ExchangeRequestStatusEnum.Pending}",
                                NewValues = $"Status: {ExchangeRequestStatusEnum.Applied}",
                                ChangedBy = currentUserId,
                                ChangedDate = DateTime.UtcNow,
                                Description = $"Box exchange applied. Box {oldBoxTag} updated."
                            };
                            await _unitOfWork.Repository<AuditLog>().AddAsync(exchangeAuditLog, cancellationToken);
                            await _unitOfWork.CompleteAsync(cancellationToken);
                        }
                    }
                }
            }

            // Add comment if provided
            if (!string.IsNullOrWhiteSpace(request.Comment))
            {
                var addCommentCommand = new AddCommentCommand(
                    IssueId: issue.IssueId,
                    ParentCommentId: null,
                    CommentText: request.Comment,
                    IsStatusUpdateComment: true,
                    RelatedStatus: request.Status
                );

                // Fire and forget - don't fail status update if comment fails
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _mediator.Send(addCommentCommand, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding status update comment: {ex.Message}");
                    }
                }, CancellationToken.None);
            }

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
