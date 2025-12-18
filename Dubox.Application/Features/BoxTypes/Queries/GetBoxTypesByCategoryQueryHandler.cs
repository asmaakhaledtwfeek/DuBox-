using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypes.Queries;

public class GetBoxTypesByCategoryQueryHandler : IRequestHandler<GetBoxTypesByCategoryQuery, Result<List<BoxTypeDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBoxTypesByCategoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<BoxTypeDto>>> Handle(GetBoxTypesByCategoryQuery request, CancellationToken cancellationToken)
    {
        // Use specification to get box types with BoxSubTypes navigation property included
        var specification = new GetBoxTypesByCategorySpecification(request.CategoryId);
        var boxTypesQuery = _unitOfWork.Repository<BoxType>().GetWithSpec(specification);
        var boxTypesList = boxTypesQuery.Data.ToList();

        // Map to DTOs with HasSubTypes properly set
        var boxTypes = boxTypesList.Select(bt => new BoxTypeDto
        {
            BoxTypeId = bt.BoxTypeId,
            BoxTypeName = bt.BoxTypeName,
            Abbreviation = bt.Abbreviation,
            CategoryId = bt.CategoryId,
            HasSubTypes = bt.BoxSubTypes != null && bt.BoxSubTypes.Any()
        }).ToList();

        return Result<List<BoxTypeDto>>.Success(boxTypes);
    }
}

