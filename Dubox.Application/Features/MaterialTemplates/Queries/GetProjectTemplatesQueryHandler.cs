using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public class GetProjectTemplatesQueryHandler : IRequestHandler<GetProjectTemplatesQuery, Result<List<ProjectMaterialTemplateDto>>>
{
    private readonly IDbContext _context;

    public GetProjectTemplatesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ProjectMaterialTemplateDto>>> Handle(GetProjectTemplatesQuery request, CancellationToken cancellationToken)
    {
        var templates = await _context.ProjectMaterialTemplates
            .Where(pmt => pmt.ProjectId == request.ProjectId)
            .Include(pmt => pmt.MaterialTemplate)
            .ThenInclude(mt => mt.Items)
            .Select(pmt => new ProjectMaterialTemplateDto
            {
                ProjectMaterialTemplateId = pmt.ProjectMaterialTemplateId,
                ProjectId = pmt.ProjectId,
                MaterialTemplateId = pmt.MaterialTemplateId,
                TemplateName = pmt.MaterialTemplate.TemplateName,
                TemplateCode = pmt.MaterialTemplate.TemplateCode,
                Category = pmt.MaterialTemplate.Category,
                ItemCount = pmt.MaterialTemplate.Items.Count,
                AssignedDate = pmt.AssignedDate,
                AssignedBy = pmt.AssignedBy
            })
            .OrderByDescending(pmt => pmt.AssignedDate)
            .ToListAsync(cancellationToken);

        return Result.Success(templates);
    }
}






