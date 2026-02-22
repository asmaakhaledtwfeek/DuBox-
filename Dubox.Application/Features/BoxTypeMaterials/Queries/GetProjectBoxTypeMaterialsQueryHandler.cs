using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypeMaterials.Queries;

public class GetProjectBoxTypeMaterialsQueryHandler 
    : IRequestHandler<GetProjectBoxTypeMaterialsQuery, Result<List<BoxTypeMaterialDto>>>
{
    private readonly IDbContext _context;

    public GetProjectBoxTypeMaterialsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<BoxTypeMaterialDto>>> Handle(
        GetProjectBoxTypeMaterialsQuery request, 
        CancellationToken cancellationToken)
    {
        // Build query for boxes with optional building and level filters
        var boxesQuery = _context.Boxes
            .Where(b => b.ProjectId == request.ProjectId && b.ProjectBoxTypeId != null);

        // Apply building filter if provided
        if (!string.IsNullOrWhiteSpace(request.BuildingNumber))
        {
            boxesQuery = boxesQuery.Where(b => b.BuildingNumber == request.BuildingNumber);
        }

        // Apply floor/level filter if provided
        if (!string.IsNullOrWhiteSpace(request.Floor))
        {
            boxesQuery = boxesQuery.Where(b => b.Floor == request.Floor);
        }

        // Get the box type IDs that match the filters
        var filteredBoxTypeIds = await boxesQuery
            .Select(b => b.ProjectBoxTypeId!.Value)
            .Distinct()
            .ToListAsync(cancellationToken);

        // If filters are applied and no boxes match, return empty result
        if ((request.BuildingNumber != null || request.Floor != null) && filteredBoxTypeIds.Count == 0)
        {
            return Result.Success(new List<BoxTypeMaterialDto>());
        }

        // Get box type materials, filtered by box types that exist in the filtered buildings/levels
        var boxTypeMaterialsQuery = _context.BoxTypeMaterials
            .Include(btm => btm.Material)
            .Include(btm => btm.ProjectBoxType)
            .Include(btm => btm.ArrivedByUser)
            .Where(btm => btm.ProjectBoxType.ProjectId == request.ProjectId);

        // If filters are applied, only get materials for box types that have boxes in those filters
        if (filteredBoxTypeIds.Count > 0 && (request.BuildingNumber != null || request.Floor != null))
        {
            boxTypeMaterialsQuery = boxTypeMaterialsQuery
                .Where(btm => filteredBoxTypeIds.Contains(btm.ProjectBoxTypeId));
        }

        var boxTypeMaterials = await boxTypeMaterialsQuery
            .OrderBy(btm => btm.ProjectBoxType.TypeName)
            .ThenBy(btm => btm.Material.MaterialName)
            .ToListAsync(cancellationToken);

        // Get box counts for each box type in this project (with filters applied)
        var boxCountsByType = await boxesQuery
            .GroupBy(b => b.ProjectBoxTypeId)
            .Select(g => new { ProjectBoxTypeId = g.Key!.Value, Count = g.Count() })
            .ToDictionaryAsync(x => x.ProjectBoxTypeId, x => x.Count, cancellationToken);

        var result = boxTypeMaterials.Select(btm => new BoxTypeMaterialDto
        {
            BoxTypeMaterialId = btm.BoxTypeMaterialId,
            ProjectBoxTypeId = btm.ProjectBoxTypeId,
            BoxTypeName = btm.ProjectBoxType.TypeName,
            BoxCount = boxCountsByType.GetValueOrDefault(btm.ProjectBoxTypeId, 0),
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






