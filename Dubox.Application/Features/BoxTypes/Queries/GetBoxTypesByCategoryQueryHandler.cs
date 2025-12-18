using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var boxTypes =  _unitOfWork.Repository<BoxType>().Get()
            .Where(bt => bt.CategoryId == request.CategoryId)
            .Select(bt => new BoxTypeDto
            {
                BoxTypeId = bt.BoxTypeId,
                BoxTypeName = bt.BoxTypeName,
                Abbreviation = bt.Abbreviation,
                CategoryId = bt.CategoryId,
                HasSubTypes = bt.BoxSubTypes.Any()
            })
            .OrderBy(bt => bt.BoxTypeName)
            .ToList();

        return Result<List<BoxTypeDto>>.Success(boxTypes);
    }
}

