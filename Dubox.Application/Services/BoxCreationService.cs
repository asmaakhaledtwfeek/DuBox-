using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dubox.Application.Services
{
    public interface IBoxCreationService
    {
        Task<BoxDto> CreateAsync(
            Box box,
            Project project,
            Guid currentUserId,
            string auditAction,
            string auditDescription,
            Guid? activityTemplateId,
            CancellationToken cancellationToken);
    }

    public class BoxCreationService : IBoxCreationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBoxActivityService _boxActivityService;
        private readonly IProjectProgressService _projectProgressService;
        private readonly ISerialNumberService _serialNumberService;
        private readonly IMaterialTemplateApplicationService _templateService;

        public BoxCreationService(
            IUnitOfWork unitOfWork,
            IBoxActivityService boxActivityService,
            IProjectProgressService projectProgressService,
            ISerialNumberService serialNumberService,
            IMaterialTemplateApplicationService templateService)
        {
            _unitOfWork = unitOfWork;
            _boxActivityService = boxActivityService;
            _projectProgressService = projectProgressService;
            _serialNumberService = serialNumberService;
            _templateService = templateService;
        }

        public async Task<BoxDto> CreateAsync(
            Box box,
            Project project,
            Guid currentUserId,
            string auditAction,
            string auditDescription,
            Guid? activityTemplateId,
            CancellationToken cancellationToken)
        {
            // Generate BoxNumber - unique within project type/subtype
            box.BoxNumber = await GenerateBoxNumberAsync(
                box.ProjectId,
                box.ProjectBoxTypeId,
                box.ProjectBoxSubTypeId,
                cancellationToken);

            // Generate sequential + serial
            var lastSeq = _unitOfWork.Repository<Box>().Get()
                .Where(b => b.ProjectId == box.ProjectId)
                .Max(b => (int?)b.SequentialNumber) ?? 0;

            box.SequentialNumber = lastSeq + 1;

            var year = project.CreatedDate.Year.ToString()[^2..];
            box.SerialNumber = _serialNumberService.GenerateSerialNumber("X", lastSeq, year);

            box.CreatedBy = currentUserId;
            box.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<Box>().AddAsync(box, cancellationToken);
            await _unitOfWork.CompleteAsync();

            box = _unitOfWork.Repository<Box>()
                .GetEntityWithSpec(new GetBoxWithIncludesSpecification(box.BoxId));

            // Resolve activity template priority: box type template > project template > activity master
            Guid? resolvedTemplateId = null;
            
            // Priority 1: Check if box type has an assigned activity template
            if (box.ProjectBoxTypeId.HasValue)
            {
                var boxType = _unitOfWork.Repository<ProjectBoxType>().Get()
                    .Where(bt => bt.Id == box.ProjectBoxTypeId.Value)
                    .FirstOrDefault();
                
                if (boxType?.ActivityTemplateId.HasValue == true)
                {
                    resolvedTemplateId = boxType.ActivityTemplateId;
                }
            }
            
            // Priority 2: If no box type template, check project-level template
            if (!resolvedTemplateId.HasValue)
            {
                resolvedTemplateId = project.ActivityTemplateId;
            }

            // Assign activities based on resolved template
            if (resolvedTemplateId.HasValue)
            {
                // Use activity template (from box type or project)
                await _boxActivityService.CopyActivitiesFromTemplateToBox(box, resolvedTemplateId.Value, cancellationToken);
            }
            else
            {
                // Priority 3: Use activity master (standard approach based on box type)
                await _boxActivityService.CopyActivitiesToBox(box, cancellationToken);
            }

            // Apply material templates (box type template overrides project template)
            await _templateService.ApplyTemplatesToNewBox(box, cancellationToken);

            // Create BoxMaterial records for selected project materials (only if no templates applied)
            await CreateBoxMaterialsAsync(box, cancellationToken);

            // Update project total
            var oldTotalBoxes = project.TotalBoxes;
            project.TotalBoxes++;
            _unitOfWork.Repository<Project>().Update(project);

            // Audit logs
            const string dateFormat = "yyyy-MM-dd HH:mm:ss";

            await _unitOfWork.Repository<AuditLog>().AddAsync(new AuditLog
            {
                TableName = nameof(Box),
                RecordId = box.BoxId,
                Action = auditAction,
                OldValues = auditAction == "Creation" ? "N/A (New Entity)" : null,
                NewValues = $"Tag: {box.BoxTag}, ProjectId: {box.ProjectId}, SerialNumber: {box.SerialNumber}, PlannedStart: {box.PlannedStartDate?.ToString(dateFormat) ?? "N/A"}, Duration: {box.Duration}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = auditDescription
            }, cancellationToken);

            await _unitOfWork.Repository<AuditLog>().AddAsync(new AuditLog
            {
                TableName = nameof(Project),
                RecordId = project.ProjectId,
                Action = "TotalBoxesUpdate",
                OldValues = $"TotalBoxes: {oldTotalBoxes}",
                NewValues = $"TotalBoxes: {project.TotalBoxes}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow
            }, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _projectProgressService.UpdateProjectProgressAsync(
                project.ProjectId,
                currentUserId,
                auditDescription,
                cancellationToken);

            return box.Adapt<BoxDto>() with
            {
                FactoryId = box.FactoryId,
                FactoryCode = box.Factory?.FactoryCode,
                FactoryName = box.Factory?.FactoryName
            };
        }

        private async Task<string> GenerateBoxNumberAsync(
            Guid projectId,
            int? projectBoxTypeId,
            int? projectBoxSubTypeId,
            CancellationToken cancellationToken)
        {
            var boxRepository = _unitOfWork.Repository<Box>();
            var query = boxRepository.Get()
                .Where(b => b.ProjectId == projectId 
                    && b.ProjectBoxTypeId == projectBoxTypeId);

            // If subtype exists, filter by subtype; otherwise ensure no subtype
            if (projectBoxSubTypeId.HasValue)
                query = query.Where(b => b.ProjectBoxSubTypeId == projectBoxSubTypeId.Value);
            else
                query = query.Where(b => b.ProjectBoxSubTypeId == null);

            var existingBoxNumbers =  query
                .Where(b => !string.IsNullOrEmpty(b.BoxNumber))
                .Select(b => b.BoxNumber)
                .ToList();

            int maxNumber = 0;
            foreach (var boxNumber in existingBoxNumbers)
            {
                if (int.TryParse(boxNumber, out int num))
                    maxNumber = Math.Max(maxNumber, num);
            }

            int nextNumber = maxNumber + 1;

            return nextNumber.ToString("000");
        }

        private async Task CreateBoxMaterialsAsync(Box box, CancellationToken cancellationToken)
        {
            // If no box type is assigned, we cannot determine which materials to assign
            if (!box.ProjectBoxTypeId.HasValue)
                return;

            // Get materials assigned to this specific box type
            var boxTypeMaterials = await _unitOfWork.Repository<BoxTypeMaterial>()
                .FindAsync(btm => btm.ProjectBoxTypeId == box.ProjectBoxTypeId.Value, cancellationToken);

            if (!boxTypeMaterials.Any())
                return; // No materials assigned to this box type

            // Get material details
            var materialIds = boxTypeMaterials.Select(btm => btm.MaterialId).ToList();
            var materials = await _unitOfWork.Repository<Material>()
                .FindAsync(m => materialIds.Contains(m.MaterialId), cancellationToken);

            var boxMaterials = new List<BoxMaterial>();

            foreach (var boxTypeMaterial in boxTypeMaterials)
            {
                var material = materials.FirstOrDefault(m => m.MaterialId == boxTypeMaterial.MaterialId);
                if (material == null) continue;

                // Use RequiredBeforeDays from BoxTypeMaterial (which may come from material template or default)
                var requiredBeforeDays = boxTypeMaterial.RequiredBeforeDays;

                // Calculate required by date based on box planned start date
                var requiredByDate = box.PlannedStartDate.HasValue
                    ? box.PlannedStartDate.Value.AddDays(-requiredBeforeDays)
                    : DateTime.UtcNow.AddDays(requiredBeforeDays);

                var boxMaterial = new BoxMaterial
                {
                    BoxId = box.BoxId,
                    MaterialId = material.MaterialId,
                    ProjectMaterialId = null, // Not linked to project material anymore
                    RequiredBeforeDays = requiredBeforeDays,
                    RequiredByDate = requiredByDate,
                    IsArrived = false,
                    CreatedDate = DateTime.UtcNow
                };

                boxMaterials.Add(boxMaterial);
            }

            if (boxMaterials.Any())
            {
                await _unitOfWork.Repository<BoxMaterial>().AddRangeAsync(boxMaterials, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }
        }
    }

}
