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
            return Result.Failure<ProjectDto>("Project not found");

        // Check if project code changed and is unique
        if (project.ProjectCode != request.ProjectCode)
        {
            var codeExists = await _unitOfWork.Repository<Project>()
                .IsExistAsync(p => p.ProjectCode == request.ProjectCode && p.ProjectId != request.ProjectId, cancellationToken);

            if (codeExists)
                return Result.Failure<ProjectDto>("Project code already exists");
        }

        project.ProjectCode = request.ProjectCode;
        project.ProjectName = request.ProjectName;
        project.ClientName = request.ClientName;
        project.Location = request.Location;
        project.StartDate = request.StartDate;
        project.PlannedEndDate = request.PlannedEndDate;
        project.ActualEndDate = request.ActualEndDate;
        project.Status = (ProjectStatusEnum)request.Status;
        project.Description = request.Description;
        project.IsActive = request.IsActive;
        project.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.Repository<Project>().Update(project);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectDto>());
    }
}

