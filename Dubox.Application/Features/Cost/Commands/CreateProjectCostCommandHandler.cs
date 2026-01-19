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
        // Verify box exists
        var box = await _unitOfWork.Repository<Box>()
            .GetByIdAsync(request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<ProjectCostDto>("Box not found.");

        // Get current user
        Guid? currentUserId = null;
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
            currentUserId = userId;

        // Create project cost entity
        var projectCost = new ProjectCost
        {
            BoxId = request.BoxId,
            Cost = request.Cost,
            CostType = request.CostType,
            HRCostRecordId = request.HRCostRecordId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId,
            ProjectId = box.ProjectId
        };

        try
        {
            await _unitOfWork.Repository<ProjectCost>().AddAsync(projectCost, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Map to DTO and include box information
            var projectCostDto = new ProjectCostDto
            {
                ProjectCostId = projectCost.ProjectCostId,
                BoxId = projectCost.BoxId,
                HRCostRecordId = projectCost.HRCostRecordId,
                Cost = projectCost.Cost,
                CostType = projectCost.CostType,
                CreatedDate = projectCost.CreatedDate,
                CreatedBy = projectCost.CreatedBy,
                BoxTag = box.BoxTag
            };

            return Result.Success(projectCostDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<ProjectCostDto>($"Failed to create project cost: {ex.Message}");
        }
    }
}



