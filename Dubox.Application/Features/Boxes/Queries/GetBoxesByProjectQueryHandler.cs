using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxesByProjectQueryHandler : IRequestHandler<GetBoxesByProjectQuery, Result<List<BoxDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBoxesByProjectQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetBoxesByProjectQuery request, CancellationToken cancellationToken)
    {
        var boxes = _unitOfWork.Repository<Box>().GetWithSpec(new GetBoxesByProjectIdSpecification(request.ProjectId)).Data.ToList();

        var boxDtos = boxes.Select(b =>
        {
            var dto = b.Adapt<BoxDto>();
            return dto with { ProjectCode = b.Project.ProjectCode };
        }).ToList();

        return Result.Success(boxDtos);
    }
}

