using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Dubox.Application.Features.ProgressUpdates.Commands;

public class CreateProgressUpdateCommandHandler : IRequestHandler<CreateProgressUpdateCommand, Result<ProgressUpdateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public CreateProgressUpdateCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService,
        IImageProcessingService imageProcessingService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _imageProcessingService = imageProcessingService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<ProgressUpdateDto>> Handle(CreateProgressUpdateCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Check permissions
        var module = PermissionModuleEnum.ProgressUpdates;
        var action = PermissionActionEnum.Create;
        var canUpdate = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
        if (!canUpdate)
            return Result.Failure<ProgressUpdateDto>("Access denied. You do not have permission to update activities progress.");

        // Step 2: Get box activity with includes
        var boxActivity = _unitOfWork.Repository<BoxActivity>()
            .GetEntityWithSpec(new BoxActivitiesWithIncludesSpecification(request.BoxActivityId, request.BoxId));

        // Step 3: Validate the request
        var validationResult = await ValidateProgressUpdateAsync(request, boxActivity, cancellationToken);
        if (validationResult.IsFailure)
            return Result.Failure<ProgressUpdateDto>(validationResult.Error);

        // Step 4: Get current user
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);
        if (user == null)
            return Result.Failure<ProgressUpdateDto>("User not found");

        // Step 5: Determine inferred status based on progress percentage
        BoxStatusEnum inferredStatus = DetermineInferredStatus(request.ProgressPercentage, boxActivity);

        // Step 6: Check for duplicate progress update
        var duplicatedProgress = _unitOfWork.Repository<ProgressUpdate>().GetEntityWithSpec(
            new GetProgressUpdatesByActivitySpecification(request.BoxId, request.BoxActivityId, request.ProgressPercentage, inferredStatus));

        if (duplicatedProgress != null)
        {
            // Process WIR even for duplicates (position might be new)
            var wirResult = await CreateWIRRecordOrUpdatePositionIfWIRExist(request, boxActivity, currentUserId, cancellationToken);
            if (wirResult.IsFailure)
                return Result.Failure<ProgressUpdateDto>(wirResult.Error);

            await _unitOfWork.CompleteAsync(cancellationToken);

            var duplicatedDto = duplicatedProgress.Adapt<ProgressUpdateDto>() with
            {
                BoxTag = boxActivity.Box.BoxTag,
                ActivityName = GetActivityName(boxActivity),
                UpdatedByName = user.FullName ?? user.Email,
            };

            return Result.Success(duplicatedDto);
        }

        // Step 7: Create new progress update
        var progressUpdate = request.Adapt<ProgressUpdate>();
        progressUpdate.Status = inferredStatus;
        progressUpdate.UpdatedBy = currentUserId;
        progressUpdate.UpdateDate = DateTime.UtcNow;
        progressUpdate.CreatedDate = DateTime.UtcNow;

        // Sanitize UpdateMethod
        if (string.IsNullOrEmpty(progressUpdate.UpdateMethod))
        {
            progressUpdate.UpdateMethod = "Web";
        }
        else if (progressUpdate.UpdateMethod.Length > 50)
        {
            progressUpdate.UpdateMethod = progressUpdate.UpdateMethod.Substring(0, 50);
        }

        // Sanitize DeviceInfo
        if (!string.IsNullOrEmpty(progressUpdate.DeviceInfo) && progressUpdate.DeviceInfo.Length > 100)
        {
            progressUpdate.DeviceInfo = progressUpdate.DeviceInfo.Substring(0, 100);
        }

        try
        {
            // Step 8: Save progress update
            await _unitOfWork.Repository<ProgressUpdate>().AddAsync(progressUpdate, cancellationToken);

            // Step 9: Process WIR record creation/update
            var wirResult = await CreateWIRRecordOrUpdatePositionIfWIRExist(request, boxActivity, currentUserId, cancellationToken);
            if (wirResult.IsFailure)
                return Result.Failure<ProgressUpdateDto>(wirResult.Error);

            await _unitOfWork.CompleteAsync(cancellationToken);

            // Step 10: Update box activity and create audit log
            await UpdateBoxActivityAndAddAuditLog(request, boxActivity, inferredStatus, currentUserId, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Step 11: Update box progress and get snapshot
            progressUpdate.BoxProgressSnapshot = await UpdateBoxProgress(request.BoxId, currentUserId, cancellationToken);
            _unitOfWork.Repository<ProgressUpdate>().Update(progressUpdate);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Step 12: Update project progress
            if (boxActivity.Box != null)
            {
                await UpdateProjectProgress(boxActivity.Box.ProjectId, currentUserId, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }
        }
        catch (DbUpdateException ex)
        {
            if (IsUniqueConstraintViolation(ex))
            {
                return Result.Failure<ProgressUpdateDto>(
                    "A position conflict occurred while saving. Another box has taken this location. Please refresh and try again.");
            }
            return Result.Failure<ProgressUpdateDto>($"Failed to save progress update: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Failure<ProgressUpdateDto>($"Failed to save progress update: {ex.Message}");
        }

        // Step 13: Process images (ONLY for this SPECIFIC ACTIVITY)
        var progressUpdatesForActivity = _dbContext.ProgressUpdates
            .Where(pu => pu.BoxActivityId == progressUpdate.BoxActivityId)
            .Select(pu => pu.ProgressUpdateId)
            .ToList();

        var existingImagesForActivity = _dbContext.Set<ProgressUpdateImage>()
            .Where(img => progressUpdatesForActivity.Contains(img.ProgressUpdateId))
            .ToList();

        Console.WriteLine($"🔍 VERSION DEBUG - Found {existingImagesForActivity.Count} existing ProgressUpdateImages for Activity {progressUpdate.BoxActivityId}");
        Console.WriteLine($"🔍 VERSION DEBUG - Images will only be versioned if they have same filename AND same activity");

        var imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<ProgressUpdateImage>(
            progressUpdate.ProgressUpdateId,
            request.Files,
            request.ImageUrls,
            cancellationToken,
            fileNames: request.FileNames,
            existingImagesForVersioning: existingImagesForActivity
        );

        if (!imagesProcessResult.IsSuccess)
            return Result.Failure<ProgressUpdateDto>(imagesProcessResult.ErrorMessage);

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Step 14: Return success with DTO
        var dto = progressUpdate.Adapt<ProgressUpdateDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityName = GetActivityName(boxActivity),
            UpdatedByName = user.FullName ?? user.Email,
        };

        return Result.Success(dto);
    }

    #region WIR Position Management

    /// <summary>
    /// Creates WIR record or updates position if WIR exists.
    /// This is the main orchestration method for WIR position management.
    /// </summary>
    private async Task<Result> CreateWIRRecordOrUpdatePositionIfWIRExist(
        CreateProgressUpdateCommand request,
        BoxActivity currentActivity,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        // Step 1: Determine the target WIR activity
        var targetWIRActivity = GetTargetWIRActivity(currentActivity);
        if (targetWIRActivity == null)
            return Result.Success(); // No WIR activity to process

        // Step 2: Validate previous WIR approval (if position data provided)
        bool hasPositionData = HasPositionData(request);
        if (hasPositionData)
        {
            var previousWIRValidation = await ValidatePreviousWIRApprovedAsync(
                targetWIRActivity,
                currentActivity.BoxId,
                cancellationToken);

            if (previousWIRValidation.IsFailure)
                return previousWIRValidation;
        }

        // Step 3: Validate and determine target section (if position data provided)
        Guid? targetSectionId = null;
        if (hasPositionData)
        {
            var positionValidation = await ValidateAndDetermineSection(
                request,
                currentActivity.BoxId,
                cancellationToken);

            if (positionValidation.IsFailure)
                return positionValidation;

            targetSectionId = positionValidation.Data.Value;
        }

        // Step 4: Get or create WIR record
        var wirRecord = await GetOrCreateWIRRecord(
            targetWIRActivity,
            currentUserId,
            cancellationToken);

        // Step 5: Update WIR position (only if not already set)
        var wirUpdateResult = UpdateWIRPositionIfEmpty(
            wirRecord,
            request,
            targetSectionId,
            out bool wirPositionChanged);

        if (wirUpdateResult.IsFailure)
            return wirUpdateResult;

        if (wirPositionChanged)
        {
            wirRecord.ModifiedDate = DateTime.UtcNow;
            _unitOfWork.Repository<WIRRecord>().Update(wirRecord);

            // Create WIR audit log
            await CreateWIRAuditLog(wirRecord, request, currentUserId, cancellationToken);
        }

        // Step 6: Synchronize Box position with latest WIR (if WIR has position)
        if (HasWIRPosition(wirRecord))
        {
            var boxUpdateResult = await SynchronizeBoxPositionWithWIR(
                wirRecord,
                currentActivity.BoxId,
                currentUserId,
                cancellationToken);

            if (boxUpdateResult.IsFailure)
                return boxUpdateResult;
        }

        return Result.Success();
    }

    /// <summary>
    /// Determines the target WIR activity - either current activity if it's a WIR checkpoint,
    /// or the next WIR checkpoint in sequence
    /// </summary>
    private BoxActivity? GetTargetWIRActivity(BoxActivity currentActivity)
    {
        bool isCurrentWIRCheckpoint = GetIsWIRCheckpoint(currentActivity);

        if (isCurrentWIRCheckpoint)
            return currentActivity;

        return _unitOfWork.Repository<BoxActivity>()
            .GetEntityWithSpec(new BoxActivitiesWithIncludesSpecification(currentActivity));
    }

    /// <summary>
    /// Validates position data and determines target factory section.
    /// Returns the section ID if validation passes.
    /// </summary>
    private async Task<Result<Guid?>> ValidateAndDetermineSection(
        CreateProgressUpdateCommand request,
        Guid boxId,
        CancellationToken cancellationToken)
    {
        string bayValue = request.WirBay?.Trim() ?? string.Empty;
        string rowValue = request.WirRow?.Trim() ?? string.Empty;
        string positionValue = request.WirPosition?.Trim() ?? string.Empty;

        // Use section ID from request if provided (user selection from UI)
        Guid? targetSectionId = request.WirFactorySectionId;

        // If section not provided, validate position falls within a valid section
        if (!targetSectionId.HasValue)
        {
            var sectionValidation = await ValidatePositionWithinFactorySectionsAsync(
                bayValue,
                rowValue,
                boxId,
                cancellationToken);

            if (sectionValidation.IsFailure)
                return Result.Failure<Guid?>(sectionValidation.Error);

            targetSectionId = sectionValidation.Data.Value;
        }

        // Validate position is not already occupied by another box
        var uniquenessValidation = await ValidateUniqueLocationInFactoryAsync(
            bayValue,
            rowValue,
            positionValue,
            boxId,
            targetSectionId,
            cancellationToken);

        if (uniquenessValidation.IsFailure)
            return Result.Failure<Guid?>(uniquenessValidation.Error);

        return Result.Success(targetSectionId);
    }

    /// <summary>
    /// Gets existing WIR record or creates a new one
    /// </summary>
    private async Task<WIRRecord> GetOrCreateWIRRecord(
        BoxActivity targetWIRActivity,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        var existingWIR = _unitOfWork.Repository<WIRRecord>()
            .Get()
            .FirstOrDefault(w => w.BoxActivityId == targetWIRActivity.BoxActivityId);

        if (existingWIR != null)
            return existingWIR;

        // Create new WIR record
        string wirCode = GetWIRCode(targetWIRActivity) ?? "WIR";

        var newWIR = new WIRRecord
        {
            BoxActivityId = targetWIRActivity.BoxActivityId,
            WIRCode = wirCode,
            Status = WIRRecordStatusEnum.Pending,
            RequestedDate = DateTime.UtcNow,
            RequestedBy = currentUserId,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<WIRRecord>().AddAsync(newWIR, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return newWIR;
    }

    /// <summary>
    /// Updates WIR position fields ONLY if they are currently empty.
    /// Returns true if any field was updated.
    /// </summary>
    private Result UpdateWIRPositionIfEmpty(
        WIRRecord wirRecord,
        CreateProgressUpdateCommand request,
        Guid? targetSectionId,
        out bool positionChanged)
    {
        positionChanged = false;

        // If no position data in request, nothing to update
        if (!HasPositionData(request))
            return Result.Success();

        // Update Bay (only if currently empty)
        if (!string.IsNullOrWhiteSpace(request.WirBay) && string.IsNullOrWhiteSpace(wirRecord.Bay))
        {
            wirRecord.Bay = TruncateString(request.WirBay.Trim(), 50);
            positionChanged = true;
        }

        // Update Row (only if currently empty)
        if (!string.IsNullOrWhiteSpace(request.WirRow) && string.IsNullOrWhiteSpace(wirRecord.Row))
        {
            wirRecord.Row = TruncateString(request.WirRow.Trim(), 50);
            positionChanged = true;
        }

        // Update Position (only if currently empty)
        if (!string.IsNullOrWhiteSpace(request.WirPosition) && string.IsNullOrWhiteSpace(wirRecord.Position))
        {
            wirRecord.Position = TruncateString(request.WirPosition.Trim(), 50);
            positionChanged = true;
        }

        // Update FactorySectionId (only if currently null and we have a target section)
        if (targetSectionId.HasValue && !wirRecord.FactorySectionId.HasValue)
        {
            wirRecord.FactorySectionId = targetSectionId;
            positionChanged = true;
        }

        return Result.Success();
    }

    /// <summary>
    /// Synchronizes Box position with the latest WIR position.
    /// Handles concurrency conflicts via DbUpdateException.
    /// </summary>
    private async Task<Result> SynchronizeBoxPositionWithWIR(
        WIRRecord wirRecord,
        Guid boxId,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxId, cancellationToken);
        if (box == null)
            return Result.Failure("Box not found");

        // Check if box position actually needs updating
        bool boxNeedsUpdate = box.Bay != wirRecord.Bay ||
                             box.Row != wirRecord.Row ||
                             box.Position != wirRecord.Position ||
                             box.FactorySectionId != wirRecord.FactorySectionId;

        if (!boxNeedsUpdate)
            return Result.Success(); // Already synchronized

        // Store old values for audit log
        string oldBay = box.Bay ?? string.Empty;
        string oldRow = box.Row ?? string.Empty;
        string oldPosition = box.Position ?? string.Empty;

        // Update box position
        box.Bay = wirRecord.Bay;
        box.Row = wirRecord.Row;
        box.Position = wirRecord.Position;
        box.FactorySectionId = wirRecord.FactorySectionId;
        box.ModifiedDate = DateTime.UtcNow;
        box.ModifiedBy = currentUserId;

        _unitOfWork.Repository<Box>().Update(box);

        try
        {
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Create audit log for box position update
            await CreateBoxPositionAuditLog(
                box,
                wirRecord,
                oldBay,
                oldRow,
                oldPosition,
                currentUserId,
                cancellationToken);

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            // Handle unique constraint violation (concurrency conflict)
            if (IsUniqueConstraintViolation(ex))
            {
                return Result.Failure(
                    $"Location conflict: Bay {wirRecord.Bay}, Row {wirRecord.Row}, Position {wirRecord.Position} " +
                    $"is already occupied by another Box in this section. This position was taken by another user " +
                    $"while you were updating. Please refresh and select a different location.");
            }

            // Re-throw if it's a different type of database error
            throw;
        }
    }

    #endregion

    #region WIR Validation

    /// <summary>
    /// Validates that previous WIR checkpoint has been approved before allowing position updates.
    /// Returns Result instead of throwing exceptions.
    /// </summary>
    private async Task<Result> ValidatePreviousWIRApprovedAsync(
        BoxActivity currentWIRActivity,
        Guid boxId,
        CancellationToken cancellationToken)
    {
        // Find the previous WIR activity for this box (by sequence)
        var previousWIRActivity = _dbContext.BoxActivities
            .Include(x => x.ActivityMaster)
            .Include(x => x.ActivityTemplateActivity)
            .Where(x => x.BoxId == boxId && x.Sequence < currentWIRActivity.Sequence)
            .AsEnumerable()
            .Where(x => GetIsWIRCheckpoint(x))
            .OrderByDescending(x => x.Sequence)
            .FirstOrDefault();

        // If no previous WIR exists, validation passes (this is the first WIR)
        if (previousWIRActivity == null)
            return Result.Success();

        string previousWIRCode = GetWIRCode(previousWIRActivity) ?? "WIR";

        // Check if previous WIR record exists
        var previousWIR = await _dbContext.WIRRecords
            .Where(w => w.BoxActivityId == previousWIRActivity.BoxActivityId)
            .FirstOrDefaultAsync(cancellationToken);

        if (previousWIR == null)
        {
            return Result.Failure(
                $"Cannot update WIR position. Previous WIR '{previousWIRCode}' has not been created yet. " +
                $"Please ensure previous checkpoints are completed in sequence.");
        }

        // Check latest checkpoint for this WIR
        var previousCheckpoint = await _dbContext.WIRCheckpoints
            .Include(cp => cp.ChecklistItems)
            .Where(cp => cp.BoxId == boxId && cp.WIRCode == previousWIR.WIRCode)
            .OrderByDescending(cp => cp.Version)
            .FirstOrDefaultAsync(cancellationToken);

        if (previousCheckpoint != null)
        {
            return ValidateCheckpointStatus(previousCheckpoint, previousWIRCode);
        }

        // If no checkpoint exists, validate WIR record status
        return ValidateWIRRecordStatus(previousWIR, previousWIRCode);
    }

    /// <summary>
    /// Validates WIR checkpoint status
    /// </summary>
    private Result ValidateCheckpointStatus(WIRCheckpoint checkpoint, string wirCode)
    {
        if (checkpoint.Status == WIRCheckpointStatusEnum.Rejected)
        {
            return Result.Failure(
                $"Cannot update WIR position. Previous WIR '{wirCode}' (Version {checkpoint.Version}) was Rejected. " +
                $"Issues must be resolved and it must be approved before proceeding.");
        }

        if (checkpoint.Status == WIRCheckpointStatusEnum.Pending)
        {
            return Result.Failure(
                $"Cannot update WIR position. Previous WIR '{wirCode}' (Version {checkpoint.Version}) is still Pending. " +
                $"It must be approved first before proceeding to the next checkpoint.");
        }

        // ConditionallyApproved is acceptable - validation passes
        if (checkpoint.Status == WIRCheckpointStatusEnum.ConditionalApproval)
            return Result.Success();

        // If status is Approved, verify all checklist items are "Pass"
        if (checkpoint.Status == WIRCheckpointStatusEnum.Approved)
        {
            if (checkpoint.ChecklistItems == null || !checkpoint.ChecklistItems.Any())
            {
                return Result.Failure(
                    $"Cannot update WIR position. Previous WIR '{wirCode}' (Version {checkpoint.Version}) is Under Review. " +
                    $"Checklist items must be added and approved before proceeding.");
            }

            bool allItemsPass = checkpoint.ChecklistItems.All(item =>
                item.Status == CheckListItemStatusEnum.Pass);

            if (!allItemsPass)
            {
                return Result.Failure(
                    $"Cannot update WIR position. Previous WIR '{wirCode}' (Version {checkpoint.Version}) is Under Review. " +
                    $"All checklist items must have 'Pass' status before proceeding to the next checkpoint.");
            }
        }

        return Result.Success();
    }

    /// <summary>
    /// Validates WIR record status when no checkpoint exists
    /// </summary>
    private Result ValidateWIRRecordStatus(WIRRecord wirRecord, string wirCode)
    {
        var wirStatus = (WIRCheckpointStatusEnum)wirRecord.Status;

        if (wirStatus == WIRCheckpointStatusEnum.Pending)
        {
            return Result.Failure(
                $"Cannot update WIR position. Previous WIR '{wirCode}' is still Pending. " +
                $"It must be approved before proceeding to the next checkpoint.");
        }

        if (wirStatus == WIRCheckpointStatusEnum.Rejected)
        {
            return Result.Failure(
                $"Cannot update WIR position. Previous WIR '{wirCode}' was Rejected. " +
                $"Issues must be resolved and it must be approved before proceeding.");
        }

        return Result.Success();
    }

    /// <summary>
    /// Validates position uniqueness within factory section.
    /// Returns Result instead of boolean.
    /// </summary>
    private async Task<Result> ValidateUniqueLocationInFactoryAsync(
        string bayValue,
        string rowValue,
        string positionValue,
        Guid currentBoxId,
        Guid? targetSectionId,
        CancellationToken cancellationToken)
    {
        // Skip validation if no position data
        if (string.IsNullOrWhiteSpace(bayValue) &&
            string.IsNullOrWhiteSpace(rowValue) &&
            string.IsNullOrWhiteSpace(positionValue))
            return Result.Success();

        var currentBox = await _unitOfWork.Repository<Box>().GetByIdAsync(currentBoxId, cancellationToken);
        if (currentBox == null || !currentBox.FactoryId.HasValue)
            return Result.Success(); // No factory assigned, skip validation

        bayValue = bayValue?.Trim() ?? string.Empty;
        rowValue = rowValue?.Trim() ?? string.Empty;
        positionValue = positionValue?.Trim() ?? string.Empty;

        // If the current box already has this exact position, allow it (no conflict)
        if (currentBox.Bay?.Trim() == bayValue &&
            currentBox.Row?.Trim() == rowValue &&
            currentBox.Position?.Trim() == positionValue &&
            currentBox.FactorySectionId == targetSectionId)
            return Result.Success();

        // Check for conflicts within the same section
        var conflictingBox = _unitOfWork.Repository<Box>().GetEntityWithSpec(
            new GetBoxWithIncludesSpecification(
                currentBoxId,
                currentBox.FactoryId.Value,
                bayValue,
                rowValue,
                positionValue,
                targetSectionId));

        if (conflictingBox != null)
        {
            string sectionInfo = targetSectionId.HasValue
                ? "in this section"
                : "in this factory";

            return Result.Failure(
                $"Location conflict: Bay {bayValue}, Row {rowValue}, Position {positionValue} " +
                $"is already occupied by Box '{conflictingBox.BoxTag}' {sectionInfo}. " +
                $"Please select a different location.");
        }

        return Result.Success();
    }

    /// <summary>
    /// Validates position falls within factory section boundaries.
    /// Returns section ID if valid.
    /// </summary>
    private async Task<Result<Guid?>> ValidatePositionWithinFactorySectionsAsync(
        string bayValue,
        string rowValue,
        Guid currentBoxId,
        CancellationToken cancellationToken)
    {
        // Skip validation if bay and row are not provided
        if (string.IsNullOrWhiteSpace(bayValue) || string.IsNullOrWhiteSpace(rowValue))
            return Result.Success<Guid?>(null);

        var currentBox = await _unitOfWork.Repository<Box>().GetByIdAsync(currentBoxId, cancellationToken);
        if (currentBox == null || !currentBox.FactoryId.HasValue)
            return Result.Success<Guid?>(null); // No factory assigned, skip validation

        // Load factory sections
        var factorySections = await _dbContext.FactorySections
            .Where(s => s.FactoryId == currentBox.FactoryId.Value && s.IsActive)
            .ToListAsync(cancellationToken);

        // If no sections defined, allow any position (backwards compatibility)
        if (factorySections == null || !factorySections.Any())
            return Result.Success<Guid?>(null);

        bayValue = bayValue.Trim().ToUpper();
        rowValue = rowValue.Trim();

        // Parse row as integer
        if (!int.TryParse(rowValue, out int rowNumber))
        {
            return Result.Failure<Guid?>(
                $"Invalid row value '{rowValue}'. Row must be a numeric value.");
        }

        // Find matching section
        foreach (var section in factorySections)
        {
            bool bayInRange = IsBayInRange(bayValue, section.MinBay, section.MaxBay);
            bool rowInRange = rowNumber >= section.MinRow && rowNumber <= section.MaxRow;

            if (bayInRange && rowInRange)
                return Result.Success<Guid?>(section.SectionId);
        }

        // Build error message with available section ranges
        var sectionRanges = factorySections
            .Select(s => $"{s.SectionName} (Rows {s.MinRow}-{s.MaxRow}, Bays {s.MinBay}-{s.MaxBay})")
            .ToList();

        string availableSections = string.Join(", ", sectionRanges);

        return Result.Failure<Guid?>(
            $"Position Bay {bayValue}, Row {rowValue} does not fall within any factory section. " +
            $"Available sections: {availableSections}. " +
            $"Please select a position within the defined section boundaries.");
    }

    #endregion

    #region Progress Update Validation

    private async Task<Result> ValidateProgressUpdateAsync(
        CreateProgressUpdateCommand request,
        BoxActivity boxActivity,
        CancellationToken cancellationToken)
    {
        if (boxActivity == null)
            return Result.Failure("Box activity not found");

        if (request.BoxId == Guid.Empty)
            return Result.Failure("Invalid BoxId");

        if (request.BoxActivityId == Guid.Empty)
            return Result.Failure("Invalid BoxActivityId");

        // Validate project status
        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(
            boxActivity.Box.ProjectId,
            "create progress updates",
            cancellationToken);

        if (!projectStatusValidation.IsSuccess)
            return Result.Failure(projectStatusValidation.Error!);

        // Validate box status
        var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(
            boxActivity.Box.BoxId,
            "create progress update",
            cancellationToken);

        if (!boxStatusValidation.IsSuccess)
            return Result.Failure(boxStatusValidation.Error!);

        // Block progress update if the box has any panel that is not Second Approval Approved
        var boxPanels = await _dbContext.BoxPanels
            .Where(p => p.BoxId == request.BoxId)
            .ToListAsync(cancellationToken);

        if (boxPanels.Any())
        {
            var panelsNotSecondApproved = boxPanels
                .Where(p => p.PanelStatus != PanelStatusEnum.SecondApprovalApproved)
                .ToList();

            if (panelsNotSecondApproved.Any())
            {
                return Result.Failure(
                    "Cannot update activity progress. All panels in this box must have Second Approval Approved " +
                    "before updating progress. One or more panels are not yet approved at Second Approval.");
            }
        }

        // Check activity status
        // Allow position-only updates for completed activities (if WIR position data is provided)
        bool isPositionOnlyUpdate = !string.IsNullOrWhiteSpace(request.WirBay) ||
                                   !string.IsNullOrWhiteSpace(request.WirRow) ||
                                   !string.IsNullOrWhiteSpace(request.WirPosition);

        if (boxActivity.Status == BoxStatusEnum.Completed || boxActivity.Status == BoxStatusEnum.Delayed)
        {
            if (!isPositionOnlyUpdate || request.ProgressPercentage != boxActivity.ProgressPercentage)
            {
                return Result.Failure(
                    "Cannot update progress for completed activities. " +
                    "Only position (Bay/Row) can be updated for completed activities.");
            }
        }

        if (boxActivity.Status == BoxStatusEnum.OnHold)
        {
            return Result.Failure(
                "Cannot create progress update. Activities in 'OnHold' status cannot be modified. " +
                "Please change the activity status first.");
        }

        // When updating progress to 100%, require at least one image
        if (request.ProgressPercentage >= 100)
        {
            var imageValidation = await ValidateImageRequirementAsync(request, cancellationToken);
            if (imageValidation.IsFailure)
                return imageValidation;
        }

        return Result.Success();
    }

    /// <summary>
    /// Validates that at least one image exists when completing an activity (100% progress)
    /// </summary>
    private async Task<Result> ValidateImageRequirementAsync(
        CreateProgressUpdateCommand request,
        CancellationToken cancellationToken)
    {
        bool hasImageInCurrentRequest = (request.Files != null && request.Files.Count > 0) ||
                                        (request.ImageUrls != null && request.ImageUrls.Count > 0);

        if (hasImageInCurrentRequest)
            return Result.Success();

        // Check for existing images in previous progress updates
        var existingUpdateIds = await _dbContext.ProgressUpdates
            .Where(pu => pu.BoxActivityId == request.BoxActivityId)
            .Select(pu => pu.ProgressUpdateId)
            .ToListAsync(cancellationToken);

        if (existingUpdateIds.Count > 0)
        {
            bool hasExistingImage = await _dbContext.ProgressUpdateImages
                .AnyAsync(img => existingUpdateIds.Contains(img.ProgressUpdateId), cancellationToken);

            if (hasExistingImage)
                return Result.Success();
        }

        // Check for legacy Photo field
        bool hasExistingPhoto = await _dbContext.ProgressUpdates
            .AnyAsync(pu => pu.BoxActivityId == request.BoxActivityId &&
                           !string.IsNullOrWhiteSpace(pu.Photo),
                     cancellationToken);

        if (hasExistingPhoto)
            return Result.Success();

        return Result.Failure(
            "You must add one or more images to a progress update before completing this activity (100%). " +
            "Add at least one progress photo and try again.");
    }

    #endregion

    #region Box and Project Progress Updates

    private async Task<decimal> UpdateBoxProgress(
        Guid boxId,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        var boxActivities = await _unitOfWork.Repository<BoxActivity>()
            .FindAsync(ba => ba.BoxId == boxId && ba.IsActive, cancellationToken);

        if (!boxActivities.Any())
            return 0;

        var averageProgress = boxActivities.Average(ba => ba.ProgressPercentage);
        var allCompleted = boxActivities.All(ba => ba.ProgressPercentage >= 100);

        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxId, cancellationToken);
        if (box == null)
            return 0;

        var oldProgress = box.ProgressPercentage;
        var oldStatus = box.Status;
        const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        var oldActualStartDateString = box.ActualStartDate?.ToString(dateFormat) ?? "N/A";
        var oldActualEndDateString = box.ActualEndDate?.ToString(dateFormat) ?? "N/A";

        bool isProgressChanged = oldProgress != averageProgress;
        bool isStatusChanged = false;

        box.ProgressPercentage = averageProgress;

        if (allCompleted && oldStatus != BoxStatusEnum.Completed)
        {
            box.Status = BoxStatusEnum.Completed;
            box.ActualEndDate = DateTime.UtcNow;
            isStatusChanged = true;
        }
        else if (averageProgress > 0 && oldStatus != BoxStatusEnum.InProgress && oldStatus != BoxStatusEnum.Completed)
        {
            box.Status = BoxStatusEnum.InProgress;
            if (box.ActualStartDate == null)
                box.ActualStartDate = DateTime.UtcNow;
            isStatusChanged = true;
        }

        if (isProgressChanged || isStatusChanged)
        {
            box.ModifiedDate = DateTime.UtcNow;
            box.ModifiedBy = currentUserId;
            _unitOfWork.Repository<Box>().Update(box);

            var newActualStartDateString = box.ActualStartDate?.ToString(dateFormat) ?? "N/A";
            var newActualEndDateString = box.ActualEndDate?.ToString(dateFormat) ?? "N/A";

            var log = new AuditLog
            {
                TableName = nameof(Box),
                RecordId = box.BoxId,
                Action = isStatusChanged ? "StatusAutoChange" : "ProgressAutoUpdate",
                OldValues = $"Progress: {oldProgress}%, Status: {oldStatus.ToString()}, Start: {oldActualStartDateString}, End: {oldActualEndDateString}",
                NewValues = $"Progress: {box.ProgressPercentage}%, Status: {box.Status.ToString()}, Start: {newActualStartDateString}, End: {newActualEndDateString}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Box progress updated automatically from {oldProgress}% to {box.ProgressPercentage}%."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
        }

        return box.ProgressPercentage;
    }

    private async Task UpdateProjectProgress(
        Guid projectId,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        var projectBoxes = await _unitOfWork.Repository<Box>()
            .FindAsync(b => b.ProjectId == projectId, cancellationToken);

        if (!projectBoxes.Any())
            return;

        var averageProgress = projectBoxes.Average(b => b.ProgressPercentage);
        var allCompleted = projectBoxes.All(b => b.Status == BoxStatusEnum.Completed || b.Status == BoxStatusEnum.Dispatched);

        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(projectId, cancellationToken);
        if (project == null)
            return;

        var oldProgress = project.ProgressPercentage;
        var oldStatus = project.Status;
        const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        var oldActualStartDateString = project.ActualStartDate?.ToString(dateFormat) ?? "N/A";
        var oldActualEndDateString = project.ActualEndDate?.ToString(dateFormat) ?? "N/A";

        bool isProgressChanged = oldProgress != averageProgress;
        bool isStatusChanged = false;

        project.ProgressPercentage = averageProgress;

        if (allCompleted && oldStatus != ProjectStatusEnum.Completed)
        {
            project.Status = ProjectStatusEnum.Completed;
            project.ActualEndDate = DateTime.UtcNow;
            isStatusChanged = true;
        }
        else if (averageProgress > 0 && oldStatus != ProjectStatusEnum.Completed &&
                 oldStatus != ProjectStatusEnum.OnHold && project.ActualStartDate == null)
        {
            project.ActualStartDate = DateTime.UtcNow;
        }

        if (isProgressChanged || isStatusChanged)
        {
            project.ModifiedDate = DateTime.UtcNow;
            project.ModifiedBy = _currentUserService.Username;
            _unitOfWork.Repository<Project>().Update(project);

            var newActualStartDateString = project.ActualStartDate?.ToString(dateFormat) ?? "N/A";
            var newActualEndDateString = project.ActualEndDate?.ToString(dateFormat) ?? "N/A";

            var log = new AuditLog
            {
                TableName = nameof(Project),
                RecordId = project.ProjectId,
                Action = isStatusChanged ? "StatusAutoChange" : "ProgressAutoUpdate",
                OldValues = $"Progress: {oldProgress}%, Status: {oldStatus.ToString()}, Start: {oldActualStartDateString}, End: {oldActualEndDateString}",
                NewValues = $"Progress: {project.ProgressPercentage}%, Status: {project.Status.ToString()}, Start: {newActualStartDateString}, End: {newActualEndDateString}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Project progress updated automatically from {oldProgress}% to {project.ProgressPercentage}%."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
        }
    }

    private async Task UpdateBoxActivityAndAddAuditLog(
        CreateProgressUpdateCommand request,
        BoxActivity boxActivity,
        BoxStatusEnum inferredStatus,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        var oldStatus = boxActivity.Status;
        var oldProgress = boxActivity.ProgressPercentage;
        var oldActualStartDate = boxActivity.ActualStartDate;
        var oldActualEndDate = boxActivity.ActualEndDate;

        boxActivity.ProgressPercentage = request.ProgressPercentage;
        boxActivity.Status = inferredStatus;
        boxActivity.WorkDescription = request.WorkDescription;
        boxActivity.IssuesEncountered = request.IssuesEncountered;
        boxActivity.ModifiedDate = DateTime.UtcNow;
        boxActivity.ModifiedBy = currentUserId;

        if ((inferredStatus == BoxStatusEnum.InProgress ||
             inferredStatus == BoxStatusEnum.Completed ||
             inferredStatus == BoxStatusEnum.Delayed) && boxActivity.ActualStartDate == null)
        {
            boxActivity.ActualStartDate = DateTime.UtcNow;
        }

        // Set ActualEndDate when activity reaches 100% progress (either Completed or Delayed)
        if ((inferredStatus == BoxStatusEnum.Completed || inferredStatus == BoxStatusEnum.Delayed) &&
            oldStatus != BoxStatusEnum.Completed && oldStatus != BoxStatusEnum.Delayed)
        {
            boxActivity.ActualEndDate = DateTime.UtcNow;
            boxActivity.ProgressPercentage = 100;
        }

        var newActualStartDateString = boxActivity.ActualStartDate?.ToString(dateFormat) ?? "N/A";
        var newActualEndDateString = boxActivity.ActualEndDate?.ToString(dateFormat) ?? "N/A";

        _unitOfWork.Repository<BoxActivity>().Update(boxActivity);

        var log = new AuditLog
        {
            TableName = nameof(BoxActivity),
            RecordId = boxActivity.BoxActivityId,
            Action = "ProgressUpdate",
            OldValues = $"Progress: {oldProgress}%, Status: {oldStatus.ToString()}, Start: {oldActualStartDate?.ToString(dateFormat) ?? "N/A"}, End: {oldActualEndDate?.ToString(dateFormat) ?? "N/A"}",
            NewValues = $"Progress: {boxActivity.ProgressPercentage}%, Status: {boxActivity.Status.ToString()}, Start: {newActualStartDateString}, End: {newActualEndDateString}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Progress updated to {boxActivity.ProgressPercentage}%. Status inferred to {boxActivity.Status.ToString()}."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    #endregion

    #region Audit Logging

    /// <summary>
    /// Creates audit log for WIR position update
    /// </summary>
    private async Task CreateWIRAuditLog(
        WIRRecord wirRecord,
        CreateProgressUpdateCommand request,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        var auditLog = new AuditLog
        {
            TableName = nameof(WIRRecord),
            RecordId = wirRecord.WIRRecordId,
            Action = "WIRPositionUpdate",
            OldValues = "Position: Not Set",
            NewValues = $"Bay: {wirRecord.Bay ?? "N/A"}, Row: {wirRecord.Row ?? "N/A"}, Position: {wirRecord.Position ?? "N/A"}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"WIR position set from progress update. WIR: {wirRecord.WIRCode}"
        };

        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
    }

    /// <summary>
    /// Creates audit log for box position synchronization
    /// </summary>
    private async Task CreateBoxPositionAuditLog(
        Box box,
        WIRRecord wirRecord,
        string oldBay,
        string oldRow,
        string oldPosition,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        var auditLog = new AuditLog
        {
            TableName = nameof(Box),
            RecordId = box.BoxId,
            Action = "BoxPositionUpdate",
            OldValues = $"Bay: {oldBay}, Row: {oldRow}, Position: {oldPosition}",
            NewValues = $"Bay: {wirRecord.Bay ?? "N/A"}, Row: {wirRecord.Row ?? "N/A"}, Position: {wirRecord.Position ?? "N/A"}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Box position synchronized with WIR. Box: {box.BoxTag}, WIR: {wirRecord.WIRCode}"
        };

        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Determines the inferred status based on progress percentage
    /// </summary>
    private BoxStatusEnum DetermineInferredStatus(decimal progressPercentage, BoxActivity boxActivity)
    {
        if (progressPercentage >= 100)
            return DetermineActivityStatusAtCompletion(boxActivity);

        if (progressPercentage > 0)
            return BoxStatusEnum.InProgress;

        // Progress is 0
        if (boxActivity.Status == BoxStatusEnum.OnHold)
            return BoxStatusEnum.OnHold;

        return BoxStatusEnum.NotStarted;
    }

    /// <summary>
    /// Determines whether activity should be marked as Completed or Delayed based on duration
    /// </summary>
    private BoxStatusEnum DetermineActivityStatusAtCompletion(BoxActivity boxActivity)
    {
        if (!boxActivity.Duration.HasValue || boxActivity.Duration.Value <= 0)
            return BoxStatusEnum.Completed;

        DateTime? actualStart = boxActivity.ActualStartDate;
        DateTime actualEnd = boxActivity.ActualEndDate ?? DateTime.UtcNow;

        if (!actualStart.HasValue)
            return BoxStatusEnum.Completed;

        var actualDurationDays = (actualEnd - actualStart.Value).TotalDays;
        var plannedDurationDays = boxActivity.Duration.Value;

        if (actualDurationDays > plannedDurationDays)
            return BoxStatusEnum.Delayed;

        return BoxStatusEnum.Completed;
    }

    /// <summary>
    /// Checks if the request contains position data
    /// </summary>
    private bool HasPositionData(CreateProgressUpdateCommand request)
    {
        return !string.IsNullOrWhiteSpace(request.WirBay) ||
               !string.IsNullOrWhiteSpace(request.WirRow) ||
               !string.IsNullOrWhiteSpace(request.WirPosition);
    }

    /// <summary>
    /// Checks if WIR record has position data
    /// </summary>
    private bool HasWIRPosition(WIRRecord wirRecord)
    {
        return !string.IsNullOrWhiteSpace(wirRecord.Bay) ||
               !string.IsNullOrWhiteSpace(wirRecord.Row) ||
               !string.IsNullOrWhiteSpace(wirRecord.Position);
    }

    /// <summary>
    /// Checks if bay value falls within the specified range
    /// </summary>
    private bool IsBayInRange(string bayValue, string minBay, string maxBay)
    {
        if (string.IsNullOrWhiteSpace(minBay) || string.IsNullOrWhiteSpace(maxBay))
            return false;

        if (string.IsNullOrWhiteSpace(bayValue))
            return false;

        char minBayChar = minBay.ToUpper()[0];
        char maxBayChar = maxBay.ToUpper()[0];
        char bayChar = bayValue.ToUpper()[0];

        return bayChar >= minBayChar && bayChar <= maxBayChar;
    }

    /// <summary>
    /// Checks if exception is a unique constraint violation
    /// </summary>
    private bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        // SQL Server
        if (ex.InnerException?.Message.Contains("IX_Boxes_UniquePosition") == true)
            return true;

        // PostgreSQL
        if (ex.InnerException?.Message.Contains("duplicate key value violates unique constraint") == true)
            return true;

        // MySQL
        if (ex.InnerException?.Message.Contains("Duplicate entry") == true)
            return true;

        return false;
    }

    /// <summary>
    /// Truncates string to specified maximum length
    /// </summary>
    private string TruncateString(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /// <summary>
    /// Gets activity name from either ActivityMaster or ActivityTemplateActivity
    /// </summary>
    private string GetActivityName(BoxActivity boxActivity)
    {
        if (boxActivity.ActivityMaster != null)
            return boxActivity.ActivityMaster.ActivityName;

        if (boxActivity.ActivityTemplateActivity != null)
            return boxActivity.ActivityTemplateActivity.ActivityName;

        return "Unknown Activity";
    }

    /// <summary>
    /// Gets WIR checkpoint flag from either ActivityMaster or ActivityTemplateActivity
    /// </summary>
    private bool GetIsWIRCheckpoint(BoxActivity boxActivity)
    {
        if (boxActivity.ActivityMaster != null)
            return boxActivity.ActivityMaster.IsWIRCheckpoint;

        if (boxActivity.ActivityTemplateActivity != null)
            return boxActivity.ActivityTemplateActivity.IsWIRCheckpoint;

        return false;
    }

    /// <summary>
    /// Gets WIR code from either ActivityMaster or ActivityTemplateActivity
    /// </summary>
    private string? GetWIRCode(BoxActivity boxActivity)
    {
        if (boxActivity.ActivityMaster != null)
            return boxActivity.ActivityMaster.WIRCode;

        if (boxActivity.ActivityTemplateActivity != null)
            return boxActivity.ActivityTemplateActivity.WIRCode;

        return null;
    }

    #endregion
}