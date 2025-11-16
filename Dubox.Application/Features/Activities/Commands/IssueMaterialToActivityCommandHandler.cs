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

        var currentStock = material.CurrentStock ?? 0;
        if (request.Quantity > currentStock)
            return Result.Failure<MaterialTransactionDto>(
                $"Insufficient stock. Only {currentStock} units of {material.MaterialName} are available.");


        var boxMaterial = _unitOfWork.Repository<BoxMaterial>()
            .GetEntityWithSpec(new BoxMaterialByBoxIdAndMaterialIdSpecification(activity.BoxId, request.MaterialId));

        if (boxMaterial == null)
            return Result.Failure<MaterialTransactionDto>(
                $"Cannot issue material. Material {material.MaterialName} has no budget allocated (BoxMaterial entry) for Box.");

        var allocatedRemaining = (boxMaterial.AllocatedQuantity ?? 0) - (boxMaterial.ConsumedQuantity ?? 0);

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

        material.CurrentStock = currentStock - request.Quantity;

        material.AllocatedStock = (material.AllocatedStock ?? 0) - request.Quantity;
        _unitOfWork.Repository<Material>().Update(material);

        if (boxMaterial != null)
        {
            boxMaterial.ConsumedQuantity = (boxMaterial.ConsumedQuantity ?? 0) + request.Quantity;
            if (boxMaterial.ConsumedQuantity >= boxMaterial.AllocatedQuantity)
            {
                boxMaterial.Status = BoxMaterialStatusEnum.Consumed;
            }
            _unitOfWork.Repository<BoxMaterial>().Update(boxMaterial);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        var savedTransaction = _unitOfWork.Repository<MaterialTransaction>()
            .GetEntityWithSpec(new GetMaterialTransactionWithIncludesSpecification(transaction.TransactionId));
        var dto = savedTransaction.Adapt<MaterialTransactionDto>();
        return Result.Success(dto);
    }
}