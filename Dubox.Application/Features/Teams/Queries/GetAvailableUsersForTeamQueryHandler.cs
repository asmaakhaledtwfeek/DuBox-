using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Queries;

public class GetAvailableUsersForTeamQueryHandler : IRequestHandler<GetAvailableUsersForTeamQuery, Result<List<UserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAvailableUsersForTeamQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<UserDto>>> Handle(GetAvailableUsersForTeamQuery request, CancellationToken cancellationToken)
    {
        var team = await _unitOfWork.Repository<Team>()
            .GetByIdAsync(request.TeamId, cancellationToken);

        if (team == null)
            return Result.Failure<List<UserDto>>("Team not found.");

        var unActiveTeamMemberUserIds =  _unitOfWork.Repository<TeamMember>()
            .Get()
            .Where(tm => tm.TeamId == request.TeamId && tm.UserId.HasValue && !tm.IsActive)
            .Select(tm => tm.UserId!.Value)
            .ToList();

        var availableUsers = _unitOfWork.Repository<User>()
            .GetWithSpec(new GetUsersByDepartmentWithIncludesSpecification(team.DepartmentId, unActiveTeamMemberUserIds))
            .Data.ToList();
            

        // Map to UserDto
        var userDtos = availableUsers.Adapt<List<UserDto>>();

        return Result.Success(userDtos);
    }
}

