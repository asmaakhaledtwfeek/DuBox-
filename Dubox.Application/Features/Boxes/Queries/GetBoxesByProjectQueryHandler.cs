using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxesByProjectQueryHandler : IRequestHandler<GetBoxesByProjectQuery, Result<List<BoxDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetBoxesByProjectQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetBoxesByProjectQuery request, CancellationToken cancellationToken)
    {
        // Verify user has access to the requested project
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
        if (!canAccessProject)
        {
            return Result.Failure<List<BoxDto>>("Access denied. You do not have permission to view boxes for this project.");
        }

        var boxes = _unitOfWork.Repository<Box>().GetWithSpec(new GetBoxesByProjectIdSpecification(request.ProjectId)).Data.ToList();

        var boxDtos = boxes.Select(b =>
        {
            var dto = b.Adapt<BoxDto>();
            return dto with { ProjectCode = b.Project.ProjectCode };
        }).ToList();

        return Result.Success(boxDtos);
    }
}

