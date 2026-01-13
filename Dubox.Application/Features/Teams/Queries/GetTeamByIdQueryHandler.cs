using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, Result<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetTeamByIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<TeamDto>> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = _unitOfWork.Repository<Team>()
            .GetEntityWithSpec(new GetTeamWithIncludesSpecification(request.TeamId));

        if (team == null)
            return Result.Failure<TeamDto>("Team not found");

        // Check if user has access to this team
        var canAccess = await _visibilityService.CanAccessTeamAsync(request.TeamId, cancellationToken);
        if (!canAccess)
        {
            return Result.Failure<TeamDto>("Access denied. You do not have permission to view this team.");
        }

        return Result.Success(team.Adapt<TeamDto>());
    }
}

