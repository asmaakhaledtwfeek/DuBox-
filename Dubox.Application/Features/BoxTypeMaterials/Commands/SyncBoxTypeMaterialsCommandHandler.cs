using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public class SyncBoxTypeMaterialsCommandHandler : IRequestHandler<SyncBoxTypeMaterialsCommand, Result<int>>
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public SyncBoxTypeMaterialsCommandHandler(IDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(SyncBoxTypeMaterialsCommand request, CancellationToken cancellationToken)
    {
        int createdCount = 0;

        // Get the project with start date
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<int>("Project not found.");

        var baseDate = project.PlannedStartDate ?? DateTime.UtcNow;

        // Get all box type template assignments for this project
        var boxTypeTemplateAssignments = await _context.BoxTypeMaterialTemplates
            .Include(btmt => btmt.ProjectBoxType)
            .Include(btmt => btmt.MaterialTemplate)
                .ThenInclude(mt => mt.Items)
            .Where(btmt => btmt.ProjectBoxType.ProjectId == request.ProjectId)
            .ToListAsync(cancellationToken);

        foreach (var assignment in boxTypeTemplateAssignments)
        {
            if (!assignment.MaterialTemplate.IsActive)
                continue;

            // Create BoxTypeMaterial records for each template item
            foreach (var templateItem in assignment.MaterialTemplate.Items)
            {
                // Check if BoxTypeMaterial already exists
                var existingBoxTypeMaterial = await _context.BoxTypeMaterials
                    .FirstOrDefaultAsync(btm => 
                        btm.ProjectBoxTypeId == assignment.ProjectBoxTypeId && 
                        btm.MaterialId == templateItem.MaterialId, 
                        cancellationToken);

                if (existingBoxTypeMaterial == null)
                {
                    //var requiredByDate = baseDate.AddDays(-templateItem.RequiredBeforeDays);

                    var boxTypeMaterial = new BoxTypeMaterial
                    {
                        ProjectBoxTypeId = assignment.ProjectBoxTypeId,
                        MaterialId = templateItem.MaterialId,
                        RequiredBeforeDays = templateItem.RequiredBeforeDays,
                       
                        IsArrived = false,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _unitOfWork.Repository<BoxTypeMaterial>().AddAsync(boxTypeMaterial, cancellationToken);
                    createdCount++;
                }
            }
        }

        // Also check for project-level templates (for box types without specific template assignments)
        var projectTemplates = await _context.ProjectMaterialTemplates
            .Include(pmt => pmt.MaterialTemplate)
                .ThenInclude(mt => mt.Items)
            .Where(pmt => pmt.ProjectId == request.ProjectId)
            .ToListAsync(cancellationToken);

        if (projectTemplates.Any())
        {
            // Get all box types for this project
            var projectBoxTypes = await _context.ProjectBoxTypes
                .Where(pbt => pbt.ProjectId == request.ProjectId && pbt.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var boxType in projectBoxTypes)
            {
                // Check if this box type has a specific template assigned
                var hasSpecificTemplate = boxTypeTemplateAssignments.Any(bta => bta.ProjectBoxTypeId == boxType.Id);

                if (!hasSpecificTemplate)
                {
                    // Apply project templates to this box type
                    foreach (var projectTemplate in projectTemplates)
                    {
                        if (!projectTemplate.MaterialTemplate.IsActive)
                            continue;

                        foreach (var templateItem in projectTemplate.MaterialTemplate.Items)
                        {
                            // Check if BoxTypeMaterial already exists
                            var existingBoxTypeMaterial = await _context.BoxTypeMaterials
                                .FirstOrDefaultAsync(btm => 
                                    btm.ProjectBoxTypeId == boxType.Id && 
                                    btm.MaterialId == templateItem.MaterialId, 
                                    cancellationToken);

                            if (existingBoxTypeMaterial == null)
                            {
                               // var requiredByDate = baseDate.AddDays(-templateItem.RequiredBeforeDays);

                                var boxTypeMaterial = new BoxTypeMaterial
                                {
                                    ProjectBoxTypeId = boxType.Id,
                                    MaterialId = templateItem.MaterialId,
                                    RequiredBeforeDays = templateItem.RequiredBeforeDays,
                                    
                                    IsArrived = false,
                                    CreatedDate = DateTime.UtcNow
                                };

                                await _unitOfWork.Repository<BoxTypeMaterial>().AddAsync(boxTypeMaterial, cancellationToken);
                                createdCount++;
                            }
                        }
                    }
                }
            }
        }

        // Also sync project-level selected materials (not from templates)
        var projectMaterials = await _context.ProjectMaterials
            .Include(pm => pm.Material)
            .Where(pm => pm.ProjectId == request.ProjectId && pm.IsSelected)
            .ToListAsync(cancellationToken);

        if (projectMaterials.Any())
        {
            // Get all box types for this project
            var projectBoxTypes = await _context.ProjectBoxTypes
                .Where(pbt => pbt.ProjectId == request.ProjectId && pbt.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var boxType in projectBoxTypes)
            {
                foreach (var projectMaterial in projectMaterials)
                {
                    // Check if BoxTypeMaterial already exists
                    var existingBoxTypeMaterial = await _context.BoxTypeMaterials
                        .FirstOrDefaultAsync(btm => 
                            btm.ProjectBoxTypeId == boxType.Id && 
                            btm.MaterialId == projectMaterial.MaterialId, 
                            cancellationToken);

                    if (existingBoxTypeMaterial == null)
                    {
                        var boxTypeMaterial = new BoxTypeMaterial
                        {
                            ProjectBoxTypeId = boxType.Id,
                            MaterialId = projectMaterial.MaterialId,
                            RequiredBeforeDays = projectMaterial.Material.DefaultRequiredBeforeDays,
                            IsArrived = false,
                            CreatedDate = DateTime.UtcNow
                        };

                        await _unitOfWork.Repository<BoxTypeMaterial>().AddAsync(boxTypeMaterial, cancellationToken);
                        createdCount++;
                    }
                }
            }
        }

        if (createdCount > 0)
        {
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        return Result.Success(createdCount);
    }
}






