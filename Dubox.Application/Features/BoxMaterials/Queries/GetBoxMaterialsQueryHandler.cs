using Dubox.Application.Features.BoxMaterials.Commands;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxMaterials.Queries;

public class GetBoxMaterialsQueryHandler 
    : IRequestHandler<GetBoxMaterialsQuery, Result<List<BoxMaterialDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _context;

    public GetBoxMaterialsQueryHandler(
        IUnitOfWork unitOfWork,
        IDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result<List<BoxMaterialDto>>> Handle(
        GetBoxMaterialsQuery request, 
        CancellationToken cancellationToken)
    {
        // Get box materials with includes using a query that joins with BoxTypeMaterial
        var query = from bm in _context.BoxMaterials
                    where bm.BoxId == request.BoxId
                    join m in _context.Materials on bm.MaterialId equals m.MaterialId
                    join b in _context.Boxes on bm.BoxId equals b.BoxId
                    join btm in _context.BoxTypeMaterials 
                        on new { b.ProjectBoxTypeId, bm.MaterialId } 
                        equals new { ProjectBoxTypeId = (int?)btm.ProjectBoxTypeId, btm.MaterialId }
                        into btmGroup
                    from boxTypeMaterial in btmGroup.DefaultIfEmpty()
                    select new 
                    {
                        BoxMaterial = bm,
                        Material = m,
                        Box = b,
                        BoxTypeMaterial = boxTypeMaterial
                    };

        var boxMaterialsData = await query.ToListAsync(cancellationToken);

        // Map to domain objects for filtering
        var boxMaterials = boxMaterialsData.Select(x => 
        {
            var bm = x.BoxMaterial;
            bm.Material = x.Material;
            bm.Box = x.Box;
            return new { BoxMaterial = bm, QuantityPerBox = x.BoxTypeMaterial?.QuantityPerBox };
        }).ToList();

        // Apply filters
        var filteredMaterials = boxMaterials;
        
        if (request.OnlyOverdue == true)
        {
            filteredMaterials = filteredMaterials.Where(x => x.BoxMaterial.IsOverdue).ToList();
        }

        if (request.OnlyPending == true)
        {
            filteredMaterials = filteredMaterials.Where(x => !x.BoxMaterial.IsArrived).ToList();
        }

        // Map to DTOs
        var dtos = filteredMaterials.Select(x => new BoxMaterialDto
        {
            BoxMaterialId = x.BoxMaterial.BoxMaterialId,
            BoxId = x.BoxMaterial.BoxId,
            BoxTag = x.BoxMaterial.Box?.BoxTag,
            MaterialId = x.BoxMaterial.MaterialId,
            MaterialCode = x.BoxMaterial.Material?.MaterialCode ?? "",
            MaterialName = x.BoxMaterial.Material?.MaterialName ?? "",
            MaterialCategory = x.BoxMaterial.Material?.MaterialCategory,
            RequiredBeforeDays = x.BoxMaterial.RequiredBeforeDays,
            RequiredByDate = x.BoxMaterial.RequiredByDate,
            IsArrived = x.BoxMaterial.IsArrived,
            ArrivedDate = x.BoxMaterial.ArrivedDate,
            DaysUntilRequired = x.BoxMaterial.DaysUntilRequired,
            IsOverdue = x.BoxMaterial.IsOverdue,
            Status = x.BoxMaterial.Status,
            DeliveredQuantity = x.BoxMaterial.DeliveredQuantity,
            QuantityPerBox = x.QuantityPerBox
        }).ToList();

        return Result.Success(dtos);
    }
}






