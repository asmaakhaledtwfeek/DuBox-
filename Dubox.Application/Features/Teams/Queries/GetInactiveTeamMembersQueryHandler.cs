using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Queries;

public class GetInactiveTeamMembersQueryHandler : IRequestHandler<GetInactiveTeamMembersQuery, Result<TeamMembersDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInactiveTeamMembersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TeamMembersDto>> Handle(GetInactiveTeamMembersQuery request, CancellationToken cancellationToken)
    {
        var team = await _unitOfWork.Repository<Team>()
            .GetByIdAsync(request.TeamId, cancellationToken);
        
        if (team == null)
            return Result.Failure<TeamMembersDto>("Team not found.");

        var inactiveTeamMembers = _unitOfWork.Repository<TeamMember>()
            .GetWithSpec(new GetInActiveTeamMembersWithIncludeSpecification(request.TeamId, false)).Data.ToList();
            
        // Manual mapping
        var memberDtos = inactiveTeamMembers.Select(tm => new TeamMemberDto
        {
            TeamMemberId = tm.TeamMemberId,
            UserId = tm.UserId,
            TeamId = tm.TeamId,
            TeamCode = team.TeamCode,
            TeamName = team.TeamName,
            Email = tm.User?.Email ?? string.Empty,
            FullName = tm.User?.FullName ?? tm.EmployeeName ?? string.Empty,
            EmployeeCode = tm.EmployeeCode,
            EmployeeName = tm.EmployeeName,
            MobileNumber = tm.MobileNumber,
            IsActive = tm.IsActive
        }).ToList();

        var dto = new TeamMembersDto
        {
            TeamId = team.TeamId,
            TeamCode = team.TeamCode,
            TeamName = team.TeamName,
            TeamSize = inactiveTeamMembers.Count,
            Members = memberDtos
        };

        return Result.Success(dto);
    }
}

