using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectDto>("Project not found.");

        if (!string.IsNullOrEmpty(request.ProjectCode))
        {
            var codeExists = await _unitOfWork.Repository<Project>()
                .IsExistAsync(p => p.ProjectCode == request.ProjectCode && p.ProjectId != request.ProjectId, cancellationToken);

            if (codeExists)
                return Result.Failure<ProjectDto>("Project with this code already exists.");

            project.ProjectCode = request.ProjectCode;
        }

        ApplyProjectUpdates(project, request);

        project.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.Repository<Project>().Update(project);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectDto>());
    }

    private void ApplyProjectUpdates(Project project, UpdateProjectCommand request)
    {
        if (!string.IsNullOrEmpty(request.ProjectName))
            project.ProjectName = request.ProjectName;

        if (!string.IsNullOrEmpty(request.ClientName))
            project.ClientName = request.ClientName;

        if (!string.IsNullOrEmpty(request.Location))
            project.Location = request.Location;

        if (!string.IsNullOrEmpty(request.Description))
            project.Description = request.Description;

        if (request.StartDate.HasValue)
            project.StartDate = request.StartDate.Value;

        if (request.PlannedEndDate.HasValue)
            project.PlannedEndDate = request.PlannedEndDate.Value;

        if (request.ActualEndDate.HasValue)
            project.ActualEndDate = request.ActualEndDate.Value;

        if (request.Status.HasValue)
            project.Status = (ProjectStatusEnum)request.Status.Value;

        if (request.IsActive.HasValue)
            project.IsActive = request.IsActive.Value;
    }
}

