using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public class GetTeamGroupMembersQueryHandler : IRequestHandler<GetTeamGroupMembersQuery, Result<TeamGroupMembersDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTeamGroupMembersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TeamGroupMembersDto>> Handle(GetTeamGroupMembersQuery request, CancellationToken cancellationToken)
    {
        var teamGroup = _unitOfWork.Repository<TeamGroup>()
            .GetEntityWithSpec(new GetTeamGroupWithIncludesSpecification(request.TeamGroupId));

        if (teamGroup == null)
            return Result.Failure<TeamGroupMembersDto>("Team group not found");

        // Get all team members that belong to this team group
        var teamMembers = await _unitOfWork.Repository<TeamMember>()
            .FindAsync(tm => tm.TeamGroupId == request.TeamGroupId && tm.IsActive, cancellationToken);

        var teamMembersList = teamMembers.ToList();

        // Create the DTO
        var dto = new TeamGroupMembersDto
        {
            TeamGroupId = teamGroup.TeamGroupId,
            TeamId = teamGroup.TeamId,
            TeamName = teamGroup.Team.TeamName,
            TeamCode = teamGroup.Team.TeamCode,
            GroupTag = teamGroup.GroupTag,
            GroupType = teamGroup.GroupType,
            GroupLeaderId = teamGroup.GroupLeaderId,
            GroupLeaderName = teamGroup.GroupLeader?.User?.FullName,
            MemberCount = teamMembersList.Count,
            IsActive = teamGroup.IsActive,
            Members = teamMembersList.Adapt<List<TeamMemberDto>>()
        };

        return Result.Success(dto);
    }
}

