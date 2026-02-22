using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public class GetMaterialTemplateByIdQueryHandler : IRequestHandler<GetMaterialTemplateByIdQuery, Result<MaterialTemplateDetailDto>>
{
    private readonly IDbContext _context;

    public GetMaterialTemplateByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<MaterialTemplateDetailDto>> Handle(GetMaterialTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var template = await _context.MaterialTemplates
            .Include(mt => mt.Items)
            .ThenInclude(mti => mti.Material)
            .FirstOrDefaultAsync(mt => mt.MaterialTemplateId == request.MaterialTemplateId, cancellationToken);

        if (template == null)
            return Result.Failure<MaterialTemplateDetailDto>("Template not found.");

        var dto = new MaterialTemplateDetailDto
        {
            MaterialTemplateId = template.MaterialTemplateId,
            TemplateName = template.TemplateName,
            TemplateCode = template.TemplateCode,
            Description = template.Description,
            Category = template.Category,
            IsActive = template.IsActive,
            Items = template.Items
                .OrderBy(item => item.DisplayOrder)
                .Select(item => new MaterialTemplateItemDetailDto
                {
                    MaterialTemplateItemId = item.MaterialTemplateItemId,
                    MaterialId = item.MaterialId,
                    MaterialCode = item.Material.MaterialCode,
                    MaterialName = item.Material.MaterialName,
                    MaterialCategory = item.Material.MaterialCategory,
                    RequiredBeforeDays = item.RequiredBeforeDays,
                    IsRequired = item.IsRequired,
                    Notes = item.Notes,
                    DisplayOrder = item.DisplayOrder,
                    Unit=item.Material.Unit,
                    QuantityPerBox=item.Material.QuantityPerBox,
                })
                .ToList(),
            CreatedDate = template.CreatedDate,
            CreatedBy = template.CreatedBy,
            ModifiedDate = template.ModifiedDate,
            ModifiedBy = template.ModifiedBy
        };

        return Result.Success(dto);
    }
}






