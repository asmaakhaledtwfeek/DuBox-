using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Activities.Queries;

public class GetBoxActivitiesByBoxQueryHandler : IRequestHandler<GetBoxActivitiesByBoxQuery, Result<List<BoxActivityDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetBoxActivitiesByBoxQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<BoxActivityDto>>> Handle(GetBoxActivitiesByBoxQuery request, CancellationToken cancellationToken)
    {
        // Optimized: Load box only to verify it exists and get ProjectId (minimal data)
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);
        if (box == null)
            return Result.Failure<List<BoxActivityDto>>("Box not found");

        // Verify user has access to the project this box belongs to
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
        if (!canAccessProject)
        {
            return Result.Failure<List<BoxActivityDto>>("Access denied. You do not have permission to view activities for this box.");
        }

        // Optimized: Use specification with single query strategy (removed split query)
        var specification = new GetBoxActivitiesByBoxSpecification(request.BoxId);
        var boxActivitiesResult = _unitOfWork.Repository<BoxActivity>().GetWithSpec(specification);
        
        // Use AsNoTracking for read-only query to improve performance
        // Convert to list asynchronously to avoid blocking
        var boxActivities = await boxActivitiesResult.Data
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var boxActivityDtos = boxActivities.Adapt<List<BoxActivityDto>>();

        return Result.Success(boxActivityDtos);
    }
}

