using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTeamCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TeamDto>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var teamExists = await _unitOfWork.Repository<Team>()
            .IsExistAsync(t => t.TeamCode == request.TeamCode, cancellationToken);

        if (teamExists)
            return Result.Failure<TeamDto>("Team with this code already exists");

        var team = new Team
        {
            TeamCode = request.TeamCode,
            TeamName = request.TeamName,
            Department = request.Department,
            Trade = request.Trade,
            TeamLeaderName = request.TeamLeaderName,
            TeamSize = request.TeamSize,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Team>().AddAsync(team, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(team.Adapt<TeamDto>());
    }
}

