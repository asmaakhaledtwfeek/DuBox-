using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class AddMembersToGroupCommandHandler : IRequestHandler<AddMembersToGroupCommand, Result<TeamGroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public AddMembersToGroupCommandHandler(
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

    public async Task<Result<TeamGroupDto>> Handle(AddMembersToGroupCommand request, CancellationToken cancellationToken)
    {
        var teamGroup = _unitOfWork.Repository<TeamGroup>()
            .GetEntityWithSpec(new GetTeamGroupWithIncludesSpecification(request.TeamGroupId));

        if (teamGroup == null)
            return Result.Failure<TeamGroupDto>("Team group not found");

        if (!teamGroup.IsActive)
            return Result.Failure<TeamGroupDto>("Cannot modify members of an inactive team group");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var canManageByRole = await _visibilityService.CanCreateProjectOrTeamAsync(cancellationToken);
        var isGroupLeader = teamGroup.GroupLeaderId.HasValue && teamGroup.GroupLeaderId.Value == currentUserId;

        if (!canManageByRole && !isGroupLeader)
        {
            return Result.Failure<TeamGroupDto>("Access denied. Only System Administrators, Project Managers, or the Group Leader can manage members of this group.");
        }

        if (request.TeamMemberIds == null)
            return Result.Failure<TeamGroupDto>("Team member IDs list is required");

        // Get current members of the group
        var currentMembers = await _unitOfWork.Repository<TeamMember>()
            .FindAsync(tm => tm.TeamGroupId == request.TeamGroupId, cancellationToken);
        var currentMemberIds = currentMembers.Select(m => m.TeamMemberId).ToList();

        var requestedMemberIds = request.TeamMemberIds.ToList();

        // Determine which members to add and remove
        var membersToAdd = requestedMemberIds.Except(currentMemberIds).ToList();
        var membersToRemove = currentMemberIds.Except(requestedMemberIds).ToList();
        var membersToKeep = requestedMemberIds.Intersect(currentMemberIds).ToList();

        var addedMembers = new List<Guid>();
        var removedMembers = new List<Guid>();
        var invalidMembers = new List<string>();

        // Process members to add
        foreach (var teamMemberId in membersToAdd)
        {
            var teamMember = await _unitOfWork.Repository<TeamMember>()
                .GetByIdAsync(teamMemberId, cancellationToken);

            if (teamMember == null)
            {
                invalidMembers.Add($"Team member {teamMemberId} not found");
                continue;
            }

            if (teamMember.TeamId != teamGroup.TeamId)
            {
                invalidMembers.Add($"{teamMember.EmployeeName} does not belong to team {teamGroup.Team.TeamName}");
                continue;
            }

            if (!teamMember.IsActive)
            {
                invalidMembers.Add($"{teamMember.EmployeeName} is not an active team member");
                continue;
            }

            if (teamMember.TeamGroupId.HasValue && teamMember.TeamGroupId.Value != request.TeamGroupId)
            {
                invalidMembers.Add($"{teamMember.EmployeeName} is already assigned to another group");
                continue;
            }

            teamMember.TeamGroupId = request.TeamGroupId;
            _unitOfWork.Repository<TeamMember>().Update(teamMember);
            addedMembers.Add(teamMemberId);
        }

        foreach (var teamMemberId in membersToRemove)
        {
            var teamMember = await _unitOfWork.Repository<TeamMember>()
                .GetByIdAsync(teamMemberId, cancellationToken);

            if (teamMember != null)
            {
                // Check if the member being removed is the group leader
                if (teamGroup.GroupLeaderId.HasValue && teamGroup.GroupLeaderId.Value == teamMemberId)
                {
                    invalidMembers.Add($"{teamMember.EmployeeName} is the group leader and cannot be removed. Please assign a new leader first.");
                    continue;
                }

                teamMember.TeamGroupId = null;
                _unitOfWork.Repository<TeamMember>().Update(teamMember);
                removedMembers.Add(teamMemberId);
            }
        }

        // Check if any changes were made
        if (!addedMembers.Any() && !removedMembers.Any())
        {
            var message = "No changes were made to group membership.";
            if (invalidMembers.Any())
                message += $" Issues: {string.Join("; ", invalidMembers)}.";

            if (membersToKeep.Any())
                message = $"Group already has the selected members. {message}";

            return Result.Failure<TeamGroupDto>(message);
        }

        // Create audit log
        var oldValues = $"Member Count: {currentMemberIds.Count}";
        var newValues = $"Member Count: {requestedMemberIds.Count}";
        if (addedMembers.Any())
            newValues += $", Added: {string.Join(", ", addedMembers)}";
        if (removedMembers.Any())
            newValues += $", Removed: {string.Join(", ", removedMembers)}";

        var auditLog = new AuditLog
        {
            TableName = nameof(TeamGroup),
            RecordId = teamGroup.TeamGroupId,
            Action = "Update",
            OldValues = oldValues,
            NewValues = newValues,
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Group {teamGroup.GroupTag} membership updated: {addedMembers.Count} added, {removedMembers.Count} removed"
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Retrieve updated team group with all includes
        var updatedTeamGroup = _unitOfWork.Repository<TeamGroup>()
            .GetEntityWithSpec(new GetTeamGroupWithIncludesSpecification(teamGroup.TeamGroupId));

        var response = _mapper.Map<TeamGroupDto>(updatedTeamGroup);

        // Build result message
        var resultMessage = "Group membership updated successfully.";
        if (addedMembers.Any())
            resultMessage += $" Added {addedMembers.Count} member(s).";
        if (removedMembers.Any())
            resultMessage += $" Removed {removedMembers.Count} member(s).";
        if (invalidMembers.Any())
            resultMessage += $" Warning: {invalidMembers.Count} member(s) could not be processed.";

        return Result.Success(response, resultMessage);
    }
}

