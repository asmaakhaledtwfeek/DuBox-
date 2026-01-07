using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Queries;

public class GetAllActiveMembersQueryHandler : IRequestHandler<GetAllActiveMembersQuery, Result<List<TeamMemberDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetAllActiveMembersQueryHandler(
        IUnitOfWork unitOfWork,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<TeamMemberDto>>> Handle(GetAllActiveMembersQuery request, CancellationToken cancellationToken)
    {
        // Get accessible team IDs for the current user
        var accessibleTeamIds = await _visibilityService.GetAccessibleTeamIdsAsync(cancellationToken);
        
        // Build query for active members
        var query = _unitOfWork.Repository<TeamMember>()
            .GetWithSpec(new GetAllTeamMembersSpecification()).Data;
        
        // Filter by accessible team IDs if user has restricted access
        // null means user can access all teams (SystemAdmin/Viewer)
        if (accessibleTeamIds != null)
        {
            query = query.Where(tm => accessibleTeamIds.Contains(tm.TeamId));
        }
        
        var activeMembers = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
            
        var memberDtos = activeMembers.Select(tm => new TeamMemberDto
        {
            TeamMemberId = tm.TeamMemberId,
            UserId = tm.UserId,
            TeamId = tm.TeamId,
            TeamCode = tm.Team?.TeamCode ?? string.Empty,
            TeamName = tm.Team?.TeamName ?? string.Empty,
            Email = tm.User?.Email ?? string.Empty,
            FullName = tm.User?.FullName ?? tm.EmployeeName ?? string.Empty,
            EmployeeCode = tm.EmployeeCode,
            EmployeeName = tm.EmployeeName,
            MobileNumber = tm.MobileNumber,
            IsActive = tm.IsActive
        }).ToList();

        return Result.Success(memberDtos);
    }
}

