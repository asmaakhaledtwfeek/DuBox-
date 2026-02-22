using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Services;

/// <summary>
/// Service to handle automatic assignment of project templates to box types
/// </summary>
public interface IBoxTypeTemplateAutoAssignmentService
{
    /// <summary>
    /// Automatically assigns project templates (activity + material) to box types that don't have
    /// a template yet.
    /// </summary>
    Task AssignProjectTemplatesToNewBoxTypesAsync(Guid projectId, List<int> newBoxTypeIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns a specific material template to a box type and applies it to all existing boxes of
    /// that type.
    /// </summary>
    Task AssignTemplateToBoxTypeAsync(int projectBoxTypeId, Guid templateId, string assignedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Propagates an activity template change to all eligible boxes (NotStarted / ReadyToStart)
    /// under the given box type. Activities on other boxes are left untouched.
    /// </summary>
    Task PropagateActivityTemplateToBoxTypeBoxesAsync(int projectBoxTypeId, Guid activityTemplateId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the material template of a box type with a new one, validating that no
    /// delivered-progress exists first, then re-applies the new template to all boxes of that type.
    /// Returns a failure result when validation fails.
    /// </summary>
    Task<Result<bool>> ChangeMaterialTemplateForBoxTypeAsync(
        int projectBoxTypeId,
        Guid newMaterialTemplateId,
        Guid? oldMaterialTemplateId,
        string assignedBy,
        CancellationToken cancellationToken = default);
}

public class BoxTypeTemplateAutoAssignmentService : IBoxTypeTemplateAutoAssignmentService
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBoxActivityService _boxActivityService;

    public BoxTypeTemplateAutoAssignmentService(
        IDbContext context,
        IUnitOfWork unitOfWork,
        IBoxActivityService boxActivityService)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _boxActivityService = boxActivityService;
    }

    /// <summary>
    /// Automatically assigns project templates (activity + material) to newly created box types.
    /// Ensures new box types inherit the project's template configuration by default.
    /// </summary>
    public async Task AssignProjectTemplatesToNewBoxTypesAsync(Guid projectId, List<int> newBoxTypeIds, CancellationToken cancellationToken = default)
    {
        if (!newBoxTypeIds.Any())
            return;

        // Load project for activity template
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.ProjectId == projectId, cancellationToken);

        // Load project material templates
        var projectTemplates = await _context.ProjectMaterialTemplates
            .Include(pmt => pmt.MaterialTemplate)
            .ThenInclude(mt => mt.Items)
            .Where(pmt => pmt.ProjectId == projectId)
            .ToListAsync(cancellationToken);

        var firstActiveMaterialTemplate = projectTemplates
            .FirstOrDefault(pt => pt.MaterialTemplate.IsActive);

        foreach (var boxTypeId in newBoxTypeIds)
        {
            // --- Activity template ---
            if (project?.ActivityTemplateId.HasValue == true)
            {
                var boxType = await _context.ProjectBoxTypes
                    .FirstOrDefaultAsync(bt => bt.Id == boxTypeId, cancellationToken);

                if (boxType != null && !boxType.ActivityTemplateId.HasValue)
                {
                    boxType.ActivityTemplateId = project.ActivityTemplateId;
                    _unitOfWork.Repository<ProjectBoxType>().Update(boxType);
                }
            }

            // --- Material template ---
            if (firstActiveMaterialTemplate != null)
            {
                var hasExistingTemplate = await _context.BoxTypeMaterialTemplates
                    .AnyAsync(btmt => btmt.ProjectBoxTypeId == boxTypeId, cancellationToken);

                if (!hasExistingTemplate)
                {
                    var boxTypeAssignment = new BoxTypeMaterialTemplate
                    {
                        ProjectBoxTypeId = boxTypeId,
                        MaterialTemplateId = firstActiveMaterialTemplate.MaterialTemplateId,
                        AssignedDate = DateTime.UtcNow,
                        AssignedBy = "System (Auto-assigned from project template)"
                    };

                    await _unitOfWork.Repository<BoxTypeMaterialTemplate>().AddAsync(boxTypeAssignment, cancellationToken);
                    await CreateBoxTypeMaterialsFromTemplate(boxTypeId, firstActiveMaterialTemplate.MaterialTemplate, cancellationToken);
                    await ApplyTemplatesToExistingBoxesOfType(boxTypeId, cancellationToken);
                }
            }
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    /// <summary>
    /// Creates BoxTypeMaterial records from a template for box type level tracking
    /// </summary>
    private async Task CreateBoxTypeMaterialsFromTemplate(int projectBoxTypeId, MaterialTemplate template, CancellationToken cancellationToken)
    {
        // Get the box type to calculate required date
        var projectBoxType = await _context.ProjectBoxTypes
            .Include(pbt => pbt.Project)
            .FirstOrDefaultAsync(pbt => pbt.Id == projectBoxTypeId, cancellationToken);

        if (projectBoxType == null)
            return;

        foreach (var templateItem in template.Items)
        {
            // Check if material already exists for this box type
            var existingBoxTypeMaterial = await _context.BoxTypeMaterials
                .FirstOrDefaultAsync(btm => btm.ProjectBoxTypeId == projectBoxTypeId && 
                                            btm.MaterialId == templateItem.MaterialId, 
                                     cancellationToken);

            if (existingBoxTypeMaterial == null)
            {
                // Calculate required by date based on project start date or use current date
                var baseDate = projectBoxType.Project?.PlannedStartDate ?? DateTime.UtcNow;
               // var requiredByDate = baseDate.AddDays(-templateItem.RequiredBeforeDays);

                var boxTypeMaterial = new BoxTypeMaterial
                {
                    ProjectBoxTypeId = projectBoxTypeId,
                    MaterialId = templateItem.MaterialId,
                    RequiredBeforeDays = templateItem.RequiredBeforeDays,
                    
                    IsArrived = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<BoxTypeMaterial>().AddAsync(boxTypeMaterial, cancellationToken);
            }
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    /// <summary>
    /// Assigns a specific template to a box type and applies it to all existing boxes.
    /// Each box type may only hold ONE material template; any existing assignment is
    /// replaced rather than accumulated.
    /// </summary>
    public async Task AssignTemplateToBoxTypeAsync(int projectBoxTypeId, Guid templateId, string assignedBy, CancellationToken cancellationToken = default)
    {
        // If this exact template is already the only assignment, nothing to do.
        var existingAssignments = await _context.BoxTypeMaterialTemplates
            .Where(btmt => btmt.ProjectBoxTypeId == projectBoxTypeId)
            .ToListAsync(cancellationToken);

        if (existingAssignments.Count == 1 && existingAssignments[0].MaterialTemplateId == templateId)
            return; // Already the current (and only) assignment

        // Remove ALL stale assignments so the one-per-box-type constraint is respected.
        foreach (var old in existingAssignments)
            _context.BoxTypeMaterialTemplates.Remove(old);

        if (existingAssignments.Count > 0)
            await _unitOfWork.CompleteAsync(cancellationToken);

        // Create the single new assignment
        var assignment = new BoxTypeMaterialTemplate
        {
            ProjectBoxTypeId = projectBoxTypeId,
            MaterialTemplateId = templateId,
            AssignedDate = DateTime.UtcNow,
            AssignedBy = assignedBy
        };

        await _unitOfWork.Repository<BoxTypeMaterialTemplate>().AddAsync(assignment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Apply template to existing boxes of this type
        await ApplyTemplatesToExistingBoxesOfType(projectBoxTypeId, cancellationToken);
    }

    /// <summary>
    /// Applies assigned templates to all existing boxes of a specific box type
    /// </summary>
    private async Task ApplyTemplatesToExistingBoxesOfType(int projectBoxTypeId, CancellationToken cancellationToken)
    {
        // Get all boxes of this type
        var boxes = await _context.Boxes
            .Where(b => b.ProjectBoxTypeId == projectBoxTypeId)
            .ToListAsync(cancellationToken);

        if (!boxes.Any())
            return;

        // Get ALL templates assigned to this box type and take the most recent one
        // (In case there are legacy duplicates, we only want to apply the current one)
        var boxTypeTemplates = await _context.BoxTypeMaterialTemplates
            .Include(btmt => btmt.MaterialTemplate)
            .ThenInclude(mt => mt.Items)
            .Where(btmt => btmt.ProjectBoxTypeId == projectBoxTypeId)
            .OrderByDescending(btmt => btmt.AssignedDate)
            .ToListAsync(cancellationToken);

        var mostRecentTemplate = boxTypeTemplates.FirstOrDefault();
        
        if (mostRecentTemplate == null || !mostRecentTemplate.MaterialTemplate.IsActive)
            return;

        // Apply template to each box
        foreach (var box in boxes)
        {
            await ApplyTemplateToBox(box, mostRecentTemplate.MaterialTemplate, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    /// <summary>
    /// Applies a template's materials to a specific box (skips already-assigned materials).
    /// </summary>
    private async Task ApplyTemplateToBox(Box box, MaterialTemplate template, CancellationToken cancellationToken)
    {
        foreach (var templateItem in template.Items)
        {
            var existingBoxMaterial = await _context.BoxMaterials
                .FirstOrDefaultAsync(bm => bm.BoxId == box.BoxId && 
                                          bm.MaterialId == templateItem.MaterialId, 
                                     cancellationToken);

            if (existingBoxMaterial != null)
                continue;

            var requiredByDate = box.PlannedStartDate.HasValue
                ? box.PlannedStartDate.Value.AddDays(-templateItem.RequiredBeforeDays)
                : DateTime.UtcNow.AddDays(templateItem.RequiredBeforeDays);

            var boxMaterial = new BoxMaterial
            {
                BoxId = box.BoxId,
                MaterialId = templateItem.MaterialId,
                RequiredBeforeDays = templateItem.RequiredBeforeDays,
                RequiredByDate = requiredByDate,
                IsArrived = false
            };

            await _unitOfWork.Repository<BoxMaterial>().AddAsync(boxMaterial, cancellationToken);
        }
    }

    // ── New methods ─────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task PropagateActivityTemplateToBoxTypeBoxesAsync(
        int projectBoxTypeId,
        Guid activityTemplateId,
        CancellationToken cancellationToken = default)
    {
        var boxes = await _context.Boxes
            .Where(b => b.ProjectBoxTypeId == projectBoxTypeId &&
                        (b.Status == BoxStatusEnum.NotStarted || b.Status == BoxStatusEnum.ReadyToStart))
            .ToListAsync(cancellationToken);

        foreach (var box in boxes)
        {
            await _boxActivityService.ResetBoxActivitiesFromTemplateAsync(box, activityTemplateId, cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> ChangeMaterialTemplateForBoxTypeAsync(
        int projectBoxTypeId,
        Guid newMaterialTemplateId,
        Guid? oldMaterialTemplateId,
        string assignedBy,
        CancellationToken cancellationToken = default)
    {
        // Case A – reject if any BoxTypeMaterial has arrival data:
        //   ArrivalQuantity > 0  →  BoxTypeMaterial.DeliveredQuantity > 0
        //   Arrival != 0         →  BoxTypeMaterial.DeliveryProgress != 0
        var hasArrivalData = await _context.BoxTypeMaterials
            .AnyAsync(btm => btm.ProjectBoxTypeId == projectBoxTypeId &&
                             (btm.DeliveryProgress != 0 ||
                              (btm.DeliveredQuantity != null && btm.DeliveredQuantity > 0)),
                      cancellationToken);

        if (hasArrivalData)
            return Result.Failure<bool>(
                "Cannot change the material template: one or more materials under this box type already have arrival data.");

        // Load new template
        var newTemplate = await _context.MaterialTemplates
            .Include(mt => mt.Items)
            .FirstOrDefaultAsync(mt => mt.MaterialTemplateId == newMaterialTemplateId, cancellationToken);

        if (newTemplate == null)
            return Result.Failure<bool>("Material template not found.");

        if (!newTemplate.IsActive)
            return Result.Failure<bool>("Material template is not active.");

        // Remove old BoxTypeMaterialTemplate assignment(s)
        var oldAssignments = await _context.BoxTypeMaterialTemplates
            .Where(btmt => btmt.ProjectBoxTypeId == projectBoxTypeId)
            .ToListAsync(cancellationToken);

        foreach (var old in oldAssignments)
            _context.BoxTypeMaterialTemplates.Remove(old);

        // Remove old BoxTypeMaterial records
        var oldBoxTypeMaterials = await _context.BoxTypeMaterials
            .Where(btm => btm.ProjectBoxTypeId == projectBoxTypeId)
            .ToListAsync(cancellationToken);

        foreach (var btm in oldBoxTypeMaterials)
            _unitOfWork.Repository<BoxTypeMaterial>().Delete(btm);

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Add new BoxTypeMaterialTemplate assignment
        var newAssignment = new BoxTypeMaterialTemplate
        {
            ProjectBoxTypeId = projectBoxTypeId,
            MaterialTemplateId = newMaterialTemplateId,
            AssignedDate = DateTime.UtcNow,
            AssignedBy = assignedBy
        };
        await _unitOfWork.Repository<BoxTypeMaterialTemplate>().AddAsync(newAssignment, cancellationToken);

        // Add new BoxTypeMaterial records
        await CreateBoxTypeMaterialsFromTemplate(projectBoxTypeId, newTemplate, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Replace BoxMaterials on all boxes of this type
        var boxes = await _context.Boxes
            .Where(b => b.ProjectBoxTypeId == projectBoxTypeId)
            .ToListAsync(cancellationToken);

        foreach (var box in boxes)
        {
            var existingBoxMaterials = await _context.BoxMaterials
                .Where(bm => bm.BoxId == box.BoxId)
                .ToListAsync(cancellationToken);

            _context.BoxMaterials.RemoveRange(existingBoxMaterials);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await ApplyTemplateToBox(box, newTemplate, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        return Result.Success(true);
    }
}

