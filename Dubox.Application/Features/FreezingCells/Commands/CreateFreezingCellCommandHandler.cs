using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.FreezingCells.Commands;

public class CreateFreezingCellCommandHandler : IRequestHandler<CreateFreezingCellCommand, Result<FreezingCellDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateFreezingCellCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FreezingCellDto>> Handle(CreateFreezingCellCommand request, CancellationToken cancellationToken)
    {
        var partExists = await _unitOfWork.Repository<FactorySectionPart>()
            .IsExistAsync(p => p.PartId == request.FactorySectionPartId, cancellationToken);

        if (!partExists)
            return Result.Failure<FreezingCellDto>("Factory section part not found.");

        var alreadyFrozen = await _unitOfWork.Repository<FreezingCell>()
            .IsExistAsync(f => f.FactorySectionPartId == request.FactorySectionPartId
                            && f.RowNumber == request.RowNumber, cancellationToken);

        if (alreadyFrozen)
            return Result.Failure<FreezingCellDto>("This row is already frozen for the specified part.");

        var freezingCell = new FreezingCell
        {
            FactorySectionPartId = request.FactorySectionPartId,
            RowNumber = request.RowNumber
        };

        await _unitOfWork.Repository<FreezingCell>().AddAsync(freezingCell, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(freezingCell.Adapt<FreezingCellDto>());
    }
}
