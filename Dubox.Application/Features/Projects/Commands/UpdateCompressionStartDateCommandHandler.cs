using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class UpdateCompressionStartDateCommandHandler : IRequestHandler<UpdateCompressionStartDateCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public UpdateCompressionStartDateCommandHandler(
        IUnitOfWork unitOfWork, 
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<ProjectDto>> Handle(UpdateCompressionStartDateCommand request, CancellationToken cancellationToken)
    {
        var projectRepository = _unitOfWork.Repository<Project>();
        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
        {
            return Result.Failure<ProjectDto>("Project not found.");
        }

        // Check if project is archived - cannot set compression start date for archived projects
        var isArchived = await _visibilityService.IsProjectArchivedAsync(request.ProjectId, cancellationToken);
        if (isArchived)
        {
            return Result.Failure<ProjectDto>("Cannot set compression start date. Archived projects are read-only and cannot be modified.");
        }

        // Check if project is on hold - cannot set compression start date for projects on hold
        var isOnHold = await _visibilityService.IsProjectOnHoldAsync(request.ProjectId, cancellationToken);
        if (isOnHold)
        {
            return Result.Failure<ProjectDto>("Cannot set compression start date. Projects on hold cannot be modified. Only project status changes are allowed.");
        }

        // Check if project is closed - cannot set compression start date for closed projects
        var isClosed = await _visibilityService.IsProjectClosedAsync(request.ProjectId, cancellationToken);
        if (isClosed)
        {
            return Result.Failure<ProjectDto>("Cannot set compression start date. Closed projects cannot be modified. Only project status changes are allowed.");
        }

        var oldCompressionStartDate = project.CompressionStartDate;
        project.CompressionStartDate = request.CompressionStartDate;
        project.ModifiedDate = DateTime.UtcNow;
        projectRepository.Update(project);

        var changedBy = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var auditLog = new AuditLog
        {
            TableName = nameof(Project),
            RecordId = project.ProjectId,
            Action = "CompressionStartDateUpdate",
            OldValues = oldCompressionStartDate.HasValue 
                ? $"CompressionStartDate: {oldCompressionStartDate.Value:yyyy-MM-dd}" 
                : "CompressionStartDate: null",
            NewValues = request.CompressionStartDate.HasValue 
                ? $"CompressionStartDate: {request.CompressionStartDate.Value:yyyy-MM-dd}" 
                : "CompressionStartDate: null",
            ChangedBy = changedBy,
            ChangedDate = DateTime.UtcNow,
            Description = request.CompressionStartDate.HasValue
                ? $"Project compression start date set to {request.CompressionStartDate.Value:yyyy-MM-dd}."
                : "Project compression start date cleared."
        };

        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectDto>());
    }
}

