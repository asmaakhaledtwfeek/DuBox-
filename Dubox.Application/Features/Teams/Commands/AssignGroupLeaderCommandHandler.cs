using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class AssignGroupLeaderCommandHandler : IRequestHandler<AssignGroupLeaderCommand, Result<TeamGroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public AssignGroupLeaderCommandHandler(
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

    public async Task<Result<TeamGroupDto>> Handle(AssignGroupLeaderCommand request, CancellationToken cancellationToken)
    {
        var canManage = await _visibilityService.CanCreateProjectOrTeamAsync(cancellationToken);
        if (!canManage)
            return Result.Failure<TeamGroupDto>("Access denied. Only System Administrators and Project Managers can assign group leaders.");

        var teamGroup = _unitOfWork.Repository<TeamGroup>()
            .GetEntityWithSpec(new GetTeamGroupWithIncludesSpecification(request.TeamGroupId));

        if (teamGroup == null)
            return Result.Failure<TeamGroupDto>("Team group not found");

        var teamMember = await _unitOfWork.Repository<TeamMember>()
            .GetByIdAsync(request.TeamMemberId, cancellationToken);

        if (teamMember == null)
            return Result.Failure<TeamGroupDto>("Team member not found");

        if (teamMember.TeamId != teamGroup.TeamId)
            return Result.Failure<TeamGroupDto>("The selected team member does not belong to this team");

        if (!teamMember.IsActive)
            return Result.Failure<TeamGroupDto>("Cannot assign an inactive team member as group leader");

        var oldLeaderId = teamGroup.GroupLeaderId;

        teamGroup.GroupLeaderId = request.TeamMemberId;

        _unitOfWork.Repository<TeamGroup>().Update(teamGroup);
        teamMember.TeamGroupId = request.TeamGroupId;
        _unitOfWork.Repository<TeamMember>().Update(teamMember);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var auditLog = new AuditLog
        {
            TableName = nameof(TeamGroup),
            RecordId = teamGroup.TeamGroupId,
            Action = "Update",
            OldValues = $"GroupLeaderId: {oldLeaderId?.ToString() ?? "null"}",
            NewValues = $"GroupLeaderId: {teamGroup.GroupLeaderId}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Group leader assigned to team group {teamGroup.GroupTag} ({teamGroup.TeamGroupId})"
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        var updatedTeamGroup = _unitOfWork.Repository<TeamGroup>()
            .GetEntityWithSpec(new GetTeamGroupWithIncludesSpecification(teamGroup.TeamGroupId));

        var response = _mapper.Map<TeamGroupDto>(updatedTeamGroup);
        return Result.Success(response);
    }
}

