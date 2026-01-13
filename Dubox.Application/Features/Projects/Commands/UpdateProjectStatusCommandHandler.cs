using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class UpdateProjectStatusCommandHandler : IRequestHandler<UpdateProjectStatusCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProjectStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ProjectDto>> Handle(UpdateProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var projectRepository = _unitOfWork.Repository<Project>();
        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
        {
            return Result.Failure<ProjectDto>("Project not found.");
        }

        var oldStatus = project.Status;
        var newStatus = request.Status;

        if (oldStatus == newStatus)
        {
            return Result.Success(project.Adapt<ProjectDto>(), "Status unchanged.");
        }

        
        if (newStatus == ProjectStatusEnum.Archived)
        {
            var progress = project.ProgressPercentage;
            
            // Check if status allows archiving
            var statusAllowsArchiving = oldStatus == ProjectStatusEnum.Completed || 
                                       oldStatus == ProjectStatusEnum.Closed || 
                                       oldStatus == ProjectStatusEnum.OnHold;
            
            if (!statusAllowsArchiving)
            {
                return Result.Failure<ProjectDto>(
                    $"Cannot archive project. Project status must be Completed, Closed, or OnHold. Current status is {oldStatus}.");
            }
            
            if (progress < 100)
            {
                return Result.Failure<ProjectDto>(
                    $"Cannot archive project. Project must have 100% progress. Current progress is {progress}%.");
            }
            
            // Check if all boxes are dispatched
            var boxes = await _unitOfWork.Repository<Box>()
                .FindAsync(b => b.ProjectId == project.ProjectId && b.IsActive, cancellationToken);
            
            var allBoxesDispatched = boxes.All(b => b.Status == BoxStatusEnum.Dispatched);
            
            if (!allBoxesDispatched)
            {
                var nonDispatchedCount = boxes.Count(b => b.Status != BoxStatusEnum.Dispatched);
                return Result.Failure<ProjectDto>(
                    $"Cannot archive project. All boxes must be dispatched before archiving. {nonDispatchedCount} box(es) are not yet dispatched.");
            }
        }

        // Validate OnHold project status transitions
        if (oldStatus == ProjectStatusEnum.OnHold)
        {
            var progress = project.ProgressPercentage;
            
            // Closed is always allowed from OnHold, regardless of progress
            if (newStatus == ProjectStatusEnum.Closed)
            {
                // Allow Closed transition - no validation needed
            }
            else if (progress >= 100)
            {
                // If progress >= 100%, allow transition to Completed or Archived
                if (newStatus != ProjectStatusEnum.Completed && newStatus != ProjectStatusEnum.Archived)
                {
                    return Result.Failure<ProjectDto>(
                        $"Cannot change status from OnHold to {newStatus}. Projects on hold with 100% progress can only be changed to Completed, Archived, or Closed.");
                }
            }
            else
            {
                // If progress < 100%, allow transition to Active
                if (newStatus != ProjectStatusEnum.Active)
                {
                    return Result.Failure<ProjectDto>(
                        $"Cannot change status from OnHold to {newStatus}. Projects on hold with less than 100% progress can only be changed to Active or Closed.");
                }
            }
        }

        // Validate Completed project status transitions
        if (oldStatus == ProjectStatusEnum.Completed)
        {
            // Completed projects can transition to OnHold, Closed, or Archived
            if (newStatus == ProjectStatusEnum.Archived)
            {
                // For Archived, check if all boxes are dispatched
                var boxes = await _unitOfWork.Repository<Box>()
                    .FindAsync(b => b.ProjectId == project.ProjectId && b.IsActive, cancellationToken);
                
                var allBoxesDispatched = boxes.All(b => b.Status == BoxStatusEnum.Dispatched);
                
                if (!allBoxesDispatched)
                {
                    var nonDispatchedCount = boxes.Count(b => b.Status != BoxStatusEnum.Dispatched);
                    return Result.Failure<ProjectDto>(
                        $"Cannot archive project. All boxes must be dispatched before archiving. {nonDispatchedCount} box(es) are not yet dispatched.");
                }
            }
            else if (newStatus != ProjectStatusEnum.OnHold && newStatus != ProjectStatusEnum.Closed)
            {
                return Result.Failure<ProjectDto>(
                    $"Cannot change status from Completed to {newStatus}. Completed projects can only be changed to OnHold, Closed, or Archived (if all boxes are dispatched).");
            }
        }

        // Validate Closed project status transitions
        if (oldStatus == ProjectStatusEnum.Closed)
        {
            var progress = project.ProgressPercentage;
            
            if (progress >= 100)
            {
                // If progress >= 100%, check if all boxes are completed or dispatched
                var boxes = await _unitOfWork.Repository<Box>()
                    .FindAsync(b => b.ProjectId == project.ProjectId && b.IsActive, cancellationToken);
                
                var allBoxesCompletedOrDispatched = boxes.All(b => 
                    b.Status == BoxStatusEnum.Completed || b.Status == BoxStatusEnum.Dispatched);
                
                if (allBoxesCompletedOrDispatched)
                {
                    // Allow transition to Completed
                    if (newStatus != ProjectStatusEnum.Completed)
                    {
                        return Result.Failure<ProjectDto>(
                            $"Cannot change status from Closed to {newStatus}. Closed projects with 100% progress and all boxes completed or dispatched can only be changed to Completed.");
                    }
                }
                else
                {
                    return Result.Failure<ProjectDto>(
                        $"Cannot change status from Closed to {newStatus}. All boxes in the project must be completed or dispatched before changing to Completed.");
                }
            }
            else
            {
                // If progress < 100%, allow transition to OnHold or Active
                if (newStatus != ProjectStatusEnum.OnHold && newStatus != ProjectStatusEnum.Active)
                {
                    return Result.Failure<ProjectDto>(
                        $"Cannot change status from Closed to {newStatus}. Closed projects with less than 100% progress can only be changed to OnHold or Active.");
                }
            }
        }

        project.Status = newStatus;
        project.ModifiedDate = DateTime.UtcNow;
        if(project.Status == ProjectStatusEnum.Archived)
            project.ArchivedDated = DateTime.UtcNow;

        projectRepository.Update(project);

        var changedBy = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var auditLog = new AuditLog
        {
            TableName = nameof(Project),
            RecordId = project.ProjectId,
            Action = "StatusUpdate",
            OldValues = $"Status: {oldStatus}",
            NewValues = $"Status: {newStatus}",
            ChangedBy = changedBy,
            ChangedDate = DateTime.UtcNow,
            Description = $"Project status changed from {oldStatus} to {newStatus}."
        };

        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectDto>());
    }
}
