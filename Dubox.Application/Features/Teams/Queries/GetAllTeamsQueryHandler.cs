using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, Result<List<TeamDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTeamsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<TeamDto>>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        var teams = await _unitOfWork.Repository<Team>()
            .GetAllAsync(cancellationToken);

        return Result.Success(teams.Adapt<List<TeamDto>>());
    }
}

