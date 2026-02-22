using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public class GetAvailableMaterialsForTemplateQueryHandler : IRequestHandler<GetAvailableMaterialsForTemplateQuery, Result<List<MaterialDto>>>
{
    private readonly IDbContext _context;

    public GetAvailableMaterialsForTemplateQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<MaterialDto>>> Handle(GetAvailableMaterialsForTemplateQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Materials.Where(m => m.IsActive);

        if (!string.IsNullOrEmpty(request.Category))
        {
            query = query.Where(m => m.MaterialCategory == request.Category);
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(m => m.MaterialName.Contains(request.SearchTerm) || 
                                    m.MaterialCode.Contains(request.SearchTerm));
        }

        var materials = await query
            .Select(m => new MaterialDto
            {
                MaterialId = m.MaterialId,
                MaterialCode = m.MaterialCode,
                MaterialName = m.MaterialName,
                MaterialCategory = m.MaterialCategory,
                Unit = m.Unit,
                QuantityPerBox = m.QuantityPerBox,
                DefaultRequiredBeforeDays = m.DefaultRequiredBeforeDays,
                IsActive = m.IsActive
            })
            .OrderBy(m => m.MaterialName)
            .ToListAsync(cancellationToken);

        return Result.Success(materials);
    }
}






