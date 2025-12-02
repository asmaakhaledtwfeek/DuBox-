using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Activities.Queries;

public class GetBoxActivityByIdQueryHandler : IRequestHandler<GetBoxActivityByIdQuery, Result<BoxActivityDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBoxActivityByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BoxActivityDto>> Handle(GetBoxActivityByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetBoxActivityByIdSpecification(request.BoxActivityId);
        var boxActivity = _unitOfWork.Repository<Domain.Entities.BoxActivity>().GetEntityWithSpec(specification);

        if (boxActivity == null)
            return Result.Failure<BoxActivityDto>("Box Activity not found");

        var dto = boxActivity.Adapt<BoxActivityDto>();

        return Result.Success(dto);
    }
}

