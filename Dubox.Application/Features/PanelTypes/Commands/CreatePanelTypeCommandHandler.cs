using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.PanelTypes.Commands;

public class CreatePanelTypeCommandHandler : IRequestHandler<CreatePanelTypeCommand, Result<PanelTypeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreatePanelTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PanelTypeDto>> Handle(CreatePanelTypeCommand request, CancellationToken cancellationToken)
    {
        // Validate project exists
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<PanelTypeDto>("Project not found");

        // Check for duplicate code in the same project
        var existingPanelType = await _dbContext.PanelTypes
            .FirstOrDefaultAsync(pt => pt.ProjectId == request.ProjectId && 
                                      pt.PanelTypeCode == request.PanelTypeCode, 
                                 cancellationToken);

        if (existingPanelType != null)
            return Result.Failure<PanelTypeDto>($"Panel type with code '{request.PanelTypeCode}' already exists in this project");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var panelType = new PanelType
        {
            ProjectId = request.ProjectId,
            PanelTypeName = request.PanelTypeName,
            PanelTypeCode = request.PanelTypeCode,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId
        };

        await _unitOfWork.Repository<PanelType>().AddAsync(panelType, cancellationToken);
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

