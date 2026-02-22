using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Services;

public interface IMaterialTemplateApplicationService
{
    Task ApplyTemplatesToNewBox(Box box, CancellationToken cancellationToken = default);
}

public class MaterialTemplateApplicationService : IMaterialTemplateApplicationService
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public MaterialTemplateApplicationService(IDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ApplyTemplatesToNewBox(Box box, CancellationToken cancellationToken = default)
    {
        var templatesToApply = new List<MaterialTemplate>();

        // Check if box type has a template assigned (takes precedence)
        if (box.ProjectBoxTypeId.HasValue)
        {
            var boxTypeTemplate = await _context.BoxTypeMaterialTemplates
                .Include(btmt => btmt.MaterialTemplate)
                .ThenInclude(mt => mt.Items)
                .ThenInclude(mti => mti.Material)
                .FirstOrDefaultAsync(btmt => btmt.ProjectBoxTypeId == box.ProjectBoxTypeId.Value, 
                                    cancellationToken);

            if (boxTypeTemplate != null && boxTypeTemplate.MaterialTemplate.IsActive)
            {
                templatesToApply.Add(boxTypeTemplate.MaterialTemplate);
            }
        }

        // If no box type template, check for project-level templates (now supports multiple)
        if (templatesToApply.Count == 0)
        {
            var projectTemplates = await _context.ProjectMaterialTemplates
                .Include(pmt => pmt.MaterialTemplate)
                .ThenInclude(mt => mt.Items)
                .ThenInclude(mti => mti.Material)
                .Where(pmt => pmt.ProjectId == box.ProjectId)
                .ToListAsync(cancellationToken);

            // Add all active project templates
            foreach (var projectTemplate in projectTemplates)
            {
                if (projectTemplate.MaterialTemplate.IsActive)
                {
                    templatesToApply.Add(projectTemplate.MaterialTemplate);
                }
            }
        }

        // Apply all templates found
        foreach (var template in templatesToApply)
        {
            await ApplyTemplateToBox(box, template, cancellationToken);
        }
    }

    private async Task ApplyTemplateToBox(Box box, MaterialTemplate template, CancellationToken cancellationToken)
    {
        foreach (var templateItem in template.Items)
        {
            // Check if material already assigned to this box
            var existingBoxMaterial = await _context.BoxMaterials
                .FirstOrDefaultAsync(bm => bm.BoxId == box.BoxId && 
                                          bm.MaterialId == templateItem.MaterialId, 
                                     cancellationToken);

            if (existingBoxMaterial != null)
                continue; // Skip if already assigned

            // Calculate required by date
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

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}

