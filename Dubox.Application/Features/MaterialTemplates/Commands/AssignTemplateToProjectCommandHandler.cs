using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Commands;


    public class AssignTemplateToProjectCommandHandler
      : IRequestHandler<AssignTemplateToProjectCommand, Result<bool>>
    {
        private readonly IDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AssignTemplateToProjectCommandHandler(
            IDbContext context,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            AssignTemplateToProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>()
                .GetByIdAsync(request.ProjectId, cancellationToken);

            if (project == null)
                return Result.Failure<bool>("Project not found.");

            var template = await _context.MaterialTemplates
                .Include(mt => mt.Items)
                .FirstOrDefaultAsync(
                    mt => mt.MaterialTemplateId == request.MaterialTemplateId && mt.IsActive,
                    cancellationToken);

            if (template == null)
                return Result.Failure<bool>("Template not found or inactive.");

            var existingAssignment = await _context.ProjectMaterialTemplates
                .FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

            // Prefer the explicitly supplied old template ID (provided by UpdateProjectCommandHandler
            // which already has the value before any DB changes are made).
            // Fall back to the DB-derived value for direct API calls that don't supply it.
            var oldTemplateId =
                request.OldMaterialTemplateId
                ?? existingAssignment?.MaterialTemplateId;

            if (existingAssignment?.MaterialTemplateId == request.MaterialTemplateId)
                return Result.Failure<bool>("Template already assigned.");

            if (existingAssignment != null)
            {
                existingAssignment.MaterialTemplateId = request.MaterialTemplateId;
                existingAssignment.AssignedDate = DateTime.UtcNow;
                existingAssignment.AssignedBy = _currentUserService.Username;

                _unitOfWork.Repository<ProjectMaterialTemplate>().Update(existingAssignment);
            }
            else
            {
                var assignment = new ProjectMaterialTemplate
                {
                    ProjectId = request.ProjectId,
                    MaterialTemplateId = request.MaterialTemplateId,
                    AssignedDate = DateTime.UtcNow,
                    AssignedBy = _currentUserService.Username
                };

                await _unitOfWork.Repository<ProjectMaterialTemplate>()
                    .AddAsync(assignment, cancellationToken);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            await PropagateTemplateToBoxTypesAsync(
                request.ProjectId,
                request.MaterialTemplateId,
                oldTemplateId,
                cancellationToken);

            return Result.Success(true);
        }

        private async Task PropagateTemplateToBoxTypesAsync(
            Guid projectId,
            Guid newTemplateId,
            Guid? oldTemplateId,
            CancellationToken cancellationToken)
        {
            var boxTypes = await _context.ProjectBoxTypes
                .Where(bt => bt.ProjectId == projectId && bt.IsActive)
                .ToListAsync(cancellationToken);

            var template = await _context.MaterialTemplates
                .Include(mt => mt.Items)
                .FirstOrDefaultAsync(mt => mt.MaterialTemplateId == newTemplateId, cancellationToken);

            if (template == null)
                return;

            foreach (var boxType in boxTypes)
            {
                // Order by AssignedDate DESC to always compare against the most-recent
                // (current) assignment — a box type may have accumulated historical rows.
                var assignment = await _context.BoxTypeMaterialTemplates
                    .Where(x => x.ProjectBoxTypeId == boxType.Id)
                    .OrderByDescending(x => x.AssignedDate)
                    .FirstOrDefaultAsync(cancellationToken);

                bool hasNoTemplate = assignment == null;

                bool isUsingOldTemplate =
                    assignment != null &&
                    oldTemplateId.HasValue &&
                    assignment.MaterialTemplateId == oldTemplateId.Value;

                if (!hasNoTemplate && !isUsingOldTemplate)
                    continue;

                if (isUsingOldTemplate)
                {
                    var hasArrival = await _context.BoxTypeMaterials
                        .AnyAsync(b =>
                            b.ProjectBoxTypeId == boxType.Id &&
                            (b.DeliveryProgress != 0 ||
                             (b.DeliveredQuantity != null && b.DeliveredQuantity > 0)),
                            cancellationToken);

                    if (hasArrival)
                        continue;
                }

                // Remove old materials
                var oldMaterials = await _context.BoxTypeMaterials
                    .Where(b => b.ProjectBoxTypeId == boxType.Id)
                    .ToListAsync(cancellationToken);

                _context.BoxTypeMaterials.RemoveRange(oldMaterials);

                var boxes = await _context.Boxes
                    .Where(b => b.ProjectBoxTypeId == boxType.Id)
                    .ToListAsync(cancellationToken);

                foreach (var box in boxes)
                {
                    var boxMaterials = await _context.BoxMaterials
                        .Where(bm => bm.BoxId == box.BoxId)
                        .ToListAsync(cancellationToken);

                    _context.BoxMaterials.RemoveRange(boxMaterials);

                    await ApplyTemplateToBox(box, template, cancellationToken);
                }

                if (assignment != null)
                {
                    assignment.MaterialTemplateId = newTemplateId;
                    assignment.AssignedDate = DateTime.UtcNow;
                    assignment.AssignedBy = _currentUserService.Username;

                    _unitOfWork.Repository<BoxTypeMaterialTemplate>().Update(assignment);
                }
                else
                {
                    await _unitOfWork.Repository<BoxTypeMaterialTemplate>()
                        .AddAsync(new BoxTypeMaterialTemplate
                        {
                            ProjectBoxTypeId = boxType.Id,
                            MaterialTemplateId = newTemplateId,
                            AssignedDate = DateTime.UtcNow,
                            AssignedBy = _currentUserService.Username
                        }, cancellationToken);
                }

                await CreateBoxTypeMaterialsFromTemplate(boxType.Id, template, cancellationToken);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        private async Task CreateBoxTypeMaterialsFromTemplate(
            int boxTypeId,
            MaterialTemplate template,
            CancellationToken cancellationToken)
        {
            foreach (var item in template.Items)
            {
                await _unitOfWork.Repository<BoxTypeMaterial>()
                    .AddAsync(new BoxTypeMaterial
                    {
                        ProjectBoxTypeId = boxTypeId,
                        MaterialId = item.MaterialId,
                        RequiredBeforeDays = item.RequiredBeforeDays,
                        IsArrived = false,
                        CreatedDate = DateTime.UtcNow
                    }, cancellationToken);
            }
        }

        private async Task ApplyTemplateToBox(
            Box box,
            MaterialTemplate template,
            CancellationToken cancellationToken)
        {
            foreach (var item in template.Items)
            {
                var requiredByDate = box.PlannedStartDate.HasValue
                    ? box.PlannedStartDate.Value.AddDays(-item.RequiredBeforeDays)
                    : DateTime.UtcNow.AddDays(item.RequiredBeforeDays);

                await _unitOfWork.Repository<BoxMaterial>()
                    .AddAsync(new BoxMaterial
                    {
                        BoxId = box.BoxId,
                        MaterialId = item.MaterialId,
                        RequiredBeforeDays = item.RequiredBeforeDays,
                        RequiredByDate = requiredByDate,
                        IsArrived = false
                    }, cancellationToken);
            }
        }
  
}

