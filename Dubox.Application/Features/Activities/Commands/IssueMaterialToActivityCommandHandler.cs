using Dubox.Application.DTOs;
using Dubox.Application.Features.Activities.Commands;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

public class IssueMaterialToActivityCommandHandler : IRequestHandler<IssueMaterialToActivityCommand, Result<MaterialTransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    
    public IssueMaterialToActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<MaterialTransactionDto>> Handle(IssueMaterialToActivityCommand request, CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.Repository<Material>().GetByIdAsync(request.MaterialId);
        var activity = await _unitOfWork.Repository<BoxActivity>().GetByIdAsync(request.BoxActivityId);

        if (material == null)
            return Result.Failure<MaterialTransactionDto>("Material not found.");
        if (activity == null)
            return Result.Failure<MaterialTransactionDto>("Activity not found.");

        // Get the box to check project status
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(activity.BoxId, cancellationToken);
        if (box == null)
            return Result.Failure<MaterialTransactionDto>("Box not found.");

        // Check if project is archived
        var isArchived = await _visibilityService.IsProjectArchivedAsync(box.ProjectId, cancellationToken);
        if (isArchived)
        {
            return Result.Failure<MaterialTransactionDto>("Cannot issue materials in an archived project. Archived projects are read-only.");
        }

        // Check if project is on hold
        var isOnHold = await _visibilityService.IsProjectOnHoldAsync(box.ProjectId, cancellationToken);
        if (isOnHold)
        {
            return Result.Failure<MaterialTransactionDto>("Cannot issue materials in a project on hold. Projects on hold only allow project status changes.");
        }

        // Check if project is closed
        var isClosed = await _visibilityService.IsProjectClosedAsync(box.ProjectId, cancellationToken);
        if (isClosed)
        {
            return Result.Failure<MaterialTransactionDto>("Cannot issue materials in a closed project. Closed projects only allow project status changes.");
        }

        // Check if box is Dispatched - cannot perform any actions on activities
        if (box.Status == BoxStatusEnum.Dispatched)
        {
            return Result.Failure<MaterialTransactionDto>("Cannot issue materials to activity. The box is dispatched and no actions are allowed on boxes or activities.");
        }

        // Check if box is OnHold - cannot perform actions on activities
        if (box.Status == BoxStatusEnum.OnHold)
        {
            return Result.Failure<MaterialTransactionDto>("Cannot issue materials to activity. The box is on hold and no actions are allowed on activities. Only box status changes are allowed.");
        }

        // Check activity status - cannot perform actions if activity is Completed or OnHold
        if (activity.Status == BoxStatusEnum.Completed)
        {
            return Result.Failure<MaterialTransactionDto>("Cannot issue materials to activity. Activities in 'Completed' status cannot be modified.");
        }
        if (activity.Status == BoxStatusEnum.OnHold)
        {
            return Result.Failure<MaterialTransactionDto>("Cannot issue materials to activity. Activities in 'OnHold' status cannot be modified. Please change the activity status first.");
        }

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