using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public UpdateProjectCommandHandler(
        IUnitOfWork unitOfWork, 
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        // Check if user can modify data (Viewer role cannot)
        var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
        if (!canModify)
        {
            return Result.Failure<ProjectDto>("Access denied. Viewer role has read-only access and cannot update projects.");
        }

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectDto>("Project not found.");

        // Verify user has access to the project
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
        if (!canAccessProject)
        {
            return Result.Failure<ProjectDto>("Access denied. You do not have permission to update this project.");
        }

        if (!string.IsNullOrEmpty(request.ProjectCode) && project.ProjectCode != request.ProjectCode)
        {
            var codeExists = await _unitOfWork.Repository<Project>()
                .IsExistAsync(p => p.ProjectCode == request.ProjectCode && p.ProjectId != request.ProjectId, cancellationToken);

            if (codeExists)
                return Result.Failure<ProjectDto>("Project with this code already exists.");
        }

        var oldProjectState = new
        {
            project.ProjectCode,
            project.ProjectName,
            project.ClientName,
            project.Location,
            project.PlannedStartDate,
            project.Duration,
            project.Description,
            project.IsActive
        };

        ApplyProjectUpdates(project, request);

        if (project.PlannedStartDate.HasValue && project.Duration.HasValue && project.Duration > 0)
            project.PlannedEndDate = project.PlannedStartDate.Value.AddDays(project.Duration.Value);

        project.ModifiedDate = DateTime.UtcNow;
        project.ModifiedBy = currentUserId.ToString();

        var (oldValues, newValues, description) = GetProjectChanges(oldProjectState, project);

        _unitOfWork.Repository<Project>().Update(project);

        if (!string.IsNullOrEmpty(oldValues))
        {
            var projectLog = new AuditLog
            {
                TableName = nameof(Project),
                RecordId = project.ProjectId,
                Action = "Update",
                OldValues = oldValues,
                NewValues = newValues,
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Project '{project.ProjectName}' updated: {description}"
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(projectLog, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectDto>());
    }

    private void ApplyProjectUpdates(Project project, UpdateProjectCommand request)
    {
        if (!string.IsNullOrEmpty(request.ProjectCode))
            project.ProjectCode = request.ProjectCode;
        if (!string.IsNullOrEmpty(request.ProjectName))
            project.ProjectName = request.ProjectName;
        if (!string.IsNullOrEmpty(request.ClientName))
            project.ClientName = request.ClientName;
        if (request.Location.HasValue)
            project.Location = request.Location.Value;
        if (request.CategoryId.HasValue)
            project.CategoryId = request.CategoryId.Value;
        if (request.PlannedStartDate.HasValue)
            project.PlannedStartDate = request.PlannedStartDate;
        if (request.Duration.HasValue)
            project.Duration = request.Duration.Value;
        if (!string.IsNullOrEmpty(request.Description))
            project.Description = request.Description;
        if (request.BimLink != null)
            project.BimLink = string.IsNullOrWhiteSpace(request.BimLink) ? null : request.BimLink;
        if (request.IsActive.HasValue)
            project.IsActive = request.IsActive.Value;
    }

    private (string oldValues, string newValues, string description) GetProjectChanges(
        dynamic oldState, Project newState)
    {
        var changes = new List<(string Property, object Old, object New)>();

        Action<string, object?, object?> checkChange = (prop, oldVal, newVal) =>
        {
            if (oldVal?.ToString() != newVal?.ToString())
            {
                changes.Add((prop, oldVal ?? "null", newVal ?? "null"));
            }
        };

        checkChange(nameof(newState.ProjectCode), oldState.ProjectCode, newState.ProjectCode);
        checkChange(nameof(newState.ProjectName), oldState.ProjectName, newState.ProjectName);
        checkChange(nameof(newState.ClientName), oldState.ClientName, newState.ClientName);
        checkChange(nameof(newState.Location), oldState.Location, newState.Location);

        checkChange(nameof(newState.PlannedStartDate), oldState.PlannedStartDate?.ToString("yyyy-MM-dd"), newState.PlannedStartDate?.ToString("yyyy-MM-dd"));
        checkChange(nameof(newState.Duration), oldState.Duration, newState.Duration);
        checkChange(nameof(newState.IsActive), oldState.IsActive, newState.IsActive);

        var changedProperties = string.Join(", ", changes.Select(c => c.Property));

        return (
            oldValues: string.Join(" | ", changes.Select(c => $"{c.Property}: {c.Old}")),
            newValues: string.Join(" | ", changes.Select(c => $"{c.Property}: {c.New}")),
            description: changedProperties.Any() ? $"Changed: {changedProperties}" : "No significant changes."
        );
    }
}

