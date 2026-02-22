using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands
{
    public class CreateBoxExchangeRequestCommandHandler
        : IRequestHandler<CreateBoxExchangeRequestCommand, Result<BoxExchangeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly INotificationHubService _notificationHubService;

        public CreateBoxExchangeRequestCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IProjectTeamVisibilityService visibilityService,
            INotificationHubService notificationHubService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _visibilityService = visibilityService;
            _notificationHubService = notificationHubService;
        }

        public async Task<Result<BoxExchangeDto>> Handle(CreateBoxExchangeRequestCommand request, CancellationToken cancellationToken)
        {
            // Permission check
            var module = PermissionModuleEnum.Boxes;
            var action = PermissionActionEnum.Edit;
            var canModify = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
            if (!canModify)
                return Result.Failure<BoxExchangeDto>("Access denied. You do not have permission to create box exchange requests.");

            // Get box
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);
            if (box is null)
                return Result.Failure<BoxExchangeDto>("Box not found.");

            // Verify user has access to the project
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<BoxExchangeDto>("Access denied. You do not have permission to access this box.");

            // Validate at least one field is being changed
            if (string.IsNullOrWhiteSpace(request.NewBuildingNumber) && string.IsNullOrWhiteSpace(request.NewFloor))
                return Result.Failure<BoxExchangeDto>("At least one field (Building or Floor) must be specified for exchange.");

            // Validate target box if provided
            Box? targetBox = null;
            if (request.TargetBoxId.HasValue)
            {
                targetBox = await _unitOfWork.Repository<Box>().GetByIdAsync(request.TargetBoxId.Value);
                if (targetBox == null)
                    return Result.Failure<BoxExchangeDto>("Target box for exchange not found.");

                // Verify target box is in the same project
                if (targetBox.ProjectId != box.ProjectId)
                    return Result.Failure<BoxExchangeDto>("Target box must be in the same project.");

                // Verify target box has the same box type
                if (targetBox.ProjectBoxTypeId != box.ProjectBoxTypeId)
                    return Result.Failure<BoxExchangeDto>("Target box must have the same box type as the current box.");

                // Verify target box is in the requested building/floor
                var requestedBuilding = request.NewBuildingNumber ?? box.BuildingNumber;
                var requestedFloor = request.NewFloor ?? box.Floor;
                if (targetBox.BuildingNumber != requestedBuilding || targetBox.Floor != requestedFloor)
                    return Result.Failure<BoxExchangeDto>("Target box must be in the requested building and floor.");
            }

            // Get project to find project creator
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(box.ProjectId);
            if (project == null || !project.CreatedBy.HasValue)
                return Result.Failure<BoxExchangeDto>("Project or project creator not found.");

            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
                ? parsedUserId
                : Guid.Empty;

            var currentUser = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId);
            if (currentUser == null)
                return Result.Failure<BoxExchangeDto>("Current user not found.");

            // Generate new BoxTag if building or floor changed
            string? newBoxTag = null;
            if (!string.IsNullOrWhiteSpace(request.NewBuildingNumber) || !string.IsNullOrWhiteSpace(request.NewFloor))
            {
                var newBuilding = request.NewBuildingNumber ?? box.BuildingNumber ?? "";
                var newFloor = request.NewFloor ?? box.Floor;
                
                // Get box type and subtype codes
                var boxTypeCode = "";
                var boxSubTypeCode = "";
                
                if (box.ProjectBoxTypeId.HasValue)
                {
                    var boxType = await _unitOfWork.Repository<ProjectBoxType>()
                        .GetByIdAsync(box.ProjectBoxTypeId.Value);
                    if (boxType != null)
                        boxTypeCode = boxType.TypeName;
                }
                
                if (box.ProjectBoxSubTypeId.HasValue)
                {
                    var boxSubType = await _unitOfWork.Repository<ProjectBoxSubType>()
                        .GetByIdAsync(box.ProjectBoxSubTypeId.Value);
                    if (boxSubType != null)
                        boxSubTypeCode = boxSubType.SubTypeName;
                }
                
                // Generate new BoxTag: ProjectCode-Building-Floor-Type-SubType
                var parts = new List<string> { project.ProjectCode ?? "" };
                if (!string.IsNullOrWhiteSpace(newBuilding))
                    parts.Add(newBuilding);
                if (!string.IsNullOrWhiteSpace(newFloor))
                    parts.Add(newFloor);
                if (!string.IsNullOrWhiteSpace(boxTypeCode))
                    parts.Add(boxTypeCode);
                if (!string.IsNullOrWhiteSpace(boxSubTypeCode))
                    parts.Add(boxSubTypeCode);
                
                newBoxTag = string.Join("-", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
                
                
            }

            // Generate issue numbers
            var issueCountInProject = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesSpecification()).Data
                .Count(qi => qi.Box.ProjectId == box.ProjectId || qi.ProjectId == box.ProjectId);
            var issueNumber = (issueCountInProject + 1).ToString("D5");
            var secondaryIssueNumber = (issueCountInProject + 2).ToString("D5");

            // Create Primary Quality Issue (for current box - actionable)
            var newIssue = new QualityIssue
            {
                IssueNumber = issueNumber,
                BoxId = box.BoxId,
                ProjectId = box.ProjectId,
                IssueType = IssueTypeEnum.ExchangeRequest,
                Severity = SeverityEnum.Major,
                IssueDescription = targetBox != null
                    ? $"Box exchange request with {targetBox.BoxTag}: " +
                      $"Swap building and floor with box {targetBox.BoxTag}. " +
                      (string.IsNullOrWhiteSpace(request.RequestReason) ? "" : $"Reason: {request.RequestReason}")
                    : $"Box exchange request: " +
                      (request.NewBuildingNumber != null ? $"Building: {box.BuildingNumber} → {request.NewBuildingNumber}. " : "") +
                      (request.NewFloor != null ? $"Floor: {box.Floor} → {request.NewFloor}. " : "") +
                      (string.IsNullOrWhiteSpace(request.RequestReason) ? "" : $"Reason: {request.RequestReason}"),
                NCR = NCRTypeEnum.Internal,
                AssignedUserId = project.CreatedBy.Value,
                DueDate = DateTime.UtcNow.AddDays(7), // 7 days to review
                Status = QualityIssueStatusEnum.Open,
                IssueDate = DateTime.UtcNow,
                ReportedBy = currentUser.FullName,
                CreatedBy = currentUserId,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Repository<QualityIssue>().AddAsync(newIssue, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Create Secondary Quality Issue (for target box - read-only)
            QualityIssue? secondaryIssue = null;
            if (targetBox != null)
            {
                secondaryIssue = new QualityIssue
                {
                    IssueNumber = secondaryIssueNumber,
                    BoxId = targetBox.BoxId,
                    ProjectId = targetBox.ProjectId,
                    IssueType = IssueTypeEnum.ExchangeRequest,
                    Severity = SeverityEnum.Major,
                    IssueDescription = $"Box exchange - Linked to {box.BoxTag}: " +
                        $"This is a secondary issue for box exchange. " +
                        $"No action required - will be automatically resolved when issue #{issueNumber} is resolved. " +
                        $"Building and floor will be swapped with {box.BoxTag}.",
                    NCR = NCRTypeEnum.Internal,
                    AssignedUserId = null, // No assignment - read-only
                    DueDate = DateTime.UtcNow.AddDays(7),
                    Status = QualityIssueStatusEnum.Open,
                    IssueDate = DateTime.UtcNow,
                    ReportedBy = currentUser.FullName,
                    CreatedBy = currentUserId,
                    CreatedDate = DateTime.UtcNow,
                    IsReadOnly = true // Mark as read-only
                };

                await _unitOfWork.Repository<QualityIssue>().AddAsync(secondaryIssue, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            // Create BoxExchange entity
            var boxExchange = new BoxExchange
            {
                BoxId = box.BoxId,
                QualityIssueId = newIssue.IssueId,
                ExchangedWithBoxId = targetBox?.BoxId,
                SecondaryQualityIssueId = secondaryIssue?.IssueId,
                OldBuildingNumber = box.BuildingNumber,
                OldFloor = box.Floor,
                OldBoxTag = box.BoxTag,
                NewBuildingNumber = request.NewBuildingNumber,
                NewFloor = request.NewFloor,
                NewBoxTag = newBoxTag,
                RequestReason = request.RequestReason,
                Status = ExchangeRequestStatusEnum.Pending,
                RequestedDate = DateTime.UtcNow,
                RequestedBy = currentUserId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserId
            };

            await _unitOfWork.Repository<BoxExchange>().AddAsync(boxExchange, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Create audit log
            var auditLog = new AuditLog
            {
                TableName = nameof(BoxExchange),
                RecordId = boxExchange.BoxExchangeId,
                Action = "INSERT",
                OldValues = $"Building: {box.BuildingNumber}, Floor: {box.Floor}, BoxTag: {box.BoxTag}",
                NewValues = targetBox != null
                    ? $"Exchange with Box: {targetBox.BoxTag}, Building: {targetBox.BuildingNumber}, Floor: {targetBox.Floor}"
                    : $"Building: {request.NewBuildingNumber ?? "N/A"}, Floor: {request.NewFloor ?? "N/A"}, BoxTag: {newBoxTag ?? "N/A"}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = targetBox != null
                    ? $"Box exchange request created for Box {box.BoxTag} to swap with {targetBox.BoxTag}. Quality Issues {issueNumber} and {secondaryIssueNumber} created."
                    : $"Box exchange request created for Box {box.BoxTag}. Quality Issue {issueNumber} assigned to project creator."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Send notification to project creator
            var projectCreator = await _unitOfWork.Repository<User>().GetByIdAsync(project.CreatedBy.Value);
            if (projectCreator != null)
            {
                var notificationMessage = targetBox != null
                    ? $"{currentUser.FullName} requested to exchange box {box.BoxTag} with {targetBox.BoxTag}. Please review Quality Issue {issueNumber}."
                    : $"{currentUser.FullName} requested to exchange box {box.BoxTag}. Please review Quality Issue {issueNumber}.";

                var notification = new Notification
                {
                    NotificationType = "BoxExchangeRequest",
                    Priority = "High",
                    Title = $"Box Exchange Request - {box.BoxTag}",
                    Message = notificationMessage,
                    RelatedIssueId = newIssue.IssueId,
                    DirectLink = $"/projects/{box.ProjectId}/boxes/{box.BoxId}?tab=quality-issues&issueId={newIssue.IssueId}",
                    RecipientUserId = project.CreatedBy.Value,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(30)
                };

                await _unitOfWork.Repository<Notification>().AddAsync(notification, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                // Send real-time notification via SignalR
                try
                {
                    await _notificationHubService.SendNotificationToUserAsync(
                        project.CreatedBy.Value,
                        new
                        {
                            notification.NotificationId,
                            notification.NotificationType,
                            notification.Title,
                            notification.Message,
                            notification.DirectLink,
                            notification.CreatedDate
                        });

                    // Get updated unread count
                    var unreadCount = await _unitOfWork.Repository<Notification>()
                        .CountAsync(n => n.RecipientUserId == project.CreatedBy.Value
                            && !n.IsRead
                            && (!n.ExpiryDate.HasValue || n.ExpiryDate >= DateTime.UtcNow),
                            cancellationToken);

                    await _notificationHubService.SendNotificationCountUpdateAsync(
                        project.CreatedBy.Value,
                        unreadCount);
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the operation
                    Console.WriteLine($"Error sending SignalR notification: {ex.Message}");
                }
            }

            // Return DTO
            var dto = boxExchange.Adapt<BoxExchangeDto>();
            dto.BoxTag = box.BoxTag;
            dto.BoxName = box.BoxName;
            dto.IssueNumber = issueNumber;
            dto.RequestedByName = currentUser.FullName;

            return Result.Success(dto);
        }
    }
}
