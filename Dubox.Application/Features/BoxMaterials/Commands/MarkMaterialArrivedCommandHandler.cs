using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxMaterials.Commands;

public class MarkMaterialArrivedCommandHandler 
    : IRequestHandler<MarkMaterialArrivedCommand, Result<BoxMaterialDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDbContext _context;

    public MarkMaterialArrivedCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IDbContext context)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task<Result<BoxMaterialDto>> Handle(
        MarkMaterialArrivedCommand request, 
        CancellationToken cancellationToken)
    {
        // Find the box material
        var boxMaterial = _unitOfWork.Repository<BoxMaterial>()
            .GetEntityWithSpec(new BoxMaterialByBoxIdAndMaterialIdSpecification(
                request.BoxId, 
                request.MaterialId));

        if (boxMaterial == null)
            return Result.Failure<BoxMaterialDto>("Box material not found.");

        var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
            ? parsedUserId
            : (Guid?)null;

        // Update arrival status
        boxMaterial.IsArrived = request.IsArrived;
        
        if (request.IsArrived)
        {
            boxMaterial.ArrivedDate = DateTime.UtcNow;
            boxMaterial.ArrivedBy = currentUserId;
            
            // If there was a quality issue for this material, we might want to auto-close it
            // This logic can be added later
        }
        else
        {
            boxMaterial.ArrivedDate = null;
            boxMaterial.ArrivedBy = null;
        }

        boxMaterial.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.Repository<BoxMaterial>().Update(boxMaterial);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Get QuantityPerBox from BoxTypeMaterial
        var quantityPerBox = await _context.BoxTypeMaterials
            .Where(btm => btm.ProjectBoxTypeId == boxMaterial.Box.ProjectBoxTypeId 
                       && btm.MaterialId == boxMaterial.MaterialId)
            .Select(btm => (int?)btm.QuantityPerBox)
            .FirstOrDefaultAsync(cancellationToken);

        // Map to DTO
        var dto = new BoxMaterialDto
        {
            BoxMaterialId = boxMaterial.BoxMaterialId,
            BoxId = boxMaterial.BoxId,
            BoxTag = boxMaterial.Box?.BoxTag,
            MaterialId = boxMaterial.MaterialId,
            MaterialCode = boxMaterial.Material?.MaterialCode ?? "",
            MaterialName = boxMaterial.Material?.MaterialName ?? "",
            MaterialCategory = boxMaterial.Material?.MaterialCategory,
            RequiredBeforeDays = boxMaterial.RequiredBeforeDays,
            RequiredByDate = boxMaterial.RequiredByDate,
            IsArrived = boxMaterial.IsArrived,
            ArrivedDate = boxMaterial.ArrivedDate,
            DaysUntilRequired = boxMaterial.DaysUntilRequired,
            IsOverdue = boxMaterial.IsOverdue,
            Status = boxMaterial.Status,
            DeliveredQuantity = boxMaterial.DeliveredQuantity,
            QuantityPerBox = quantityPerBox
        };

        return Result.Success(dto);
    }
}






