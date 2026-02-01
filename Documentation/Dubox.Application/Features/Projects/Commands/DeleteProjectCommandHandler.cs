using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public DeleteProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<bool>("Project not found");

        // Check if project is archived - cannot delete archived projects
        if (project.Status == ProjectStatusEnum.Archived)
        {
            return Result.Failure<bool>("Cannot delete project. Archived projects are read-only and cannot be deleted.");
        }

        var projectCode = project.ProjectCode;
        var projectName = project.ProjectName;
        var projectId = project.ProjectId;

        // Check if project has boxes
        var hasBoxes = await _unitOfWork.Repository<Box>()
            .IsExistAsync(b => b.ProjectId == request.ProjectId, cancellationToken);

        // Check if project has progress - if yes, archive instead of delete
        if (project.ProgressPercentage > 0)
        {
            var oldStatus = project.Status;
            project.Status = ProjectStatusEnum.Archived;
            project.ArchivedDated = DateTime.UtcNow;
            _unitOfWork.Repository<Project>().Update(project);

            var archiveLog = new AuditLog
            {
                TableName = nameof(Project),
                RecordId = projectId,
                Action = "Archived",
                OldValues = $"Code: {projectCode}, Name: {projectName}, Status: {oldStatus.ToString()}",
                NewValues = $"Status: {ProjectStatusEnum.Archived}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Project '{projectName}' with code '{projectCode}' was moved to archived due to existing progress ({project.ProgressPercentage}%)."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(archiveLog, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Failure<bool>("Project has progress and cannot be deleted. The project has been moved to archived projects instead.");
        }

        // No progress - proceed with soft delete
        project.IsActive = false;
        project.DeletedDated = DateTime.UtcNow;
        _unitOfWork.Repository<Project>().Update(project);

        var projectLog = new AuditLog
        {
            TableName = nameof(Project),
            RecordId = projectId,
            Action = "Deletion",
            OldValues = $"Code: {projectCode}, Name: {projectName}, Status: {project.Status.ToString()}",
            NewValues = "N/A (Entity Deleted)",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Project '{projectName}' with code '{projectCode}' was deleted."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(projectLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }
}

