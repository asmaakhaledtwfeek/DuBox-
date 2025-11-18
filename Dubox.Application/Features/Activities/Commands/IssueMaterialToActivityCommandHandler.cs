using Dubox.Application.DTOs;
using Dubox.Application.Features.Activities.Commands;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

public class IssueMaterialToActivityCommandHandler : IRequestHandler<IssueMaterialToActivityCommand, Result<MaterialTransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public IssueMaterialToActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<MaterialTransactionDto>> Handle(IssueMaterialToActivityCommand request, CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.Repository<Material>().GetByIdAsync(request.MaterialId);
        var activity = await _unitOfWork.Repository<BoxActivity>().GetByIdAsync(request.BoxActivityId);

        if (material == null)
            return Result.Failure<MaterialTransactionDto>("Material not found.");
        if (activity == null)
            return Result.Failure<MaterialTransactionDto>("Activity not found.");

        var oldCurrentStock = material.CurrentStock ?? 0;
        var oldAllocatedStock = material.AllocatedStock ?? 0;

        if (request.Quantity > oldCurrentStock)
            return Result.Failure<MaterialTransactionDto>(
                 $"Insufficient stock. Only {oldCurrentStock} units of {material.MaterialName} are available.");

        var boxMaterial = _unitOfWork.Repository<BoxMaterial>()
            .GetEntityWithSpec(new BoxMaterialByBoxIdAndMaterialIdSpecification(activity.BoxId, request.MaterialId));

        if (boxMaterial == null)
            return Result.Failure<MaterialTransactionDto>(
                 $"Cannot issue material. Material {material.MaterialName} has no budget allocated (BoxMaterial entry) for Box.");

        var oldConsumedQuantity = boxMaterial.ConsumedQuantity ?? 0;
        var oldBoxMaterialStatus = boxMaterial.Status;

        var allocatedRemaining = (boxMaterial.AllocatedQuantity ?? 0) - oldConsumedQuantity;

        if (request.Quantity > allocatedRemaining)
            return Result.Failure<MaterialTransactionDto>(
                 $"Issue quantity ({request.Quantity}) exceeds the remaining allocated quantity for this Box ({allocatedRemaining}).");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var transaction = new MaterialTransaction
        {
            MaterialId = request.MaterialId,
            BoxActivityId = request.BoxActivityId,
            BoxId = activity.BoxId,
            TransactionType = MaterialTransactionTypeEnum.Issue,
            Quantity = request.Quantity,
            Reference = request.Reference,
            PerformedById = currentUserId,
            TransactionDate = DateTime.UtcNow
        };
        await _unitOfWork.Repository<MaterialTransaction>().AddAsync(transaction, cancellationToken);

        material.CurrentStock = oldCurrentStock - request.Quantity;
        material.AllocatedStock = oldAllocatedStock - request.Quantity;
        _unitOfWork.Repository<Material>().Update(material);

        boxMaterial.ConsumedQuantity = oldConsumedQuantity + request.Quantity;

        var newBoxMaterialStatus = oldBoxMaterialStatus;
        if (boxMaterial.ConsumedQuantity >= boxMaterial.AllocatedQuantity)
        {
            newBoxMaterialStatus = BoxMaterialStatusEnum.Consumed;
            boxMaterial.Status = newBoxMaterialStatus;
        }
        _unitOfWork.Repository<BoxMaterial>().Update(boxMaterial);



        var boxMaterialLog = new AuditLog
        {
            TableName = nameof(BoxMaterial),
            RecordId = boxMaterial.BoxMaterialId,
            Action = "ConsumptionUpdate",
            OldValues = $"ConsumedQuantity: {oldConsumedQuantity}, Status: {oldBoxMaterialStatus.ToString()}",
            NewValues = $"ConsumedQuantity: {boxMaterial.ConsumedQuantity}, Status: {newBoxMaterialStatus.ToString()}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Consumed quantity for Material in Box {activity.BoxId} increased by {request.Quantity}."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(boxMaterialLog, cancellationToken);


        await _unitOfWork.CompleteAsync(cancellationToken);

        var savedTransaction = _unitOfWork.Repository<MaterialTransaction>()
            .GetEntityWithSpec(new GetMaterialTransactionWithIncludesSpecification(transaction.TransactionId));
        var dto = savedTransaction.Adapt<MaterialTransactionDto>();
        return Result.Success(dto);
    }
}