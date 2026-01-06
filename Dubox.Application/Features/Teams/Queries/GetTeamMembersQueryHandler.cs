using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Queries
{
    public class GetTeamMembersQueryHandler : IRequestHandler<GetTeamMembersQuery, Result<TeamMembersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTeamMembersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<TeamMembersDto>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Repository<Team>()
          .GetByIdAsync(request.TeamId, cancellationToken);
            if (team == null)
                return Result.Failure<TeamMembersDto>("This Team not found.");

            var teamMembersQuery = _unitOfWork.Repository<TeamMember>()
                      .GetWithSpec(new TeamMembersByUserIdsSpecification(request.TeamId,false));
            
            var teamMembers = await teamMembersQuery.Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            // Manual mapping
            var memberDtos = teamMembers.Select(tm => new TeamMemberDto
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
                MobileNumber = tm.MobileNumber
            }).ToList();

            var dto = new TeamMembersDto
            {
                TeamId = team.TeamId,
                TeamCode = team.TeamCode,
                TeamName = team.TeamName,
                TeamSize = teamMembers.Count(m => m.IsActive),
                Members = memberDtos
            };
            
            return Result.Success(dto);
        }
    }
}
