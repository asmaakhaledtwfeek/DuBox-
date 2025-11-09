using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateTeamCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        var createdTeam = _unitOfWork.Repository<Team>()
              .GetWithSpec(new GetTeamWithIncludesSpecification(team.TeamId)).Data.First();
        var response = _mapper.Map<TeamDto>(createdTeam);
        return Result.Success(response);
    }
}

