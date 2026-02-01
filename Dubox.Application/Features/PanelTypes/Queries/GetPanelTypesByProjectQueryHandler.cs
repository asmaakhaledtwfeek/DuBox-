using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.PanelTypes.Queries;

public class GetPanelTypesByProjectQueryHandler : IRequestHandler<GetPanelTypesByProjectQuery, Result<List<PanelTypeDto>>>
{
    private readonly IDbContext _dbContext;

    public GetPanelTypesByProjectQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<PanelTypeDto>>> Handle(GetPanelTypesByProjectQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.PanelTypes
            .Where(pt => pt.ProjectId == request.ProjectId);

        if (!request.IncludeInactive)
        {
            query = query.Where(pt => pt.IsActive);
        }

        var panelTypes = await query
            .OrderBy(pt => pt.DisplayOrder)
            .ThenBy(pt => pt.PanelTypeName)
            .Select(pt => new PanelTypeDto
            {
                PanelTypeId = pt.PanelTypeId,
                ProjectId = pt.ProjectId,
                PanelTypeName = pt.PanelTypeName,
                PanelTypeCode = pt.PanelTypeCode,
                Description = pt.Description,
                IsActive = pt.IsActive,
                DisplayOrder = pt.DisplayOrder,
                CreatedDate = pt.CreatedDate,
                ModifiedDate = pt.ModifiedDate
            })
            .ToListAsync(cancellationToken);

        return Result.Success(panelTypes);
    }
}

