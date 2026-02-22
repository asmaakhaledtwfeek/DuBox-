using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FreezingCells.Commands;

public class DeleteFreezingCellCommandHandler : IRequestHandler<DeleteFreezingCellCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFreezingCellCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteFreezingCellCommand request, CancellationToken cancellationToken)
    {
        var freezingCell = await _unitOfWork.Repository<FreezingCell>()
            .GetByIdAsync(request.FreezingCellId, cancellationToken);

        if (freezingCell is null)
            return Result.Failure<bool>("Freezing cell not found.");

        _unitOfWork.Repository<FreezingCell>().Delete(freezingCell);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }
}
