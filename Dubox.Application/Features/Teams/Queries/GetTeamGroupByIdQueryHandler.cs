using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public class GetTeamGroupByIdQueryHandler : IRequestHandler<GetTeamGroupByIdQuery, Result<TeamGroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTeamGroupByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TeamGroupDto>> Handle(GetTeamGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var teamGroup = _unitOfWork.Repository<TeamGroup>()
            .GetEntityWithSpec(new GetTeamGroupWithIncludesSpecification(request.TeamGroupId));

        if (teamGroup == null)
            return Result.Failure<TeamGroupDto>("Team group not found");
        var createdBy = await _unitOfWork.Repository<User>().GetByIdAsync(teamGroup.CreatedBy);

        var teamGroupDto = teamGroup.Adapt<TeamGroupDto>() with
        {
            CreatedBy = createdBy != null ? createdBy.FullName : string.Empty,
        };

        return Result.Success(teamGroupDto);
    }
}

