using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectExists = await _unitOfWork.Repository<Project>()
            .IsExistAsync(p => p.ProjectCode == request.ProjectCode, cancellationToken);

        if (projectExists)
            return Result.Failure<ProjectDto>("Project with this code already exists");

        var project = new Project
        {
            ProjectCode = request.ProjectCode,
            ProjectName = request.ProjectName,
            ClientName = request.ClientName,
            Location = request.Location,
            StartDate = request.StartDate,
            PlannedEndDate = request.PlannedEndDate,
            Description = request.Description,
            Status = ProjectStatusEnum.Active,
            IsActive = true,
            TotalBoxes = 0,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Project>().AddAsync(project, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectDto>());
    }
}

