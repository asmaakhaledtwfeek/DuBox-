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

    public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
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

        return Result.Success(project.Adapt<ProjectDto>());
    }
}

