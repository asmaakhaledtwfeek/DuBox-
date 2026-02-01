using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class AssignTeamLeaderCommandHandler : IRequestHandler<AssignTeamLeaderCommand, Result<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public AssignTeamLeaderCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<TeamDto>> Handle(AssignTeamLeaderCommand request, CancellationToken cancellationToken)
    {
        var module=PermissionModuleEnum.Teams;
        var action =PermissionActionEnum.ManageMembers;

        var canManage = await _visibilityService.CanPerformAsync(module,action,cancellationToken);
        if (!canManage)
            return Result.Failure<TeamDto>("Access denied. Only System Administrators and Project Managers can assign crew leaders.");

        var team = _unitOfWork.Repository<Team>()
            .GetEntityWithSpec(new GetTeamWithIncludesSpecification(request.TeamId));

        if (team == null)
            return Result.Failure<TeamDto>("Crew  not found");

        var teamMember = await _unitOfWork.Repository<TeamMember>()
            .GetByIdAsync(request.TeamMemberId, cancellationToken);

        if (teamMember == null)
            return Result.Failure<TeamDto>("Crew member not found");

        if (teamMember.TeamId != team.TeamId)
            return Result.Failure<TeamDto>("The selected crew member does not belong to this crew");

        if (!teamMember.IsActive)
            return Result.Failure<TeamDto>("Cannot assign an inactive crew member as crew leader");

        var oldLeaderId = team.TeamLeaderMemberId;

        team.TeamLeaderMemberId = request.TeamMemberId;

        _unitOfWork.Repository<Team>().Update(team);
        teamMember.TeamId = request.TeamId;
        _unitOfWork.Repository<TeamMember>().Update(teamMember);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var auditLog = new AuditLog
        {
            TableName = nameof(Team),
            RecordId = team.TeamId,
            Action = "Update",
            OldValues = $"Crew LeaderId: {oldLeaderId?.ToString() ?? "null"}",
            NewValues = $"Crew LeaderId: {team.TeamLeaderMemberId}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Crew leader {teamMember.EmployeeName} assigned to Crew {team.TeamCode} ({team.TeamName})"
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        var response = _mapper.Map<TeamDto>(team);
        return Result.Success(response);
    }
}

