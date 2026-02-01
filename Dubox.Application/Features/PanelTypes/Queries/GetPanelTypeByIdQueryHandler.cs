using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.PanelTypes.Queries;

public class GetPanelTypeByIdQueryHandler : IRequestHandler<GetPanelTypeByIdQuery, Result<PanelTypeDto>>
{
    private readonly IDbContext _dbContext;

    public GetPanelTypeByIdQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PanelTypeDto>> Handle(GetPanelTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var panelType = await _dbContext.PanelTypes
            .Where(pt => pt.PanelTypeId == request.PanelTypeId)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (panelType == null)
            return Result.Failure<PanelTypeDto>("Panel type not found");

        return Result.Success(panelType);
    }
}

