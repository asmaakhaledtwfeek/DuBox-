using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Mapster;
using System;
using System.Collections.Generic;
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
            CancellationToken cancellationToken);
    }

    public class BoxCreationService : IBoxCreationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBoxActivityService _boxActivityService;
        private readonly IProjectProgressService _projectProgressService;
        private readonly ISerialNumberService _serialNumberService;

        public BoxCreationService(
            IUnitOfWork unitOfWork,
            IBoxActivityService boxActivityService,
            IProjectProgressService projectProgressService,
            ISerialNumberService serialNumberService)
        {
            _unitOfWork = unitOfWork;
            _boxActivityService = boxActivityService;
            _projectProgressService = projectProgressService;
            _serialNumberService = serialNumberService;
        }

        public async Task<BoxDto> CreateAsync(
            Box box,
            Project project,
            Guid currentUserId,
            string auditAction,
            string auditDescription,
            CancellationToken cancellationToken)
        {
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

            await _boxActivityService.CopyActivitiesToBox(box, cancellationToken);

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
    }

}
