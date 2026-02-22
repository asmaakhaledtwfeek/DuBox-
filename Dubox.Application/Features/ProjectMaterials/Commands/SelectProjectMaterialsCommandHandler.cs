using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ProjectMaterials.Commands;

public class SelectProjectMaterialsCommandHandler 
    : IRequestHandler<SelectProjectMaterialsCommand, Result<List<MaterialDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _context;

    public SelectProjectMaterialsCommandHandler(IUnitOfWork unitOfWork, IDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result<List<MaterialDto>>> Handle(
        SelectProjectMaterialsCommand request, 
        CancellationToken cancellationToken)
    {
        // Verify project exists
        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            return Result.Failure<List<MaterialDto>>("Project not found.");

        // Get all materials
        var allMaterials = await _unitOfWork.Repository<Material>()
            .FindAsync(m => m.IsActive, cancellationToken);

        // Get existing project materials
        var existingProjectMaterials = await _unitOfWork.Repository<ProjectMaterial>()
            .FindAsync(pm => pm.ProjectId == request.ProjectId, cancellationToken);

        // Determine which materials should be selected
        var selectedMaterialIds = request.SelectAll 
            ? allMaterials.Select(m => m.MaterialId).ToList()
            : request.MaterialIds;

        // Track which materials are newly selected or deselected
        var previouslySelectedMaterialIds = existingProjectMaterials
            .Where(pm => pm.IsSelected)
            .Select(pm => pm.MaterialId)
            .ToList();

        var newlySelectedMaterialIds = selectedMaterialIds
            .Except(previouslySelectedMaterialIds)
            .ToList();

        var deselectedMaterialIds = previouslySelectedMaterialIds
            .Except(selectedMaterialIds)
            .ToList();

        // Update existing ProjectMaterials
        foreach (var pm in existingProjectMaterials)
        {
            pm.IsSelected = selectedMaterialIds.Contains(pm.MaterialId);
            _unitOfWork.Repository<ProjectMaterial>().Update(pm);
        }

        // Create new ProjectMaterial records for materials that don't exist yet
        var existingMaterialIds = existingProjectMaterials.Select(pm => pm.MaterialId).ToList();
        var newMaterialIds = selectedMaterialIds.Except(existingMaterialIds).ToList();

        foreach (var materialId in newMaterialIds)
        {
            var newProjectMaterial = new ProjectMaterial
            {
                ProjectId = request.ProjectId,
                MaterialId = materialId,
                IsSelected = true,
                RequiredQuantity = 0,
                AllocatedQuantity = 0,
                ConsumedQuantity = 0
            };
            await _unitOfWork.Repository<ProjectMaterial>().AddAsync(newProjectMaterial, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Sync newly selected materials to all box types in this project
        if (newlySelectedMaterialIds.Any() || newMaterialIds.Any())
        {
            await SyncMaterialsToBoxTypes(
                request.ProjectId, 
                newlySelectedMaterialIds.Concat(newMaterialIds).ToList(), 
                allMaterials.ToDictionary(m => m.MaterialId),
                cancellationToken);
        }

        // Remove deselected materials from all box types
        if (deselectedMaterialIds.Any())
        {
            await RemoveMaterialsFromBoxTypes(
                request.ProjectId, 
                deselectedMaterialIds, 
                cancellationToken);
        }

        // Return selected materials
        var selectedMaterials = allMaterials
            .Where(m => selectedMaterialIds.Contains(m.MaterialId))
            .Select(m => m.Adapt<MaterialDto>() with
            {
                IsLowStock = m.IsLowStock,
                NeedsReorder = m.NeedsReorder
            })
            .ToList();

        return Result.Success(selectedMaterials);
    }

    private async Task SyncMaterialsToBoxTypes(
        Guid projectId, 
        List<Guid> materialIds, 
        Dictionary<Guid, Material> materialsDict,
        CancellationToken cancellationToken)
    {
        // Get all active box types for this project
        var projectBoxTypes = await _context.ProjectBoxTypes
            .Where(pbt => pbt.ProjectId == projectId && pbt.IsActive)
            .ToListAsync(cancellationToken);

        if (!projectBoxTypes.Any())
            return;

        // Get ALL existing box type materials to avoid duplicates
        var existingBoxTypeMaterials = await _context.BoxTypeMaterials
            .Where(btm => projectBoxTypes.Select(pbt => pbt.Id).Contains(btm.ProjectBoxTypeId))
            .Select(btm => new { btm.ProjectBoxTypeId, btm.MaterialId })
            .ToListAsync(cancellationToken);

        // Create a HashSet for fast lookup
        var existingLookup = existingBoxTypeMaterials
            .Select(btm => $"{btm.ProjectBoxTypeId}_{btm.MaterialId}")
            .ToHashSet();

        // Create BoxTypeMaterial records for each box type, checking for duplicates
        var boxTypeMaterialsToAdd = new List<BoxTypeMaterial>();

        foreach (var boxType in projectBoxTypes)
        {
            foreach (var materialId in materialIds)
            {
                // Create consistent lookup key
                var lookupKey = $"{boxType.Id}_{materialId}";
                
                // Only add if not already exists in database or in current batch
                if (!existingLookup.Contains(lookupKey) && materialsDict.TryGetValue(materialId, out var material))
                {
                    // Add to lookup to prevent duplicates within this batch
                    existingLookup.Add(lookupKey);
                    
                    boxTypeMaterialsToAdd.Add(new BoxTypeMaterial
                    {
                        ProjectBoxTypeId = boxType.Id,
                        MaterialId = materialId,
                        RequiredBeforeDays = material.DefaultRequiredBeforeDays,
                        IsArrived = false,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
        }

        if (boxTypeMaterialsToAdd.Any())
        {
            await _unitOfWork.Repository<BoxTypeMaterial>().AddRangeAsync(boxTypeMaterialsToAdd, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        // Cascade to all individual boxes under each box type
        await SyncMaterialsToBoxes(projectId, projectBoxTypes, materialIds, materialsDict, cancellationToken);
    }

    private async Task SyncMaterialsToBoxes(
        Guid projectId,
        List<ProjectBoxType> projectBoxTypes,
        List<Guid> materialIds,
        Dictionary<Guid, Material> materialsDict,
        CancellationToken cancellationToken)
    {
        // Get all active boxes for these box types
        var projectBoxTypeIds = projectBoxTypes.Select(pbt => pbt.Id).ToList();
        var boxes = await _context.Boxes
            .Where(b => b.ProjectBoxTypeId.HasValue 
                && projectBoxTypeIds.Contains(b.ProjectBoxTypeId.Value)
                && b.ProjectId == projectId
                && b.IsActive)
            .ToListAsync(cancellationToken);

        if (!boxes.Any())
            return;

        // Get ALL existing box materials for these boxes to ensure no duplicates
        var boxIds = boxes.Select(b => b.BoxId).ToList();
        var existingBoxMaterials = await _context.BoxMaterials
            .Where(bm => boxIds.Contains(bm.BoxId))
            .Select(bm => new { bm.BoxId, bm.MaterialId })
            .ToListAsync(cancellationToken);

        // Create a HashSet for fast lookup using both BoxId and MaterialId
        var existingBoxMaterialLookup = existingBoxMaterials
            .Select(bm => $"{bm.BoxId}_{bm.MaterialId}")
            .ToHashSet();

        // Create BoxMaterial records for each box, checking for duplicates
        var boxMaterialsToAdd = new List<BoxMaterial>();

        foreach (var box in boxes)
        {
            if (!box.ProjectBoxTypeId.HasValue)
                continue;

            // Get the box type to determine the required before days
            var boxType = projectBoxTypes.FirstOrDefault(pbt => pbt.Id == box.ProjectBoxTypeId.Value);
            if (boxType == null)
                continue;

            foreach (var materialId in materialIds)
            {
                // Create consistent lookup key
                var lookupKey = $"{box.BoxId}_{materialId}";
                
                // Only add if not already exists in database or in current batch
                if (!existingBoxMaterialLookup.Contains(lookupKey) && materialsDict.TryGetValue(materialId, out var material))
                {
                    // Add to lookup to prevent duplicates within this batch
                    existingBoxMaterialLookup.Add(lookupKey);
                    
                    boxMaterialsToAdd.Add(new BoxMaterial
                    {
                        BoxId = box.BoxId,
                        MaterialId = materialId,
                        RequiredBeforeDays = material.DefaultRequiredBeforeDays,
                        IsArrived = false,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
        }

        if (boxMaterialsToAdd.Any())
        {
            await _unitOfWork.Repository<BoxMaterial>().AddRangeAsync(boxMaterialsToAdd, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }

    private async Task RemoveMaterialsFromBoxTypes(
        Guid projectId, 
        List<Guid> materialIds, 
        CancellationToken cancellationToken)
    {
        // Get all box types for this project
        var projectBoxTypeIds = await _context.ProjectBoxTypes
            .Where(pbt => pbt.ProjectId == projectId)
            .Select(pbt => pbt.Id)
            .ToListAsync(cancellationToken);

        if (!projectBoxTypeIds.Any())
            return;

        // Get all box type materials that match the deselected materials
        var boxTypeMaterialsToRemove = await _context.BoxTypeMaterials
            .Where(btm => 
                projectBoxTypeIds.Contains(btm.ProjectBoxTypeId) && 
                materialIds.Contains(btm.MaterialId))
            .ToListAsync(cancellationToken);

        if (boxTypeMaterialsToRemove.Any())
        {
            _unitOfWork.Repository<BoxTypeMaterial>().DeleteRange(boxTypeMaterialsToRemove);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        // Also remove from all individual boxes
        await RemoveMaterialsFromBoxes(projectId, projectBoxTypeIds, materialIds, cancellationToken);
    }

    private async Task RemoveMaterialsFromBoxes(
        Guid projectId,
        List<int> projectBoxTypeIds,
        List<Guid> materialIds,
        CancellationToken cancellationToken)
    {
        // Get all boxes for these box types in the project
        var boxIds = await _context.Boxes
            .Where(b => projectBoxTypeIds.Contains(b.ProjectBoxTypeId.Value)
                && b.ProjectId == projectId
                && b.IsActive)
            .Select(b => b.BoxId)
            .ToListAsync(cancellationToken);

        if (!boxIds.Any())
            return;

        // Get all box materials that match the deselected materials
        var boxMaterialsToRemove = await _context.BoxMaterials
            .Where(bm => 
                boxIds.Contains(bm.BoxId) && 
                materialIds.Contains(bm.MaterialId))
            .ToListAsync(cancellationToken);

        if (boxMaterialsToRemove.Any())
        {
            _unitOfWork.Repository<BoxMaterial>().DeleteRange(boxMaterialsToRemove);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}






