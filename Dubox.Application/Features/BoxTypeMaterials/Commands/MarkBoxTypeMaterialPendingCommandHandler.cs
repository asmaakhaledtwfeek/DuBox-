using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public class MarkBoxTypeMaterialPendingCommandHandler 
    : IRequestHandler<MarkBoxTypeMaterialPendingCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkBoxTypeMaterialPendingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(
        MarkBoxTypeMaterialPendingCommand request, 
        CancellationToken cancellationToken)
    {
        // Get the box type material
        var boxTypeMaterial = await _unitOfWork.Repository<BoxTypeMaterial>()
            .GetByIdAsync(request.BoxTypeMaterialId, cancellationToken);

        if (boxTypeMaterial == null)
            return Result.Failure<bool>("Box type material not found.");

        if (!boxTypeMaterial.IsArrived)
            return Result.Failure<bool>("Material is already marked as pending.");

        // Once delivered (100%), material cannot be reverted to pending
        return Result.Failure<bool>("Once delivered (100%), material cannot be reverted to pending.");
    }
}






