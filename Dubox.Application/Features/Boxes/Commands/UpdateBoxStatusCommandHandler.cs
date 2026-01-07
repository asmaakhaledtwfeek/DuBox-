using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Commands
{

    public class UpdateBoxStatusCommandHandler : IRequestHandler<UpdateBoxStatusCommand, Result<BoxDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly IDbContext _context;
        public UpdateBoxStatusCommandHandler(
            IUnitOfWork unitOfWork, 
            ICurrentUserService currentUserService,
            IProjectTeamVisibilityService visibilityService,
            IDbContext context)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _visibilityService = visibilityService;
            _context = context;
        }

        public async Task<Result<BoxDto>> Handle(UpdateBoxStatusCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<BoxDto>("Access denied. Viewer role has read-only access and cannot update box status.");
            }

            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);

            if (box == null)
                return Result.Failure<BoxDto>("Box not found.");

            // Check if project is archived
            var isArchived = await _visibilityService.IsProjectArchivedAsync(box.ProjectId, cancellationToken);
            if (isArchived)
            {
                return Result.Failure<BoxDto>("Cannot update box status in an archived project. Archived projects are read-only.");
            }

            // Check if project is on hold
            var isOnHold = await _visibilityService.IsProjectOnHoldAsync(box.ProjectId, cancellationToken);
            if (isOnHold)
            {
                return Result.Failure<BoxDto>("Cannot update box status in a project on hold. Projects on hold only allow project status changes.");
            }

            // Check if project is closed
            var isClosed = await _visibilityService.IsProjectClosedAsync(box.ProjectId, cancellationToken);
            if (isClosed)
            {
                return Result.Failure<BoxDto>("Cannot update box status in a closed project. Closed projects only allow project status changes.");
            }

            // Check if box is Dispatched - cannot change status
            if (box.Status == BoxStatusEnum.Dispatched)
            {
                return Result.Failure<BoxDto>("Cannot change status of a dispatched box. Dispatched boxes are read-only.");
            }

            var oldStatus = box.Status;
            var newStatus = (BoxStatusEnum)request.Status;

            if (oldStatus == newStatus)
                return Result.Success(box.Adapt<BoxDto>());

            // Validate transition from Completed or OnHold to Dispatched
            if ((oldStatus == BoxStatusEnum.Completed || oldStatus == BoxStatusEnum.OnHold) && 
                newStatus == BoxStatusEnum.Dispatched)
            {
                var validationResult = await ValidateBoxCanBeDispatched(box.BoxId, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<BoxDto>(validationResult.ErrorMessage);
                }
            }

            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            box.Status = newStatus;
            box.ModifiedDate = DateTime.UtcNow;
            box.ModifiedBy = currentUserId;

            _unitOfWork.Repository<Box>().Update(box);
            var log = new AuditLog
            {
                TableName = nameof(Box),
                RecordId = box.BoxId,
                Action = "StatusChange",
                OldValues = $"Status: {oldStatus.ToString()}",
                NewValues = $"Status: {newStatus.ToString()}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Box status changed manually from {oldStatus.ToString()} to {newStatus.ToString()}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(box.Adapt<BoxDto>());
        }

        /// <summary>
        /// Validates if a box can be dispatched
        /// Box cannot be dispatched if:
        /// 1. Has any quality issues in progress or open
        /// 2. Has WIR checkpoint not approved or conditionally approved
        /// 3. Has any activities with progress < 100%
        /// </summary>
        private async Task<(bool IsValid, string ErrorMessage)> ValidateBoxCanBeDispatched(Guid boxId, CancellationToken cancellationToken)
        {
            var errorMessages = new List<string>();

            // Check for quality issues in progress or open
            var qualityIssues = await _unitOfWork.Repository<QualityIssue>()
                .FindAsync(qi => qi.BoxId == boxId && 
                    (qi.Status == QualityIssueStatusEnum.Open || qi.Status == QualityIssueStatusEnum.InProgress), 
                    cancellationToken);

            if (qualityIssues.Any())
            {
                var issueCount = qualityIssues.Count();
                var issueText = issueCount == 1 ? "issue" : "issues";
                var verb = issueCount == 1 ? "is" : "are";
                errorMessages.Add($"Cannot dispatch box. There {verb} {issueCount} open or in-progress quality {issueText} that must be resolved first.");
            }

            // Check for WIR checkpoints not approved (Pending or Rejected)
            // Allow dispatch if checkpoints are Approved or ConditionalApproval
            var wirCheckpoints = await _unitOfWork.Repository<WIRCheckpoint>()
                .FindAsync(wir => wir.BoxId == boxId && 
                    (wir.Status != WIRCheckpointStatusEnum.Approved && wir.Status != WIRCheckpointStatusEnum.ConditionalApproval), 
                    cancellationToken);

            if (wirCheckpoints.Any())
            {
                var checkpointCount = wirCheckpoints.Count();
                var checkpointText = checkpointCount == 1 ? "checkpoint" : "checkpoints";
                
                // Group by status for clearer error message
                // Note: ConditionalApproval is now allowed, so we only show Pending and Rejected
                var pendingCount = wirCheckpoints.Count(w => w.Status == WIRCheckpointStatusEnum.Pending);
                var rejectedCount = wirCheckpoints.Count(w => w.Status == WIRCheckpointStatusEnum.Rejected);
                
                var statusDetails = new List<string>();
                if (pendingCount > 0) statusDetails.Add($"{pendingCount} pending");
                if (rejectedCount > 0) statusDetails.Add($"{rejectedCount} rejected");
                
                var statusText = string.Join(", ", statusDetails);
                var verb = checkpointCount == 1 ? "is" : "are";
                errorMessages.Add($"Cannot dispatch box. There {verb} {checkpointCount} WIR {checkpointText} ({statusText}) that must be approved or conditionally approved first.");
            }
            // Check for WIR Records that don't have corresponding WIR Checkpoints
            // Query through BoxActivities to get WIRRecords for this box
            var boxActivities = await _unitOfWork.Repository<BoxActivity>()
                .FindAsync(ba => ba.BoxId == boxId, cancellationToken);

            var boxActivityIds = boxActivities.Select(ba => ba.BoxActivityId).ToList();

            if (boxActivityIds.Any())
            {
                var allWIRRecords = await _unitOfWork.Repository<WIRRecord>()
                    .FindAsync(wir => boxActivityIds.Contains(wir.BoxActivityId), cancellationToken);

                if (allWIRRecords.Any())
                {
                    // Get all WIR Checkpoints for this box
                    var allWIRCheckpoints = await _unitOfWork.Repository<WIRCheckpoint>()
                        .FindAsync(wcp => wcp.BoxId == boxId, cancellationToken);

                    // Create a set of WIRCodes that have checkpoints
                    var checkpointWIRCodes = new HashSet<string>(
                        allWIRCheckpoints.Select(wcp => wcp.WIRCode),
                        StringComparer.OrdinalIgnoreCase
                    );

                    // Find WIR Records that don't have corresponding checkpoints
                    var recordsWithoutCheckpoints = allWIRRecords
                        .Where(wir => !checkpointWIRCodes.Contains(wir.WIRCode))
                        .ToList();

                    if (recordsWithoutCheckpoints.Any())
                    {
                        var recordCount = recordsWithoutCheckpoints.Count();
                        var recordText = recordCount == 1 ? "WIR Record" : "WIR Records";
                        var verb = recordCount == 1 ? "does" : "do";
                        var wirCodes = string.Join(", ", recordsWithoutCheckpoints.Select(wir => wir.WIRCode).Distinct());
                        errorMessages.Add($"Cannot dispatch box. There {verb} {recordCount} {recordText} ({wirCodes}) that must have corresponding WIR Checkpoints created first.");
                    }
                }
            }

            // Check for activities with progress < 100%
            var activities = await _unitOfWork.Repository<BoxActivity>()
                .FindAsync(ba => ba.BoxId == boxId && ba.IsActive, cancellationToken);

            var incompleteActivities = activities.Where(ba => ba.ProgressPercentage < 100).ToList();
            if (incompleteActivities.Any())
            {
                var activityCount = incompleteActivities.Count();
                var activityText = activityCount == 1 ? "activity" : "activities";
                var verb = activityCount == 1 ? "is" : "are";
                errorMessages.Add($"Cannot dispatch box. There {verb} {activityCount} {activityText} with progress less than 100% that must be completed first.");
            }
          
            if (errorMessages.Any())
            {
                return (false, string.Join(" ", errorMessages));
            }

            return (true, string.Empty);
        }
    }
}
