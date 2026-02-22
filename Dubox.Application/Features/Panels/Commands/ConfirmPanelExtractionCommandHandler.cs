using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.Panels.Commands;

/// <summary>
/// Handler for confirming and saving reviewed panel extraction data
/// </summary>
public class ConfirmPanelExtractionCommandHandler : IRequestHandler<ConfirmPanelExtractionCommand, Result<PanelExtractionConfirmationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<ConfirmPanelExtractionCommandHandler> _logger;

    public ConfirmPanelExtractionCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService,
        ILogger<ConfirmPanelExtractionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<Result<PanelExtractionConfirmationDto>> Handle(
        ConfirmPanelExtractionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Confirming panel extraction for project: {request.ProjectId}");

            // Parse current user ID
            Guid? currentUserId = null;
            if (!string.IsNullOrEmpty(_currentUserService.UserId) && Guid.TryParse(_currentUserService.UserId, out var parsedUserId))
            {
                currentUserId = parsedUserId;
            }
            var errors = new List<string>();
            var result = new PanelExtractionConfirmationDto();

            // Validate project
            var project = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

            if (project == null)
            {
                return Result.Failure<PanelExtractionConfirmationDto>("Project not found");
            }

            // Validate box if provided
            Box? box = null;
            if (request.BoxId.HasValue)
            {
                box = await _dbContext.Boxes
                    .FirstOrDefaultAsync(b => b.BoxId == request.BoxId.Value && b.ProjectId == request.ProjectId, cancellationToken);

                if (box == null)
                {
                    return Result.Failure<PanelExtractionConfirmationDto>("Box not found or does not belong to this project");
                }
            }

            // Step 1: Create/Update PanelTypes
            var panelTypeMap = new Dictionary<string, Guid>(); // Map PanelTypeCode to PanelTypeId

            foreach (var panelTypeDto in request.PanelTypes)
            {
                var panelCode = panelTypeDto.PanelTypeCode.ToUpper().Trim();

                if (panelTypeDto.IsNew)
                {
                    // Create new panel type
                    var newPanelType = new PanelType
                    {
                        ProjectId = request.ProjectId,
                        PanelTypeCode = panelCode,
                        PanelTypeName = panelTypeDto.PanelTypeName,
                        VolumeM3 = panelTypeDto.VolumeM3,
                        WeightTon = panelTypeDto.WeightTon,
                        ConcreteGrade = panelTypeDto.ConcreteGrade,
                        CoverMm = panelTypeDto.CoverMm,
                        EmbedsJson = panelTypeDto.EmbedsJson,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = currentUserId
                    };

                    await _dbContext.PanelTypes.AddAsync(newPanelType, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    panelTypeMap[panelCode] = newPanelType.PanelTypeId;
                    result.CreatedPanelTypeIds.Add(newPanelType.PanelTypeId);
                    result.PanelTypesCreated++;

                    _logger.LogInformation($"Created new panel type: {panelCode}");
                }
                else if (panelTypeDto.PanelTypeId.HasValue)
                {
                    // Update existing panel type
                    var existingPanelType = await _dbContext.PanelTypes
                        .FirstOrDefaultAsync(pt => pt.PanelTypeId == panelTypeDto.PanelTypeId.Value, cancellationToken);

                    if (existingPanelType != null)
                    {
                        // Update specifications if changed
                        existingPanelType.PanelTypeName = panelTypeDto.PanelTypeName;
                        existingPanelType.VolumeM3 = panelTypeDto.VolumeM3;
                        existingPanelType.WeightTon = panelTypeDto.WeightTon;
                        existingPanelType.ConcreteGrade = panelTypeDto.ConcreteGrade;
                        existingPanelType.CoverMm = panelTypeDto.CoverMm;
                        existingPanelType.EmbedsJson = panelTypeDto.EmbedsJson;
                        existingPanelType.ModifiedDate = DateTime.UtcNow;
                        existingPanelType.ModifiedBy = currentUserId;

                        _dbContext.PanelTypes.Update(existingPanelType);
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        panelTypeMap[panelCode] = existingPanelType.PanelTypeId;
                        result.PanelTypesUpdated++;

                        _logger.LogInformation($"Updated existing panel type: {panelCode}");
                    }
                }
            }

            // Step 2: Process Box Tag Mappings
            if (request.BoxTagMappings != null && request.BoxTagMappings.Any())
            {
                var skippedTags = new List<string>();
                
                foreach (var mapping in request.BoxTagMappings)
                {
                    // Skip unmapped tags (user chose not to map them)
                    if (!mapping.BoxId.HasValue)
                    {
                        skippedTags.Add(mapping.BoxTag);
                        _logger.LogInformation($"Skipping unmapped box tag: {mapping.BoxTag}");
                        continue;
                    }

                    Box? targetBox = null;

                    // Get existing box based on mapping
                    targetBox = await _dbContext.Boxes
                        .FirstOrDefaultAsync(b => b.BoxId == mapping.BoxId.Value && b.ProjectId == request.ProjectId, cancellationToken);

                    if (targetBox == null)
                    {
                        errors.Add($"Box tag '{mapping.BoxTag}': Box with ID {mapping.BoxId.Value} not found");
                        _logger.LogWarning($"Box tag {mapping.BoxTag} mapped to non-existent box ID {mapping.BoxId.Value}");
                        continue;
                    }

                    // Create box panels for all panel types extracted
                    foreach (var boxPanelDto in request.BoxPanels)
                    {
                        var panelCode = boxPanelDto.PanelTypeCode.ToUpper().Trim();

                        if (!panelTypeMap.TryGetValue(panelCode, out var panelTypeId))
                        {
                            errors.Add($"Panel type {panelCode} not found in confirmed panel types");
                            continue;
                        }

                        // Create panels based on quantity
                        for (int i = 1; i <= boxPanelDto.QuantityInThisBox; i++)
                        {
                            var panelName = boxPanelDto.QuantityInThisBox > 1 
                                ? $"{boxPanelDto.PanelName}-{i}" 
                                : boxPanelDto.PanelName;

                            var qrCode = $"PANEL-{project.ProjectCode}-{targetBox.SerialNumber}-{panelName}";

                            // Check if panel already exists
                            var existingPanel = await _dbContext.BoxPanels
                                .FirstOrDefaultAsync(
                                    bp => bp.BoxId == targetBox.BoxId && 
                                          bp.PanelName == panelName &&
                                          bp.PanelTypeId == panelTypeId,
                                    cancellationToken);

                            if (existingPanel == null)
                            {
                                var newBoxPanel = new BoxPanel
                                {
                                    BoxId = targetBox.BoxId,
                                    ProjectId = request.ProjectId,
                                    PanelTypeId = panelTypeId,
                                    PanelName = panelName,
                                    PanelStatus = PanelStatusEnum.NotStarted,
                                    CurrentStage = PanelStageEnum.NotStarted,
                                    QRCode = qrCode,
                                    CreatedDate = DateTime.UtcNow,
                                    CreatedBy = currentUserId
                                };

                                await _dbContext.BoxPanels.AddAsync(newBoxPanel, cancellationToken);
                                await _dbContext.SaveChangesAsync(cancellationToken);

                                result.CreatedBoxPanelIds.Add(newBoxPanel.BoxPanelId);
                                result.BoxPanelsCreated++;

                                _logger.LogInformation($"Created box panel: {panelName} for box {targetBox.BoxTag}");
                            }
                            else
                            {
                                _logger.LogInformation($"Box panel {panelName} already exists in box {targetBox.BoxTag}, skipping");
                            }
                        }
                    }
                }
                
                // Log summary of skipped tags
                if (skippedTags.Any())
                {
                    _logger.LogInformation($"Skipped {skippedTags.Count} unmapped box tags: {string.Join(", ", skippedTags)}");
                }
            }
            // Step 3: Create BoxPanels for legacy single box mode (if box provided and no mappings)
            else if (box != null && request.BoxPanels.Any())
            {
                foreach (var boxPanelDto in request.BoxPanels)
                {
                    var panelCode = boxPanelDto.PanelTypeCode.ToUpper().Trim();

                    if (!panelTypeMap.TryGetValue(panelCode, out var panelTypeId))
                    {
                        errors.Add($"Panel type {panelCode} not found in confirmed panel types");
                        continue;
                    }

                    // Create panels based on quantity
                    for (int i = 1; i <= boxPanelDto.QuantityInThisBox; i++)
                    {
                        var panelName = boxPanelDto.QuantityInThisBox > 1 
                            ? $"{boxPanelDto.PanelName}-{i}" 
                            : boxPanelDto.PanelName;

                        var qrCode = $"PANEL-{project.ProjectCode}-{box.SerialNumber}-{panelName}";

                        // Check if panel already exists
                        var existingPanel = await _dbContext.BoxPanels
                            .FirstOrDefaultAsync(
                                bp => bp.BoxId == box.BoxId && 
                                      bp.PanelName == panelName &&
                                      bp.PanelTypeId == panelTypeId,
                                cancellationToken);

                        if (existingPanel == null)
                        {
                            var newBoxPanel = new BoxPanel
                            {
                                BoxId = box.BoxId,
                                ProjectId = request.ProjectId,
                                PanelTypeId = panelTypeId,
                                PanelName = panelName,
                                PanelStatus = PanelStatusEnum.NotStarted,
                                CurrentStage = PanelStageEnum.NotStarted,
                                QRCode = qrCode,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = currentUserId
                            };

                            await _dbContext.BoxPanels.AddAsync(newBoxPanel, cancellationToken);
                            await _dbContext.SaveChangesAsync(cancellationToken);

                            result.CreatedBoxPanelIds.Add(newBoxPanel.BoxPanelId);
                            result.BoxPanelsCreated++;

                            _logger.LogInformation($"Created box panel: {panelName} for box {box.BoxTag}");
                        }
                        else
                        {
                            _logger.LogInformation($"Box panel {panelName} already exists, skipping");
                        }
                    }
                }
            }

            result.Errors = errors;

            _logger.LogInformation($"Confirmation complete: {result.PanelTypesCreated} panel types created, {result.PanelTypesUpdated} updated, {result.BoxPanelsCreated} box panels created");

            // If there are errors, return failure with the errors
            if (errors.Any())
            {
                _logger.LogWarning($"Confirmation completed with {errors.Count} error(s)");
                return Result.Failure<PanelExtractionConfirmationDto>($"Completed with errors: {string.Join("; ", errors)}");
            }

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming panel extraction");
            return Result.Failure<PanelExtractionConfirmationDto>($"Error saving panel data: {ex.Message}");
        }
    }
}

