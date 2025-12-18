using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypes.Queries;

public class GetBoxSubTypesByBoxTypeQueryHandler : IRequestHandler<GetBoxSubTypesByBoxTypeQuery, Result<List<BoxSubTypeDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBoxSubTypesByBoxTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<BoxSubTypeDto>>> Handle(GetBoxSubTypesByBoxTypeQuery request, CancellationToken cancellationToken)
    {
        var subTypes =  _unitOfWork.Repository<BoxSubType>().Get()
            .Where(st => st.BoxTypeId == request.BoxTypeId)
            .Select(st => new BoxSubTypeDto
            {
                BoxSubTypeId = st.BoxSubTypeId,
                BoxSubTypeName = st.BoxSubTypeName,
                Abbreviation = st.Abbreviation,
                BoxTypeId = st.BoxTypeId
            })
            .OrderBy(st => st.BoxSubTypeName)
            .ToList();

        return Result<List<BoxSubTypeDto>>.Success(subTypes);
    }
}

