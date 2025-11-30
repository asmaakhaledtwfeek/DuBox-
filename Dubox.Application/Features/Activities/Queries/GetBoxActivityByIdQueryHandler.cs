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

        // Calculate actual duration in days: (actual end date - actual start date) + 1
        // If same calendar day, return 1 day
        int? actualDuration = null;
        if (boxActivity.ActualStartDate.HasValue && boxActivity.ActualEndDate.HasValue)
        {
            if (boxActivity.ActualStartDate.Value.Date == boxActivity.ActualEndDate.Value.Date)
            {
                actualDuration = 1;
            }
            else
            {
                var timeSpan = boxActivity.ActualEndDate.Value.Date - boxActivity.ActualStartDate.Value.Date;
                var days = (int)Math.Ceiling(timeSpan.TotalDays) + 1;
                // Ensure at least 1 day
                actualDuration = days >= 1 ? days : 1;
            }
        }

        var dto = boxActivity.Adapt<BoxActivityDto>();
        var activityDto = dto with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityCode = boxActivity.ActivityMaster.ActivityCode,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            Stage = boxActivity.ActivityMaster.Stage,
            IsWIRCheckpoint = boxActivity.ActivityMaster.IsWIRCheckpoint,
            WIRCode = boxActivity.ActivityMaster.WIRCode,
            ActualDuration = actualDuration
        };

        return Result.Success(activityDto);
    }
}

