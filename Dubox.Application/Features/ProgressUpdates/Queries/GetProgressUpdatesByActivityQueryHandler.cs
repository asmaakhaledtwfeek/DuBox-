using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.ProgressUpdates.Queries;

public class GetProgressUpdatesByActivityQueryHandler : IRequestHandler<GetProgressUpdatesByActivityQuery, Result<List<ProgressUpdateDto>>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public GetProgressUpdatesByActivityQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ProgressUpdateDto>>> Handle(GetProgressUpdatesByActivityQuery request, CancellationToken cancellationToken)
    {
        var updates = _unitOfWork.Repository<ProgressUpdate>().GetWithSpec(new GetProgressUpdatesByActivitySpecification(request.BoxActivityId)).Data.ToList();

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

