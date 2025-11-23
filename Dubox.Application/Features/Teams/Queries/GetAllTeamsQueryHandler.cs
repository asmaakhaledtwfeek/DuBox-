using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Queries;

public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, Result<List<TeamDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTeamsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<TeamDto>>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        // Use specification to include Department, TeamLeader, and Members navigation properties
        var teamsQuery = _unitOfWork.Repository<Team>()
            .GetWithSpec(new GetTeamWithIncludesSpecification())
            .Data;

        var teams = await teamsQuery.ToListAsync(cancellationToken);

        return Result.Success(teams.Adapt<List<TeamDto>>());
    }
}

