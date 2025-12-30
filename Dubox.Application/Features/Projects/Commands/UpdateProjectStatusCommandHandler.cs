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

        // Validate transition to Archived
        // Project can only be archived if:
        // 1. Current status is Completed, Closed, or OnHold
        // 2. AND progress is 100%
        if (newStatus == ProjectStatusEnum.Archived)
        {
            var progress = project.ProgressPercentage;
            var canBeArchived = (oldStatus == ProjectStatusEnum.Completed || 
                                oldStatus == ProjectStatusEnum.Closed || 
                                oldStatus == ProjectStatusEnum.OnHold) && 
                               progress >= 100;

            if (!canBeArchived)
            {
                var statusMessage = oldStatus == ProjectStatusEnum.Completed || 
                                   oldStatus == ProjectStatusEnum.Closed || 
                                   oldStatus == ProjectStatusEnum.OnHold
                    ? $"Cannot archive project. Project must have 100% progress. Current progress is {progress}%."
                    : $"Cannot archive project. Project status must be Completed, Closed, or OnHold. Current status is {oldStatus}.";
                
                return Result.Failure<ProjectDto>(statusMessage);
            }
        }

        // Validate OnHold project status transitions
        if (oldStatus == ProjectStatusEnum.OnHold)
        {
            var progress = project.ProgressPercentage;
            
            if (progress >= 100)
            {
                // If progress >= 100%, only allow transition to Completed or Archived
                if (newStatus != ProjectStatusEnum.Completed && newStatus != ProjectStatusEnum.Archived)
                {
                    return Result.Failure<ProjectDto>(
                        $"Cannot change status from OnHold to {newStatus}. Projects on hold with 100% progress can only be changed to Completed or Archived.");
                }
            }
            else
            {
                // If progress < 100%, only allow transition to Active
                if (newStatus != ProjectStatusEnum.Active)
                {
                    return Result.Failure<ProjectDto>(
                        $"Cannot change status from OnHold to {newStatus}. Projects on hold with less than 100% progress can only be changed to Active.");
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
