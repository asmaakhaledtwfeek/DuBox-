using Dubox.Application.DTOs;
using Dubox.Application.Features.Materials.Commands;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

public class RestockMaterialCommandHandler : IRequestHandler<RestockMaterialCommand, Result<RestockMaterialDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    public RestockMaterialCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RestockMaterialDto>> Handle(RestockMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.Repository<Material>().GetByIdAsync(request.MaterialId);
        if (material == null)
            return Result.Failure<RestockMaterialDto>("Material not found");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var performedUser = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);

        if (performedUser == null)
            return Result.Failure<RestockMaterialDto>("Inspector user not found");

        if (request.Quantity <= 0)
            return Result.Failure<RestockMaterialDto>("Quantity must be greater than zero for a restock operation.");

        material.CurrentStock = (material.CurrentStock ?? 0) + request.Quantity;

        var transaction = new MaterialTransaction
        {
            MaterialId = request.MaterialId,
            TransactionType = MaterialTransactionTypeEnum.Receipt,
            Quantity = request.Quantity,
            TransactionDate = DateTime.UtcNow,
            Reference = request.Reference,
            Remarks = request.Remarks,
            PerformedById = currentUserId
        };

        await _unitOfWork.Repository<MaterialTransaction>().AddAsync(transaction);
        _unitOfWork.Repository<Material>().Update(material);
        await _unitOfWork.CompleteAsync();

        var resultDto = new RestockMaterialDto
        {
            MaterialId = material.MaterialId,
            RestockQuantity = request.Quantity,
            CurrentStock = material.CurrentStock ?? 0,
            TransactionType = MaterialTransactionTypeEnum.Receipt,
            Reason = request.Reference
        };

        return Result.Success(resultDto);
    }
}


