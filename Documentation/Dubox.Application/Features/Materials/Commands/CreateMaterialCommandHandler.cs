using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Materials.Commands;

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, Result<MaterialDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public CreateMaterialCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<MaterialDto>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var materialExists = await _unitOfWork.Repository<Material>()
            .IsExistAsync(m => m.MaterialCode == request.MaterialCode, cancellationToken);

        if (materialExists)
            return Result.Failure<MaterialDto>("Material with this code already exists");

        var material = request.Adapt<Material>();
        material.IsActive = true;
        await _unitOfWork.Repository<Material>().AddAsync(material, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        if (request.CurrentStock.HasValue && request.CurrentStock.Value > 0)
        {
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            var performedUser = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);
            var transaction = new MaterialTransaction
            {
                MaterialId = material.MaterialId,
                TransactionType = MaterialTransactionTypeEnum.Receipt,
                Quantity = material.CurrentStock,
                TransactionDate = DateTime.UtcNow,
                Reference = $"Initial stock upon creation (Code: {material.MaterialCode})",
                PerformedById = currentUserId
            };
            await _unitOfWork.Repository<MaterialTransaction>().AddAsync(transaction, cancellationToken);
        }
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = material.Adapt<MaterialDto>() with
        {
            IsLowStock = material.IsLowStock,
            NeedsReorder = material.NeedsReorder
        };

        return Result.Success(dto);
    }
}

