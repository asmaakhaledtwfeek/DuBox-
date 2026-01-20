using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateProjectCostCommandHandler : IRequestHandler<CreateProjectCostCommand, Result<ProjectCostDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateProjectCostCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ProjectCostDto>> Handle(CreateProjectCostCommand request, CancellationToken cancellationToken)
    {
        // Verify project exists
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project == null)
            return Result.Failure<ProjectCostDto>("Project not found.");
        
        // Verify box exists (if provided)
        Box? box = null;
        string? boxTag = null;
        
        if (request.BoxId.HasValue && request.BoxId.Value != Guid.Empty)
        {
            box = await _unitOfWork.Repository<Box>()
                .GetByIdAsync(request.BoxId.Value, cancellationToken);

            if (box == null)
                return Result.Failure<ProjectCostDto>("Box not found.");
            
            // Verify box belongs to the specified project
            if (box.ProjectId != request.ProjectId)
                return Result.Failure<ProjectCostDto>("Box does not belong to the specified project.");
                
            boxTag = box.BoxTag;
        }
      

        // Get current user
        Guid? currentUserId = null;
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
            currentUserId = userId;

        // Create project cost entity
        var projectCost = new ProjectCost
        {
            ProjectId = request.ProjectId,  // Use ProjectId from command
            BoxId = request.BoxId,
            Cost = request.Cost,
            
            // Cost Code Master fields
            CostCodeLevel1 = request.CostCodeLevel1,
            CostCodeLevel2 = request.CostCodeLevel2,
            CostCodeLevel3 = request.CostCodeLevel3,
            CostCodeId = request.CostCodeId,
            
            // HRC Code fields
            Chapter = request.Chapter,
            SubChapter = request.SubChapter,
            Classification = request.Classification,
            SubClassification = request.SubClassification,
            Units = request.Units,
            Type = request.Type,
            HRCostRecordId = request.HRCostRecordId,
            
            // Derived cost type from Type field
            CostType = request.Type ?? "General",
            
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId
        };

        try
        {
            await _unitOfWork.Repository<ProjectCost>().AddAsync(projectCost, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Map to DTO with all fields
            var projectCostDto = new ProjectCostDto
            {
                ProjectCostId = projectCost.ProjectCostId,
                BoxId = projectCost.BoxId,
                CostCodeId = projectCost.CostCodeId,
                HRCostRecordId = projectCost.HRCostRecordId,
                Cost = projectCost.Cost,
                CostType = projectCost.CostType,
                
                // Cost Code Master fields
                CostCodeLevel1 = projectCost.CostCodeLevel1,
                CostCodeLevel2 = projectCost.CostCodeLevel2,
                CostCodeLevel3 = projectCost.CostCodeLevel3,
                
                // HRC Code fields
                Chapter = projectCost.Chapter,
                SubChapter = projectCost.SubChapter,
                Classification = projectCost.Classification,
                SubClassification = projectCost.SubClassification,
                Units = projectCost.Units,
                Type = projectCost.Type,
                
                CreatedDate = projectCost.CreatedDate,
                CreatedBy = projectCost.CreatedBy,
                BoxTag = boxTag
            };

            return Result.Success(projectCostDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<ProjectCostDto>($"Failed to create project cost: {ex.Message}");
        }
    }
}




