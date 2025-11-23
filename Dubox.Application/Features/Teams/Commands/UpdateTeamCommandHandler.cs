using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Result<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTeamCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TeamDto>> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = _unitOfWork.Repository<Team>()
            .GetEntityWithSpec(new GetTeamWithIncludesSpecification(request.TeamId));

        if (team == null)
            return Result.Failure<TeamDto>("Team not found");

        // Check if team code already exists (if being changed)
        if (!string.IsNullOrEmpty(request.TeamCode) && team.TeamCode != request.TeamCode)
        {
            var codeExists = await _unitOfWork.Repository<Team>()
                .IsExistAsync(t => t.TeamCode == request.TeamCode && t.TeamId != request.TeamId, cancellationToken);

            if (codeExists)
                return Result.Failure<TeamDto>("Team with this code already exists");
        }

        // Check if department is being changed and team has members
        if (request.DepartmentId.HasValue && request.DepartmentId.Value != team.DepartmentId)
        {
            var hasMembers = team.Members != null && team.Members.Any();
            if (hasMembers)
            {
                return Result.Failure<TeamDto>("Cannot change department when team has members. Please remove all members before changing the department.");
            }
        }

        // Update team properties
        if (!string.IsNullOrEmpty(request.TeamCode))
            team.TeamCode = request.TeamCode;

        if (!string.IsNullOrEmpty(request.TeamName))
            team.TeamName = request.TeamName;

        if (request.DepartmentId.HasValue)
            team.DepartmentId = request.DepartmentId.Value;

        if (request.Trade != null)
            team.Trade = request.Trade;

        if (request.IsActive.HasValue)
            team.IsActive = request.IsActive.Value;

        _unitOfWork.Repository<Team>().Update(team);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Reload team with includes to get updated navigation properties
        var updatedTeam = _unitOfWork.Repository<Team>()
            .GetEntityWithSpec(new GetTeamWithIncludesSpecification(team.TeamId));

        var response = _mapper.Map<TeamDto>(updatedTeam);
        return Result.Success(response);
    }
}

