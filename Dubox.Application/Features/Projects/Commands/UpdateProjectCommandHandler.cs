using Dubox.Application.DTOs;
using Dubox.Application.Features.MaterialTemplates.Commands;
using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Dubox.Domain.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Projects.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private readonly IDbContext _context;
    private readonly IBoxActivityService _boxActivityService;
    private readonly IMediator _mediator;

    public UpdateProjectCommandHandler(
        IUnitOfWork unitOfWork, 
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService,
        IDbContext context,
        IBoxActivityService boxActivityService,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
        _context = context;
        _boxActivityService = boxActivityService;
        _mediator = mediator;
    }

    public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var module= PermissionModuleEnum.Projects;
        var action = PermissionActionEnum.Edit;
        var canModify = await _visibilityService.CanPerformAsync(module, action,cancellationToken);
        if (!canModify)
            return Result.Failure<ProjectDto>("Access denied. You do not have permission to update projects.");
         
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectDto>("Project not found.");

        // Verify user has access to the project
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
        if (!canAccessProject)
            return Result.Failure<ProjectDto>("Access denied. You do not have permission to update this project.");
        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(request.ProjectId, "edit project", cancellationToken);

        if (!projectStatusValidation.IsSuccess)
            return Result.Failure<ProjectDto>(projectStatusValidation.Error!);
       
        // Capture old activity template before applying updates
        var oldActivityTemplateId = project.ActivityTemplateId;

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

        // Propagate activity template change to non-customised box types and their eligible boxes
        var newActivityTemplateId = project.ActivityTemplateId;
        if (request.ActivityTemplateId.HasValue && newActivityTemplateId != oldActivityTemplateId)
        {
            await PropagateActivityTemplateChangeAsync(
                project.ProjectId,
                oldActivityTemplateId,
                newActivityTemplateId!.Value,
                cancellationToken);
        }

        if (request.MaterialTemplateId.HasValue)
        {
            var currentAssignment = await _context.ProjectMaterialTemplates
                .FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

            var oldTemplateId = currentAssignment?.MaterialTemplateId;

            if (oldTemplateId != request.MaterialTemplateId.Value)
            {
                await _mediator.Send(
                    new AssignTemplateToProjectCommand(
                        request.ProjectId,
                        request.MaterialTemplateId.Value,
                        oldTemplateId),
                    cancellationToken);
            }
        }


        return Result.Success(project.Adapt<ProjectDto>());
    }

    /// <summary>
    /// Propagates an activity template change at project level down to non-customised box types
    /// and their NotStarted / ReadyToStart boxes.
    ///
    /// A box type is considered "non-customised" when its ActivityTemplateId is null (inheriting
    /// from the project) OR was explicitly set to the old project template.
    /// Custom box types (ActivityTemplateId differs from the old project template) are skipped.
    /// </summary>
    private async Task PropagateActivityTemplateChangeAsync(
        Guid projectId,
        Guid? oldActivityTemplateId,
        Guid newActivityTemplateId,
        CancellationToken cancellationToken)
    {
        var boxTypes = await _context.ProjectBoxTypes
            .Where(bt => bt.ProjectId == projectId && bt.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var boxType in boxTypes)
        {
            bool isNonCustomised =
                boxType.ActivityTemplateId == null ||
                boxType.ActivityTemplateId == oldActivityTemplateId;

            if (!isNonCustomised)
                continue;

            // Update box type to new template (if it was explicitly set to the old one)
            if (boxType.ActivityTemplateId == oldActivityTemplateId)
            {
                boxType.ActivityTemplateId = newActivityTemplateId;
                _unitOfWork.Repository<ProjectBoxType>().Update(boxType);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }
            // Box types with null ActivityTemplateId inherit the project template automatically;
            // no entity update needed, but their existing boxes still need to be updated.

            // Propagate to eligible boxes
            var boxes = await _context.Boxes
                .Where(b => b.ProjectBoxTypeId == boxType.Id &&
                            (b.Status == BoxStatusEnum.NotStarted || b.Status == BoxStatusEnum.ReadyToStart))
                .ToListAsync(cancellationToken);

            foreach (var box in boxes)
            {
                await _boxActivityService.ResetBoxActivitiesFromTemplateAsync(box, newActivityTemplateId, cancellationToken);
            }
        }
    }

    private void ApplyProjectUpdates(Project project, UpdateProjectCommand request)
    {
        if (!string.IsNullOrEmpty(request.ProjectName))
            project.ProjectName = TextTransformHelper.ToTitleCase(request.ProjectName);
        if (!string.IsNullOrEmpty(request.ClientName))
            project.ClientName = TextTransformHelper.ToTitleCase(request.ClientName);
        if (request.PlannedStartDate.HasValue)
            project.PlannedStartDate = request.PlannedStartDate;
        
        // Handle ProjectedEndDate and Duration calculation
        if (request.ProjectedEndDate.HasValue)
        {
            project.ProjectedEndDate = request.ProjectedEndDate;
            // If ProjectedEndDate is provided, recalculate Duration
            if (project.PlannedStartDate.HasValue)
            {
                var duration = (request.ProjectedEndDate.Value - project.PlannedStartDate.Value).Days;
                project.Duration = duration > 0 ? duration : 1;
            }
        }
        else if (request.Duration.HasValue)
        {
            project.Duration = request.Duration.Value;
            // If Duration is provided, recalculate ProjectedEndDate
            if (project.PlannedStartDate.HasValue)
            {
                project.ProjectedEndDate = project.PlannedStartDate.Value.AddDays(request.Duration.Value);
            }
        }
        
        if (request.ProjectMangerId.HasValue)
            project.ProjectMangerId = request.ProjectMangerId;
        if (request.ProjectValue.HasValue)
            project.ProjectValue = request.ProjectValue;
        if (!string.IsNullOrEmpty(request.Description))
            project.Description = request.Description;
        if (request.BimLink != null)
            project.BimLink = string.IsNullOrWhiteSpace(request.BimLink) ? null : request.BimLink;
        if (request.IsActive.HasValue)
            project.IsActive = request.IsActive.Value;
        
        // Handle AllowCompletionWithConditionalApproval
        if (request.AllowCompletionWithConditionalApproval.HasValue)
            project.AllowCompletionWithConditionalApproval = request.AllowCompletionWithConditionalApproval.Value;
        
        // Handle ActivityTemplateId - allow setting or clearing the template
        if (request.ActivityTemplateId.HasValue)
            project.ActivityTemplateId = request.ActivityTemplateId.Value;
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

