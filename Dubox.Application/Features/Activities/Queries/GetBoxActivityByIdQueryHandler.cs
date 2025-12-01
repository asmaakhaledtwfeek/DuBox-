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
            .Include(ba => ba.Team)
            .Include(ba => ba.AssignedMember)
                .ThenInclude(ba => ba.User)
            .FirstOrDefaultAsync(ba => ba.BoxActivityId == request.BoxActivityId, cancellationToken);

        if (boxActivity == null)
            return Result.Failure<BoxActivityDto>("Box Activity not found");

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
            ActualDuration = actualDuration,
            TeamId = boxActivity.TeamId,
            TeamName = boxActivity.Team?.TeamName,
            AssignedMemberId = boxActivity.AssignedMemberId,
            AssignedMemberName = boxActivity.AssignedMember != null ?
               (!string.IsNullOrWhiteSpace(boxActivity.AssignedMember.EmployeeName)
        ? boxActivity.AssignedMember.EmployeeName
        : boxActivity.AssignedMember.User?.FullName) : null
        };

        return Result.Success(activityDto);
    }
}

