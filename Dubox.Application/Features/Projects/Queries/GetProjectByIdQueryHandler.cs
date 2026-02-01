using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    private readonly IBlobStorageService _blobStorageService;
    private const string _containerName = "images";
    public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService , IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetProjectByIdSpecification(request.ProjectId);
        var project = _unitOfWork.Repository<Project>().GetEntityWithSpec(specification);

        if (project == null)
            return Result.Failure<ProjectDto>("Project not found");

        var canAccess = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
        if (!canAccess)
            return Result.Failure<ProjectDto>("Access denied. You do not have permission to view this project.");

        var projectDto = project.Adapt<ProjectDto>() with
        {
            SubContractorImageUrl = !string.IsNullOrEmpty(project.SubContractorImageUrl) ? _blobStorageService.GetImageUrl(_containerName, project.SubContractorImageUrl)
                   : null,
           ContractorImageUrl = !string.IsNullOrEmpty(project.ContractorImageUrl) ? _blobStorageService.GetImageUrl(_containerName, project.ContractorImageUrl)
                   : null,
            ClientImageUrl = !string.IsNullOrEmpty(project.ClientImageUrl) ? _blobStorageService.GetImageUrl(_containerName, project.ClientImageUrl)
                   : null,
        };
        if (project.ProjectManger != null)
        {
            projectDto = projectDto with 
            {
                ProjectMangerName = project.ProjectManger.FullName ?? project.ProjectManger.Email 
            };
        }

        return Result.Success(projectDto);
    }
}

