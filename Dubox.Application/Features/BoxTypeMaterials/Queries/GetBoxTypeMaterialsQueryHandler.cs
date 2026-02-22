using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypeMaterials.Queries;

public class GetBoxTypeMaterialsQueryHandler 
    : IRequestHandler<GetBoxTypeMaterialsQuery, Result<List<BoxTypeMaterialDto>>>
{
    private readonly IDbContext _context;

    public GetBoxTypeMaterialsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<BoxTypeMaterialDto>>> Handle(
        GetBoxTypeMaterialsQuery request, 
        CancellationToken cancellationToken)
    {
        var boxTypeMaterials = await _context.BoxTypeMaterials
            .Include(btm => btm.Material)
            .Include(btm => btm.ProjectBoxType)
            .Include(btm => btm.ArrivedByUser)
            .Where(btm => btm.ProjectBoxTypeId == request.ProjectBoxTypeId)
            .OrderBy(btm => btm.Material.MaterialName)
            .ToListAsync(cancellationToken);

        var result = boxTypeMaterials.Select(btm => new BoxTypeMaterialDto
        {
            BoxTypeMaterialId = btm.BoxTypeMaterialId,
            ProjectBoxTypeId = btm.ProjectBoxTypeId,
            BoxTypeName = btm.ProjectBoxType.TypeName,
            MaterialId = btm.MaterialId,
            MaterialCode = btm.Material.MaterialCode,
            MaterialName = btm.Material.MaterialName,
            MaterialCategory = btm.Material.MaterialCategory,
            Unit = btm.Material.Unit,
            QuantityPerBox = btm.QuantityPerBox,
            RequiredBeforeDays = btm.RequiredBeforeDays,
            DeliveryProgress = btm.DeliveryProgress,
            IsArrived = btm.IsArrived,
            ArrivedDate = btm.ArrivedDate,
            ArrivedBy = btm.ArrivedBy,
            ArrivedByName = btm.ArrivedByUser != null 
                ? $"{btm.ArrivedByUser.FullName}".Trim()
                : null,
            DeliveredQuantity = btm.DeliveredQuantity,
            Notes = btm.Notes,
            Status = btm.Status,
            CreatedDate = btm.CreatedDate
        }).ToList();

        return Result.Success(result);
    }
}






