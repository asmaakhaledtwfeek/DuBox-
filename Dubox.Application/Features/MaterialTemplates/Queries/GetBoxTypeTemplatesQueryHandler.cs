using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public class GetBoxTypeTemplatesQueryHandler : IRequestHandler<GetBoxTypeTemplatesQuery, Result<List<BoxTypeMaterialTemplateDto>>>
{
    private readonly IDbContext _context;

    public GetBoxTypeTemplatesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<BoxTypeMaterialTemplateDto>>> Handle(GetBoxTypeTemplatesQuery request, CancellationToken cancellationToken)
    {
        // Get all assignments for the project
        var allAssignments = await _context.BoxTypeMaterialTemplates
            .Where(btmt => btmt.ProjectBoxType.ProjectId == request.ProjectId)
            .Include(btmt => btmt.ProjectBoxType)
            .Include(btmt => btmt.MaterialTemplate)
            .ThenInclude(mt => mt.Items)
            .ToListAsync(cancellationToken);

        // Group by box type and take only the most recent assignment for each
        var templates = allAssignments
            .GroupBy(btmt => btmt.ProjectBoxTypeId)
            .Select(group => group.OrderByDescending(btmt => btmt.AssignedDate).First())
            .Select(btmt => new BoxTypeMaterialTemplateDto
            {
                BoxTypeMaterialTemplateId = btmt.BoxTypeMaterialTemplateId,
                ProjectBoxTypeId = btmt.ProjectBoxTypeId,
                BoxTypeName = btmt.ProjectBoxType.TypeName,
                MaterialTemplateId = btmt.MaterialTemplateId,
                TemplateName = btmt.MaterialTemplate.TemplateName,
                TemplateCode = btmt.MaterialTemplate.TemplateCode,
                Category = btmt.MaterialTemplate.Category,
                ItemCount = btmt.MaterialTemplate.Items.Count,
                AssignedDate = btmt.AssignedDate,
                AssignedBy = btmt.AssignedBy
            })
            .OrderBy(btmt => btmt.BoxTypeName)
            .ToList();

        return Result.Success(templates);
    }
}






