using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Queries;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<List<ProjectDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        // Get accessible project IDs based on user role
        var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

        var projects = _unitOfWork.Repository<Project>()
            .GetWithSpec(new GetProjectsSpecification(request, accessibleProjectIds)).Data.ToList();

        var projectDtos = projects.Adapt<List<ProjectDto>>();

        return Result.Success(projectDtos);
    }
}

