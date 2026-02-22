using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public class MarkBoxTypeMaterialArrivedCommandHandler 
    : IRequestHandler<MarkBoxTypeMaterialArrivedCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDbContext _context;
    private readonly IBoxMaterialDeliveryService _boxMaterialDeliveryService;

    public MarkBoxTypeMaterialArrivedCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IDbContext context,
        IBoxMaterialDeliveryService boxMaterialDeliveryService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _context = context;
        _boxMaterialDeliveryService = boxMaterialDeliveryService;
    }

    public async Task<Result<bool>> Handle(
    MarkBoxTypeMaterialArrivedCommand request,
    CancellationToken cancellationToken)
    {
        var progress = Math.Clamp(request.DeliveryProgress, 0, 100);

        var boxTypeMaterial = await _unitOfWork.Repository<BoxTypeMaterial>()
            .GetByIdAsync(request.BoxTypeMaterialId, cancellationToken);

        if (boxTypeMaterial == null)
            return Result.Failure<bool>("Box type material not found.");

        if (boxTypeMaterial.IsArrived && progress < 100)
            return Result.Failure<bool>("Once delivered (100%), progress cannot be reverted.");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var now = DateTime.UtcNow;

        var newTotalDeliveredQuantity = request.DeliveredQuantity ?? 0;

        boxTypeMaterial.DeliveryProgress = progress;
        boxTypeMaterial.DeliveredQuantity = newTotalDeliveredQuantity;
        boxTypeMaterial.Notes = request.Notes;
        boxTypeMaterial.ModifiedDate = now;

        if (progress >= 100)
        {
            boxTypeMaterial.IsArrived = true;
            boxTypeMaterial.ArrivedDate = boxTypeMaterial.ArrivedDate ?? now;
            boxTypeMaterial.ArrivedBy = currentUserId;
        }

        _unitOfWork.Repository<BoxTypeMaterial>().Update(boxTypeMaterial);

        // Update individual box material deliveries using the service
        // Pass false for recalculateAll since this handler only changes delivered quantity, not quantity per box
        await _boxMaterialDeliveryService.UpdateBoxMaterialDeliveriesAsync(
            boxTypeMaterial,
            newTotalDeliveredQuantity,
            currentUserId,
            false,
            cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(true);
    }
}






