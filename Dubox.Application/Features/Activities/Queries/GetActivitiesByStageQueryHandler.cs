using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Activities.Queries;

public class GetActivitiesByStageQueryHandler : IRequestHandler<GetActivitiesByStageQuery, Result<List<ActivityMasterDto>>>
{
    private readonly IDbContext _dbContext;

    public GetActivitiesByStageQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<ActivityMasterDto>>> Handle(GetActivitiesByStageQuery request, CancellationToken cancellationToken)
    {
        var activities = await _dbContext.ActivityMasters
            .Where(a => a.StageNumber == request.StageNumber && a.IsActive)
            .OrderBy(a => a.SequenceInStage)
            .ToListAsync(cancellationToken);

        return Result.Success(activities.Adapt<List<ActivityMasterDto>>());
    }
}

