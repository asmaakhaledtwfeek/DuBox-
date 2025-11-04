using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ProgressUpdates.Queries;

public class GetProgressUpdatesByBoxQueryHandler : IRequestHandler<GetProgressUpdatesByBoxQuery, Result<List<ProgressUpdateDto>>>
{
    private readonly IDbContext _dbContext;

    public GetProgressUpdatesByBoxQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<ProgressUpdateDto>>> Handle(GetProgressUpdatesByBoxQuery request, CancellationToken cancellationToken)
    {
        var updates = await _dbContext.ProgressUpdates
            .Include(pu => pu.Box)
            .Include(pu => pu.BoxActivity)
                .ThenInclude(ba => ba.ActivityMaster)
            .Include(pu => pu.UpdatedByUser)
            .Where(pu => pu.BoxId == request.BoxId)
            .OrderByDescending(pu => pu.UpdateDate)
            .ToListAsync(cancellationToken);

        var updateDtos = updates.Select(u =>
        {
            var dto = u.Adapt<ProgressUpdateDto>();
            return dto with
            {
                BoxTag = u.Box.BoxTag,
                ActivityName = u.BoxActivity.ActivityMaster.ActivityName,
                UpdatedByName = u.UpdatedByUser.FullName ?? u.UpdatedByUser.Email
            };
        }).ToList();

        return Result.Success(updateDtos);
    }
}

