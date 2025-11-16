using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Activities.Queries;

public class GetBoxActivityByIdQueryHandler : IRequestHandler<GetBoxActivityByIdQuery, Result<BoxActivityDto>>
{
    private readonly IDbContext _dbContext;

    public GetBoxActivityByIdQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<BoxActivityDto>> Handle(GetBoxActivityByIdQuery request, CancellationToken cancellationToken)
    {
        var boxActivity = await _dbContext.BoxActivities
            .Include(ba => ba.ActivityMaster)
            .Include(ba => ba.Box)
                .ThenInclude(b => b.Project)
            .FirstOrDefaultAsync(ba => ba.BoxActivityId == request.BoxActivityId, cancellationToken);

        if (boxActivity == null)
            return Result.Failure<BoxActivityDto>("Box Activity not found");

        var dto = boxActivity.Adapt<BoxActivityDto>();
        var activityDto = dto with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityCode = boxActivity.ActivityMaster.ActivityCode,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            Stage = boxActivity.ActivityMaster.Stage,
            IsWIRCheckpoint = boxActivity.ActivityMaster.IsWIRCheckpoint,
            WIRCode = boxActivity.ActivityMaster.WIRCode
        };

        return Result.Success(activityDto);
    }
}

