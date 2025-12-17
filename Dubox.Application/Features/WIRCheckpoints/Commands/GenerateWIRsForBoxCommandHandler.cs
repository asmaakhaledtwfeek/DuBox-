using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.WIRCheckpoints.Commands;

public class GenerateWIRsForBoxCommandHandler : IRequestHandler<GenerateWIRsForBoxCommand, Result<List<WIRCheckpointDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GenerateWIRsForBoxCommandHandler> _logger;

    public GenerateWIRsForBoxCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<GenerateWIRsForBoxCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<WIRCheckpointDto>>> Handle(GenerateWIRsForBoxCommand request, CancellationToken cancellationToken)
    {
        return Result.Success(new List<WIRCheckpointDto>());
        //try
        //{
        //    // 1. Verify box exists
        //    var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);
        //    if (box == null)
        //    {
        //        return Result.Failure<List<WIRCheckpointDto>>($"Box with ID {request.BoxId} not found");
        //    }

        //    // 2. Check if WIRs already exist for this box
        //    var existingWIRs = await _unitOfWork.Repository<WIRCheckpoint>()
        //        .FindAsync(w => w.BoxId == request.BoxId, cancellationToken);

        //    if (existingWIRs.Any())
        //    {
        //        _logger.LogWarning("WIRs already exist for Box {BoxId}. Skipping generation.", request.BoxId);
        //        var existingDtos = existingWIRs.Adapt<List<WIRCheckpointDto>>();
        //        return Result.Success(existingDtos, "WIRs already exist for this box");
        //    }

        //    // 3. Get all WIR Masters (metadata about each WIR)
        //    var wirMasters = (await _unitOfWork.Repository<WIRMaster>()
        //        .FindAsync(w => w.IsActive, cancellationToken))
        //        .OrderBy(w => w.Sequence)
        //        .ToList();

        //    if (!wirMasters.Any())
        //    {
        //        return Result.Failure<List<WIRCheckpointDto>>("No WIR Masters found in the system. Please seed WIR Master data.");
        //    }

        //    var createdWIRs = new List<WIRCheckpoint>();

        //    // 4. For each WIR Master, create a WIR Checkpoint
        //    foreach (var wirMaster in wirMasters)
        //    {
        //        // Create WIR Checkpoint
        //        var wirCheckpoint = new WIRCheckpoint
        //        {
        //            BoxId = request.BoxId,
        //            WIRNumber = wirMaster.WIRNumber,
        //            WIRName = wirMaster.WIRName,
        //            WIRDescription = wirMaster.Description,
        //            Status = WIRCheckpointStatusEnum.Pending,
        //            RequestedDate = DateTime.UtcNow,
        //            CreatedDate = DateTime.UtcNow
        //        };

        //        // 5. Get predefined checklist items for this WIR
        //        var predefinedItems = (await _unitOfWork.Repository<PredefinedChecklistItem>()
        //            .FindAsync(p => p.WIRNumber == wirMaster.WIRNumber && p.IsActive, cancellationToken))
        //            .OrderBy(p => p.Sequence)
        //            .ToList();

        //        // 6. Get categories and references separately for the predefined items
        //        var categoryIds = predefinedItems.Select(p => p.CategoryId).Where(c => c.HasValue).Distinct().ToList();
        //        var referenceIds = predefinedItems.Select(p => p.ReferenceId).Where(r => r.HasValue).Distinct().ToList();




        //        // 7. Create WIR Checklist Items from predefined items
        //        foreach (var predefinedItem in predefinedItems)
        //        {
        //            string? refName = null;
        //            if (predefinedItem.ReferenceId.HasValue &&
        //                referenceDict.TryGetValue(predefinedItem.ReferenceId.Value, out var reference))
        //            {
        //                refName = reference.ReferenceName;
        //            }

        //            var checklistItem = new WIRChecklistItem
        //            {
        //                WIRId = wirCheckpoint.WIRId,
        //                CheckpointDescription = predefinedItem.CheckpointDescription,
        //                ReferenceDocument = refName,
        //                Status = CheckListItemStatusEnum.Pending,
        //                Sequence = predefinedItem.Sequence,
        //                PredefinedItemId = predefinedItem.PredefinedItemId
        //            };

        //            wirCheckpoint.ChecklistItems.Add(checklistItem);
        //        }

        //        await _unitOfWork.Repository<WIRCheckpoint>().AddAsync(wirCheckpoint);
        //        createdWIRs.Add(wirCheckpoint);
        //    }

        //    // 8. Save all WIRs to database
        //    await _unitOfWork.CompleteAsync(cancellationToken);

        //    _logger.LogInformation("Successfully generated {Count} WIRs for Box {BoxId}", createdWIRs.Count, request.BoxId);

        //    // 9. Map to DTOs and return
        //    var wirDtos = createdWIRs.Adapt<List<WIRCheckpointDto>>();
        //    return Result.Success(wirDtos, $"Successfully generated {createdWIRs.Count} WIRs with checklist items");
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Error generating WIRs for Box {BoxId}", request.BoxId);
        //    return Result.Failure<List<WIRCheckpointDto>>($"Error generating WIRs: {ex.Message}");
        //}
    }
}
