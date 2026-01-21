using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.PanelTypes.Commands;

public class UpdatePanelTypeCommandHandler : IRequestHandler<UpdatePanelTypeCommand, Result<PanelTypeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public UpdatePanelTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PanelTypeDto>> Handle(UpdatePanelTypeCommand request, CancellationToken cancellationToken)
    {
        var panelType = await _unitOfWork.Repository<PanelType>()
            .GetByIdAsync(request.PanelTypeId, cancellationToken);

        if (panelType == null)
            return Result.Failure<PanelTypeDto>("Panel type not found");

        // Check for duplicate code in the same project (excluding current panel type)
        var existingPanelType = await _dbContext.PanelTypes
            .FirstOrDefaultAsync(pt => pt.ProjectId == panelType.ProjectId && 
                                      pt.PanelTypeCode == request.PanelTypeCode &&
                                      pt.PanelTypeId != request.PanelTypeId, 
                                 cancellationToken);

        if (existingPanelType != null)
            return Result.Failure<PanelTypeDto>($"Panel type with code '{request.PanelTypeCode}' already exists in this project");

        // Check for duplicate displayOrder in the same project (excluding current panel type)
        var existingPanelTypeWithOrder = await _dbContext.PanelTypes
            .FirstOrDefaultAsync(pt => pt.ProjectId == panelType.ProjectId && 
                                      pt.DisplayOrder == request.DisplayOrder &&
                                      pt.PanelTypeId != request.PanelTypeId, 
                                 cancellationToken);

        if (existingPanelTypeWithOrder != null)
            return Result.Failure<PanelTypeDto>($"Panel type with display order '{request.DisplayOrder}' already exists in this project. Display order must be unique.");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        panelType.PanelTypeName = request.PanelTypeName;
        panelType.PanelTypeCode = request.PanelTypeCode;
        panelType.Description = request.Description;
        panelType.IsActive = request.IsActive;
        panelType.DisplayOrder = request.DisplayOrder;
        panelType.ModifiedDate = DateTime.UtcNow;
        panelType.ModifiedBy = currentUserId;

        _unitOfWork.Repository<PanelType>().Update(panelType);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = new PanelTypeDto
        {
            PanelTypeId = panelType.PanelTypeId,
            ProjectId = panelType.ProjectId,
            PanelTypeName = panelType.PanelTypeName,
            PanelTypeCode = panelType.PanelTypeCode,
            Description = panelType.Description,
            IsActive = panelType.IsActive,
            DisplayOrder = panelType.DisplayOrder,
            CreatedDate = panelType.CreatedDate,
            ModifiedDate = panelType.ModifiedDate
        };

        return Result.Success(dto);
    }
}

