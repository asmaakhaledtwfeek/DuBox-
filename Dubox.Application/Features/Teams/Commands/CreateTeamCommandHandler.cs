using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
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

        var team = request.Adapt<Team>();
        team.IsActive = true;
        team.CreatedDate = DateTime.UtcNow;

        await _unitOfWork.Repository<Team>().AddAsync(team, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        var department = await _unitOfWork.Repository<Department>()
             .GetByIdAsync(request.DepartmentId, cancellationToken);

        return Result.Success(team.Adapt<TeamDto>());
    }
}

