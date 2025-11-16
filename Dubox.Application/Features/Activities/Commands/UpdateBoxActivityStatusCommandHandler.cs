using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands;

public class UpdateBoxActivityStatusCommandHandler : IRequestHandler<UpdateBoxActivityStatusCommand, Result<BoxActivityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;

    public UpdateBoxActivityStatusCommandHandler(IUnitOfWork unitOfWork, IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<BoxActivityDto>> Handle(UpdateBoxActivityStatusCommand request, CancellationToken cancellationToken)
    {
        var boxActivity = _unitOfWork.Repository<BoxActivity>().GetEntityWithSpec(new GetBoxActivityByIdSpecification(request.BoxActivityId));


        if (boxActivity == null)
            return Result.Failure<BoxActivityDto>("Box activity not found");

        var wasCompleted = boxActivity.Status == BoxStatusEnum.Completed;

        boxActivity.Status = request.Status;
        boxActivity.WorkDescription = request.WorkDescription;
        boxActivity.IssuesEncountered = request.IssuesEncountered;

        // Update dates based on status
        if (request.Status == BoxStatusEnum.InProgress && boxActivity.ActualStartDate == null)
            boxActivity.ActualStartDate = DateTime.UtcNow;

        if (request.Status == BoxStatusEnum.Completed && !wasCompleted)
        {
            boxActivity.ActualEndDate = DateTime.UtcNow;
            boxActivity.ProgressPercentage = 100;
        }

        boxActivity.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.Repository<BoxActivity>().Update(boxActivity);
        await _unitOfWork.CompleteAsync(cancellationToken);


        var dto = boxActivity.Adapt<BoxActivityDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityCode = boxActivity.ActivityMaster.ActivityCode,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            Stage = boxActivity.ActivityMaster.Stage,
            Status = boxActivity.Status.ToString(),
        };

        return Result.Success(dto);
    }

}

