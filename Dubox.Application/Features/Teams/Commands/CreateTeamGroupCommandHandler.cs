using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class CreateTeamGroupCommandHandler : IRequestHandler<CreateTeamGroupCommand, Result<TeamGroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public CreateTeamGroupCommandHandler(
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

    public async Task<Result<TeamGroupDto>> Handle(CreateTeamGroupCommand request, CancellationToken cancellationToken)
    {
        // Authorization: Only SystemAdmin and ProjectManager can create team groups
        var canCreate = await _visibilityService.CanCreateProjectOrTeamAsync(cancellationToken);
        if (!canCreate)
        {
            return Result.Failure<TeamGroupDto>("Access denied. Only System Administrators and Project Managers can create team groups.");
        }

        // Verify team exists
        var teamExists = await _unitOfWork.Repository<Team>()
            .IsExistAsync(t => t.TeamId == request.TeamId, cancellationToken);

        if (!teamExists)
            return Result.Failure<TeamGroupDto>("Team not found");

        // Check if group tag already exists for this team
        var groupTagExists = await _unitOfWork.Repository<TeamGroup>()
            .IsExistAsync(tg => tg.TeamId == request.TeamId && tg.GroupTag == request.GroupTag, cancellationToken);

        if (groupTagExists)
            return Result.Failure<TeamGroupDto>("A group with this tag already exists for this team");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var teamGroup = new TeamGroup
        {
            TeamId = request.TeamId,
            GroupTag = request.GroupTag,
            GroupType = request.GroupType,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId
        };

        await _unitOfWork.Repository<TeamGroup>().AddAsync(teamGroup, cancellationToken);
        
        // Create audit log
        var auditLog = new AuditLog
        {
            TableName = nameof(TeamGroup),
            RecordId = teamGroup.TeamGroupId,
            Action = "Creation",
            OldValues = "N/A (New Entity)",
            NewValues = $"TeamId: {teamGroup.TeamId}, GroupType: {teamGroup.GroupType}, GroupTag: {teamGroup.GroupTag}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"New Team Group (Type: {teamGroup.GroupType}) created for team {teamGroup.TeamId}."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
        
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        var createdTeamGroup = _unitOfWork.Repository<TeamGroup>()
              .GetWithSpec(new GetTeamGroupWithIncludesSpecification(teamGroup.TeamGroupId)).Data.First();
        var response = _mapper.Map<TeamGroupDto>(createdTeamGroup);
        return Result.Success(response);
    }
}

