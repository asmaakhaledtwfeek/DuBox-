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
        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(activity.Box.ProjectId, "issue materials to activity", cancellationToken);

        if (!projectStatusValidation.IsSuccess)
            return Result.Failure<MaterialTransactionDto>(projectStatusValidation.Error!);

        var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(activity.Box.BoxId, "issue materials to activity", cancellationToken);

        if (!boxStatusValidation.IsSuccess)
            return Result.Failure<MaterialTransactionDto>(boxStatusValidation.Error!);

        var activityStatusValidation = await _visibilityService.GetActivityStatusChecksAsync(activity.Box.BoxId, "issue materials to activity", cancellationToken);

        if (!boxStatusValidation.IsSuccess)
            return Result.Failure<MaterialTransactionDto>(activityStatusValidation.Error!);

        var oldCurrentStock = material.CurrentStock ?? 0;
        var oldAllocatedStock = material.AllocatedStock ?? 0;

        if (request.Quantity > oldCurrentStock)
            return Result.Failure<MaterialTransactionDto>(
                 $"Insufficient stock. Only {oldCurrentStock} units of {material.MaterialName} are available.");

        // Get project material allocation for this box's project
        var projectMaterial = _unitOfWork.Repository<ProjectMaterial>()
            .GetEntityWithSpec(new ProjectMaterialByProjectAndMaterialIdSpecification(box.ProjectId, request.MaterialId));

        if (projectMaterial == null)
            return Result.Failure<MaterialTransactionDto>(
                 $"Cannot issue material. Material {material.MaterialName} is not allocated to this project.");

        var oldConsumedQuantity = projectMaterial.ConsumedQuantity ?? 0;

        var allocatedRemaining = (projectMaterial.AllocatedQuantity ?? 0) - oldConsumedQuantity;

        if (request.Quantity > allocatedRemaining)
            return Result.Failure<MaterialTransactionDto>(
                 $"Issue quantity ({request.Quantity}) exceeds the remaining allocated quantity for this project ({allocatedRemaining}).");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var transaction= request.Adapt<MaterialTransaction>();
        transaction.TransactionType = MaterialTransactionTypeEnum.Issue;
        transaction.TransactionDate= DateTime.UtcNow;
        await _unitOfWork.Repository<MaterialTransaction>().AddAsync(transaction, cancellationToken);
        
        material.CurrentStock = oldCurrentStock - request.Quantity;
        material.AllocatedStock = oldAllocatedStock - request.Quantity;
        _unitOfWork.Repository<Material>().Update(material);

        projectMaterial.ConsumedQuantity = oldConsumedQuantity + request.Quantity;
        _unitOfWork.Repository<ProjectMaterial>().Update(projectMaterial);



        var projectMaterialLog = new AuditLog
        {
            TableName = nameof(ProjectMaterial),
            RecordId = projectMaterial.ProjectMaterialId,
            Action = "ConsumptionUpdate",
            OldValues = $"ConsumedQuantity: {oldConsumedQuantity}",
            NewValues = $"ConsumedQuantity: {projectMaterial.ConsumedQuantity}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Consumed quantity for Material in Box {activity.BoxId} increased by {request.Quantity}."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(projectMaterialLog, cancellationToken);


        await _unitOfWork.CompleteAsync(cancellationToken);

        var savedTransaction = _unitOfWork.Repository<MaterialTransaction>()
            .GetEntityWithSpec(new GetMaterialTransactionWithIncludesSpecification(transaction.TransactionId));
        var dto = savedTransaction.Adapt<MaterialTransactionDto>();
        return Result.Success(dto);
    }
}