using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Activities.Queries;

public class GetBoxActivitiesByBoxQueryHandler : IRequestHandler<GetBoxActivitiesByBoxQuery, Result<List<BoxActivityDto>>>
{
    private readonly IDbContext _dbContext;

    public GetBoxActivitiesByBoxQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<BoxActivityDto>>> Handle(GetBoxActivitiesByBoxQuery request, CancellationToken cancellationToken)
    {
        var boxActivities = await _dbContext.BoxActivities
            .Include(ba => ba.ActivityMaster)
            .Include(ba => ba.Box)
            .Include(ba => ba.Team)
            .Include(ba => ba.AssignedMember)
                .ThenInclude(tm => tm.User)
            .Where(ba => ba.BoxId == request.BoxId)
            .OrderBy(ba => ba.Sequence)
            .ToListAsync(cancellationToken);

        var boxActivityDtos = boxActivities.Select(ba =>
        {
            var dto = ba.Adapt<BoxActivityDto>();
            return dto with
            {
                BoxTag = ba.Box.BoxTag,
                ActivityCode = ba.ActivityMaster.ActivityCode,
                ActivityName = ba.ActivityMaster.ActivityName,
                Stage = ba.ActivityMaster.Stage,
                IsWIRCheckpoint = ba.ActivityMaster.IsWIRCheckpoint,
                WIRCode = ba.ActivityMaster.WIRCode,
                TeamName = ba.Team?.TeamName,
                AssignedMemberName = ba.AssignedMember != null 
                    ? (!string.IsNullOrEmpty(ba.AssignedMember.EmployeeName) 
                        ? ba.AssignedMember.EmployeeName 
                        : ba.AssignedMember.User?.FullName ?? string.Empty)
                    : null
            };
        }).ToList();

        return Result.Success(boxActivityDtos);
    }
}

