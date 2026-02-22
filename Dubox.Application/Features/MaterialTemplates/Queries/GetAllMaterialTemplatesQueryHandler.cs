using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public class GetAllMaterialTemplatesQueryHandler : IRequestHandler<GetAllMaterialTemplatesQuery, Result<List<MaterialTemplateListDto>>>
{
    private readonly IDbContext _context;

    public GetAllMaterialTemplatesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<MaterialTemplateListDto>>> Handle(GetAllMaterialTemplatesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MaterialTemplates.AsQueryable();

        if (request.ActiveOnly)
        {
            query = query.Where(mt => mt.IsActive);
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            query = query.Where(mt => mt.Category == request.Category);
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(mt => mt.TemplateName.Contains(request.SearchTerm) || 
                                     mt.TemplateCode.Contains(request.SearchTerm));
        }

        var templates = await query
            .Select(mt => new MaterialTemplateListDto
            {
                MaterialTemplateId = mt.MaterialTemplateId,
                TemplateName = mt.TemplateName,
                TemplateCode = mt.TemplateCode,
                Description = mt.Description,
                Category = mt.Category,
                IsActive = mt.IsActive,
                ItemCount = mt.Items.Count,
                CreatedDate = mt.CreatedDate,
                CreatedBy = mt.CreatedBy
            })
            .OrderByDescending(mt => mt.CreatedDate)
            .ToListAsync(cancellationToken);

        return Result.Success(templates);
    }
}






