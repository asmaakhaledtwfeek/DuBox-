using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

        // Optimized: Use AsNoTracking and async for better performance
        var specification = new GetProjectsSpecification(request, accessibleProjectIds);
        var projectsQuery = _unitOfWork.Repository<Project>().GetWithSpec(specification);
        
        var projects = await projectsQuery.Data
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var projectDtos = projects.Adapt<List<ProjectDto>>();

        return Result.Success(projectDtos);
    }
}

