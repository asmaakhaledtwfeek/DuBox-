using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public class AssignTemplateToBoxTypeCommandHandler : IRequestHandler<AssignTemplateToBoxTypeCommand, Result<bool>>
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AssignTemplateToBoxTypeCommandHandler(
        IDbContext context,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(AssignTemplateToBoxTypeCommand request, CancellationToken cancellationToken)
    {
        // Validate box type exists
        var boxType = await _context.ProjectBoxTypes
            .FirstOrDefaultAsync(pbt => pbt.Id == request.ProjectBoxTypeId, cancellationToken);

        if (boxType == null)
            return Result.Failure<bool>("Box type not found.");

        // Validate template exists
        var template = await _context.MaterialTemplates
            .Include(mt => mt.Items)
            .ThenInclude(mti => mti.Material)
            .FirstOrDefaultAsync(mt => mt.MaterialTemplateId == request.MaterialTemplateId, cancellationToken);

        if (template == null)
            return Result.Failure<bool>("Template not found.");

        if (!template.IsActive)
            return Result.Failure<bool>("Template is not active.");

        // Check if this exact template is already assigned (would be redundant)
        var exactMatch = await _context.BoxTypeMaterialTemplates
            .FirstOrDefaultAsync(btmt => btmt.ProjectBoxTypeId == request.ProjectBoxTypeId && 
                                        btmt.MaterialTemplateId == request.MaterialTemplateId, 
                                 cancellationToken);

        if (exactMatch != null)
            return Result.Failure<bool>("This template is already assigned to the box type.");

        // Remove any existing template assignment for this box type (to replace it with the new one)
        var existingAssignments = await _context.BoxTypeMaterialTemplates
            .Where(btmt => btmt.ProjectBoxTypeId == request.ProjectBoxTypeId)
            .ToListAsync(cancellationToken);

        foreach (var oldAssignment in existingAssignments)
        {
            _unitOfWork.Repository<BoxTypeMaterialTemplate>().Delete(oldAssignment);
        }

        // Create new assignment
        var assignment = new BoxTypeMaterialTemplate
        {
            ProjectBoxTypeId = request.ProjectBoxTypeId,
            MaterialTemplateId = request.MaterialTemplateId,
            AssignedDate = DateTime.UtcNow,
            AssignedBy = _currentUserService.Username
        };

        await _unitOfWork.Repository<BoxTypeMaterialTemplate>().AddAsync(assignment, cancellationToken);

        // Remove old BoxTypeMaterial records for this box type (box type-level material tracking)
        var existingBoxTypeMaterials = await _context.BoxTypeMaterials
            .Where(btm => btm.ProjectBoxTypeId == request.ProjectBoxTypeId)
            .ToListAsync(cancellationToken);

        foreach (var oldMaterial in existingBoxTypeMaterials)
        {
            _unitOfWork.Repository<BoxTypeMaterial>().Delete(oldMaterial);
        }

        // Create new BoxTypeMaterial records from the template (for box type level tracking)
        await CreateBoxTypeMaterialsFromTemplate(request.ProjectBoxTypeId, template, cancellationToken);

        // Apply template to all boxes of this type
        var boxes = await _context.Boxes
            .Where(b => b.ProjectBoxTypeId == request.ProjectBoxTypeId)
            .ToListAsync(cancellationToken);

        foreach (var box in boxes)
        {
            // Remove materials from project template (box type template overrides)
            var existingMaterials = await _context.BoxMaterials
                .Where(bm => bm.BoxId == box.BoxId)
                .ToListAsync(cancellationToken);

            foreach (var material in existingMaterials)
            {
                _unitOfWork.Repository<BoxMaterial>().Delete(material);
            }

            // Apply box type template materials
            await ApplyTemplateToBox(box, template, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }

    private async Task CreateBoxTypeMaterialsFromTemplate(int projectBoxTypeId, MaterialTemplate template, CancellationToken cancellationToken)
    {
        foreach (var templateItem in template.Items)
        {
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

    private async Task ApplyTemplateToBox(Box box, MaterialTemplate template, CancellationToken cancellationToken)
    {
        foreach (var templateItem in template.Items)
        {
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
    }
}

