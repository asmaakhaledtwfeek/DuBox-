using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class UpdateTeamGroupCommandHandler : IRequestHandler<UpdateTeamGroupCommand, Result<TeamGroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public UpdateTeamGroupCommandHandler(
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

    public async Task<Result<TeamGroupDto>> Handle(UpdateTeamGroupCommand request, CancellationToken cancellationToken)
    {
        var canUpdate = await _visibilityService.CanCreateProjectOrTeamAsync(cancellationToken);
        if (!canUpdate)
            return Result.Failure<TeamGroupDto>("Access denied. Only System Administrators and Project Managers can update team groups.");

        var teamGroup = _unitOfWork.Repository<TeamGroup>()
            .GetEntityWithSpec(new GetTeamGroupWithIncludesSpecification(request.TeamGroupId));

        if (teamGroup == null)
            return Result.Failure<TeamGroupDto>("Team group not found");

        if (teamGroup.GroupTag != request.GroupTag)
        {
            var groupTagExists = await _unitOfWork.Repository<TeamGroup>()
                .IsExistAsync(tg => tg.TeamId == teamGroup.TeamId &&
                                   tg.GroupTag == request.GroupTag &&
                                   tg.TeamGroupId != request.TeamGroupId,
                              cancellationToken);

            if (groupTagExists)
                return Result.Failure<TeamGroupDto>("A group with this tag already exists for this team");
        }

        var oldValues = $"GroupTag: {teamGroup.GroupTag}, GroupType: {teamGroup.GroupType}, IsActive: {teamGroup.IsActive}";

        teamGroup.GroupTag = request.GroupTag;
        teamGroup.GroupType = request.GroupType;
        teamGroup.IsActive = request.IsActive;

        _unitOfWork.Repository<TeamGroup>().Update(teamGroup);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var newValues = $"GroupTag: {teamGroup.GroupTag}, GroupType: {teamGroup.GroupType}, IsActive: {teamGroup.IsActive}";

        var auditLog = new AuditLog
        {
            TableName = nameof(TeamGroup),
            RecordId = teamGroup.TeamGroupId,
            Action = "Update",
            OldValues = oldValues,
            NewValues = newValues,
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Team Group {teamGroup.GroupTag} ({teamGroup.TeamGroupId}) updated"
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        var updatedTeamGroup = _unitOfWork.Repository<TeamGroup>()
            .GetEntityWithSpec(new GetTeamGroupWithIncludesSpecification(teamGroup.TeamGroupId));

        var response = _mapper.Map<TeamGroupDto>(updatedTeamGroup);
        return Result.Success(response);
    }
}

