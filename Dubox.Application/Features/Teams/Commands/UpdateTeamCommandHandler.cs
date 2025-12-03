using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Result<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTeamCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TeamDto>> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = _unitOfWork.Repository<Team>()
            .GetEntityWithSpec(new GetTeamWithIncludesSpecification(request.TeamId));

        if (team == null)
            return Result.Failure<TeamDto>("Team not found");

        // Store old values for audit log
        var oldValues = new List<string>();
        var newValues = new List<string>();

        // Check if team code already exists (if being changed)
        if (!string.IsNullOrEmpty(request.TeamCode) && team.TeamCode != request.TeamCode)
        {
            var codeExists = await _unitOfWork.Repository<Team>()
                .IsExistAsync(t => t.TeamCode == request.TeamCode && t.TeamId != request.TeamId, cancellationToken);

            if (codeExists)
                return Result.Failure<TeamDto>("Team with this code already exists");

            oldValues.Add($"TeamCode: {team.TeamCode}");
            newValues.Add($"TeamCode: {request.TeamCode}");
        }

        // Check if department is being changed and team has members
        if (request.DepartmentId.HasValue && request.DepartmentId.Value != team.DepartmentId)
        {
            var hasMembers = team.Members != null && team.Members.Any();
            if (hasMembers)
            {
                return Result.Failure<TeamDto>("Cannot change department when team has members. Please remove all members before changing the department.");
            }

            oldValues.Add($"DepartmentId: {team.DepartmentId}");
            newValues.Add($"DepartmentId: {request.DepartmentId.Value}");
        }

        // Update team properties
        if (!string.IsNullOrEmpty(request.TeamCode) && team.TeamCode != request.TeamCode)
        {
            team.TeamCode = request.TeamCode;
        }

        if (!string.IsNullOrEmpty(request.TeamName) && team.TeamName != request.TeamName)
        {
            oldValues.Add($"TeamName: {team.TeamName}");
            newValues.Add($"TeamName: {request.TeamName}");
            team.TeamName = request.TeamName;
        }

        if (request.DepartmentId.HasValue && request.DepartmentId.Value != team.DepartmentId)
        {
            team.DepartmentId = request.DepartmentId.Value;
        }

        if (request.Trade != null && team.Trade != request.Trade)
        {
            oldValues.Add($"Trade: {team.Trade ?? "N/A"}");
            newValues.Add($"Trade: {request.Trade}");
            team.Trade = request.Trade;
        }

        if (request.IsActive.HasValue && team.IsActive != request.IsActive.Value)
        {
            oldValues.Add($"IsActive: {team.IsActive}");
            newValues.Add($"IsActive: {request.IsActive.Value}");
            team.IsActive = request.IsActive.Value;
        }

        _unitOfWork.Repository<Team>().Update(team);

        // Create audit log if there are changes
        if (oldValues.Any())
        {
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            var auditLog = new AuditLog
            {
                TableName = nameof(Team),
                RecordId = team.TeamId,
                Action = "Update",
                OldValues = string.Join(", ", oldValues),
                NewValues = string.Join(", ", newValues),
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Team '{team.TeamCode} - {team.TeamName}' updated. ({oldValues.Count} properties changed)."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Reload team with includes to get updated navigation properties
        var updatedTeam = _unitOfWork.Repository<Team>()
            .GetEntityWithSpec(new GetTeamWithIncludesSpecification(team.TeamId));

        var response = _mapper.Map<TeamDto>(updatedTeam);
        return Result.Success(response);
    }
}

