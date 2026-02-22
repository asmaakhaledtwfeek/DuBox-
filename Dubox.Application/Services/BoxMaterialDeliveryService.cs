using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Services;

public interface IBoxMaterialDeliveryService
{
    /// <summary>
    /// Updates delivered quantities for individual boxes based on changes to box type material delivered quantity or quantity per box.
    /// Distributes the delivery quantity across boxes ordered by planned start date.
    /// </summary>
    /// <param name="boxTypeMaterial">The box type material being updated</param>
    /// <param name="newTotalDeliveredQuantity">The new total delivered quantity for the box type material</param>
    /// <param name="currentUserId">The current user ID for audit tracking</param>
    /// <param name="recalculateAll">If true, recalculates all box deliveries from scratch (used when quantity per box changes)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateBoxMaterialDeliveriesAsync(
        BoxTypeMaterial boxTypeMaterial,
        decimal newTotalDeliveredQuantity,
        Guid currentUserId,
        bool recalculateAll,
        CancellationToken cancellationToken);
}

public class BoxMaterialDeliveryService : IBoxMaterialDeliveryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _context;

    public BoxMaterialDeliveryService(
        IUnitOfWork unitOfWork,
        IDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task UpdateBoxMaterialDeliveriesAsync(
        BoxTypeMaterial boxTypeMaterial,
        decimal newTotalDeliveredQuantity,
        Guid currentUserId,
        bool recalculateAll,
        CancellationToken cancellationToken)
    {
        // Determine the quantity to distribute
        decimal quantityToDistribute;
        
        if (recalculateAll)
        {
            // When quantity per box changes, redistribute all delivered quantity from scratch
            // We must always process boxes to update them with the new quantity per box
            quantityToDistribute = newTotalDeliveredQuantity;
        }
        else
        {
            // When only delivered quantity changes, distribute only the new amount
            var oldTotalDeliveredQuantity = boxTypeMaterial.DeliveredQuantity ?? 0;
            quantityToDistribute = newTotalDeliveredQuantity - oldTotalDeliveredQuantity;
            
            // Only process if there's an actual increase in delivered quantity
            if (quantityToDistribute <= 0)
                return;
        }

        var projectBoxType = await _context.ProjectBoxTypes
            .FirstOrDefaultAsync(pbt => pbt.Id == boxTypeMaterial.ProjectBoxTypeId, cancellationToken);

        if (projectBoxType == null)
            return;

        // Get all boxes for this box type ordered by planned start date
        var boxesWithMaterials = await _context.Boxes
            .Where(b => b.ProjectBoxTypeId == boxTypeMaterial.ProjectBoxTypeId
                && b.ProjectId == projectBoxType.ProjectId
                && b.IsActive)
            .OrderBy(b => b.PlannedStartDate)
            .Select(b => new { b.BoxId, b.PlannedStartDate })
            .ToListAsync(cancellationToken);

        if (!boxesWithMaterials.Any())
            return;

        var now = DateTime.UtcNow;
        var boxIds = boxesWithMaterials.Select(b => b.BoxId).ToList();

        // Get existing box materials for this material across all boxes
        var existingBoxMaterials = await _context.BoxMaterials
            .Where(bm => boxIds.Contains(bm.BoxId) && bm.MaterialId == boxTypeMaterial.MaterialId)
            .ToListAsync(cancellationToken);

        var boxMaterialDict = existingBoxMaterials.ToDictionary(bm => bm.BoxId);
        
        // If recalculating, reset all box material deliveries to 0 first
        if (recalculateAll)
        {
            foreach (var existingBoxMaterial in existingBoxMaterials)
            {
                existingBoxMaterial.DeliveredQuantity = 0;
                existingBoxMaterial.IsArrived = false;
                existingBoxMaterial.ArrivedDate = null;
                existingBoxMaterial.ArrivedBy = null;
                existingBoxMaterial.ModifiedDate = now;
                _unitOfWork.Repository<BoxMaterial>().Update(existingBoxMaterial);
            }
        }
        
        decimal remainingDeliveryQuantity = quantityToDistribute;
        int quantityPerBox = boxTypeMaterial.QuantityPerBox;

        // Distribute the delivery quantity across boxes
        // When recalculating (quantity per box changed), we must process ALL boxes to update them
        foreach (var box in boxesWithMaterials)
        {
            // Skip only if no remaining quantity AND not recalculating
            // If recalculating, we need to update all boxes even with 0 quantity
            if (remainingDeliveryQuantity <= 0 && !recalculateAll)
                break;

            BoxMaterial boxMaterial;
            int currentDeliveredQty = 0;

            // Get or create box material record
            if (boxMaterialDict.TryGetValue(box.BoxId, out var existingBoxMaterial))
            {
                boxMaterial = existingBoxMaterial;
                currentDeliveredQty = recalculateAll ? 0 : (boxMaterial.DeliveredQuantity ?? 0);
            }
            else
            {
                boxMaterial = new BoxMaterial
                {
                    BoxMaterialId = Guid.NewGuid(),
                    BoxId = box.BoxId,
                    MaterialId = boxTypeMaterial.MaterialId,
                    RequiredBeforeDays = boxTypeMaterial.RequiredBeforeDays,
                    RequiredByDate = box.PlannedStartDate?.AddDays(-boxTypeMaterial.RequiredBeforeDays) ?? DateTime.UtcNow,
                    IsArrived = false,
                    CreatedDate = now,
                    DeliveredQuantity = 0
                };
                _context.BoxMaterials.Add(boxMaterial);
            }

            // Calculate how much this box still needs
            int remainingRequired = quantityPerBox - currentDeliveredQty;
            
            // If recalculating, we need to update all boxes even if they don't need more
            // Otherwise, skip boxes that are already fully delivered
            if (remainingRequired <= 0 && !recalculateAll)
                continue;

            // Deliver up to the remaining required amount
            int quantityToDeliver = remainingRequired > 0 
                ? (int)Math.Min(remainingDeliveryQuantity, remainingRequired)
                : 0;

            boxMaterial.DeliveredQuantity = (boxMaterial.DeliveredQuantity ?? 0) + quantityToDeliver;
            boxMaterial.ModifiedDate = now;

            // Mark as arrived if full quantity delivered
            if (boxMaterial.DeliveredQuantity >= quantityPerBox)
            {
                boxMaterial.IsArrived = true;
                boxMaterial.ArrivedDate = now;
                boxMaterial.ArrivedBy = currentUserId;
            }
            else
            {
                // Not fully delivered
                boxMaterial.IsArrived = false;
            }

            // Update if existing, otherwise it's already added
            if (existingBoxMaterial != null)
            {
                _unitOfWork.Repository<BoxMaterial>().Update(boxMaterial);
            }

            remainingDeliveryQuantity -= quantityToDeliver;
        }
    }
}
