using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Commands;

public class ReactivateTeamMemberCommandHandler : IRequestHandler<ReactivateTeamMemberCommand, Result<TeamMemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ReactivateTeamMemberCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TeamMemberDto>> Handle(ReactivateTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var team = await _unitOfWork.Repository<Team>()
            .GetByIdAsync(request.TeamId, cancellationToken);

        if (team == null)
            return Result.Failure<TeamMemberDto>("Team not found.");

        if (!team.IsActive)
            return Result.Failure<TeamMemberDto>("Cannot reactivate members in an inactive team.");

        var teamMember =  _unitOfWork.Repository<TeamMember>()
            .FindAsync(tm => tm.TeamMemberId == request.TeamMemberId, cancellationToken).Result.FirstOrDefault();

        if (teamMember == null)
            return Result.Failure<TeamMemberDto>("Team member not found.");

        if (teamMember.TeamId != request.TeamId)
            return Result.Failure<TeamMemberDto>("Team member does not belong to this team.");

        if (teamMember.IsActive)
            return Result.Failure<TeamMemberDto>("Team member is already active.");

        if (teamMember.UserId.HasValue && teamMember.User != null)
        {
            if (!teamMember.User.IsActive)
            {
                return Result.Failure<TeamMemberDto>("Cannot reactivate team member. The associated user is inactive.");
            }
        }

        teamMember.IsActive = true;
        _unitOfWork.Repository<TeamMember>().Update(teamMember);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var auditLog = new AuditLog
        {
            TableName = nameof(TeamMember),
            RecordId = teamMember.TeamMemberId,
            Action = "Reactivation",
            OldValues = $"IsActive: false, TeamId: {team.TeamId}, TeamCode: {team.TeamCode}, TeamName: {team.TeamName}",
            NewValues = $"IsActive: true",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Team member reactivated in team '{team.TeamCode} - {team.TeamName}'."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Map to DTO
        var teamMemberDto = new TeamMemberDto
        {
            TeamMemberId = teamMember.TeamMemberId,
            UserId = teamMember.UserId,
            TeamId = teamMember.TeamId,
            TeamCode = team.TeamCode,
            TeamName = team.TeamName,
            Email = teamMember.User?.Email ?? string.Empty,
            FullName = teamMember.User?.FullName ?? teamMember.EmployeeName ?? string.Empty,
            EmployeeCode = teamMember.EmployeeCode,
            EmployeeName = teamMember.EmployeeName,
            MobileNumber = teamMember.MobileNumber,
            IsActive = teamMember.IsActive
        };

        return Result.Success(teamMemberDto);
    }
}

