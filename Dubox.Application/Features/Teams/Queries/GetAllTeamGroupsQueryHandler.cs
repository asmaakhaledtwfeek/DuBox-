using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Queries;

public class GetAllTeamGroupsQueryHandler : IRequestHandler<GetAllTeamGroupsQuery, Result<List<TeamGroupDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTeamGroupsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<TeamGroupDto>>> Handle(GetAllTeamGroupsQuery request, CancellationToken cancellationToken)
    {
        var teamGroupsResult = _unitOfWork.Repository<TeamGroup>()
            .GetWithSpec(new GetTeamGroupWithIncludesSpecification());

        var teamGroups = await teamGroupsResult.Data.ToListAsync(cancellationToken);

        var teamGroupDtos = teamGroups.Adapt<List<TeamGroupDto>>();

        return Result.Success(teamGroupDtos);
    }
}

