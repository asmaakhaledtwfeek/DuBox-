using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Activities.Queries;

public class GetBoxActivityByIdQueryHandler : IRequestHandler<GetBoxActivityByIdQuery, Result<BoxActivityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetBoxActivityByIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<BoxActivityDto>> Handle(GetBoxActivityByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetBoxActivityByIdSpecification(request.BoxActivityId);
        var boxActivity = _unitOfWork.Repository<Domain.Entities.BoxActivity>().GetEntityWithSpec(specification);

        if (boxActivity == null)
            return Result.Failure<BoxActivityDto>("Box Activity not found");

        // Verify user has access to the project this box activity belongs to
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(boxActivity.Box.ProjectId, cancellationToken);
        if (!canAccessProject)
        {
            return Result.Failure<BoxActivityDto>("Access denied. You do not have permission to view this box activity.");
        }

        var dto = boxActivity.Adapt<BoxActivityDto>();

        return Result.Success(dto);
    }
}

