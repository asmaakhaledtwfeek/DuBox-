using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, Result<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTeamByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TeamDto>> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = _unitOfWork.Repository<Team>()
            .GetEntityWithSpec(new GetTeamWithIncludesSpecification(request.TeamId));

        if (team == null)
            return Result.Failure<TeamDto>("Team not found");

        return Result.Success(team.Adapt<TeamDto>());
    }
}

