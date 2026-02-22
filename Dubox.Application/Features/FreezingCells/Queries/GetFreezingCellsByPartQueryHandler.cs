using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.FreezingCells.Queries;

public class GetFreezingCellsByPartQueryHandler : IRequestHandler<GetFreezingCellsByPartQuery, Result<IEnumerable<FreezingCellDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFreezingCellsByPartQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<FreezingCellDto>>> Handle(GetFreezingCellsByPartQuery request, CancellationToken cancellationToken)
    {
        var freezingCells = await _unitOfWork.Repository<FreezingCell>()
            .FindAsync(f => f.FactorySectionPartId == request.FactorySectionPartId, cancellationToken);

        var dtos = freezingCells
            .OrderBy(f => f.RowNumber)
            .Select(f => f.Adapt<FreezingCellDto>())
            .AsEnumerable();

        return Result.Success(dtos);
    }
}
