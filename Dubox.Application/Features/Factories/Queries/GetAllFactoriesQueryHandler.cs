using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Factories.Queries;

public class GetAllFactoriesQueryHandler : IRequestHandler<GetAllFactoriesQuery, Result<List<FactoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;

    public GetAllFactoriesQueryHandler(IUnitOfWork unitOfWork, IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<List<FactoryDto>>> Handle(GetAllFactoriesQuery request, CancellationToken cancellationToken)
    {
        var factories = await _dbContext.Factories
            .Include(f => f.Boxes)
                .ThenInclude(b => b.Project)
            .ToListAsync(cancellationToken); ;
      

        var dtos = factories.Select(f =>
        {
            // Calculate current occupancy: Only InProgress or Completed boxes from active projects
            // Exclude boxes from OnHold, Closed, or Archived projects
            var currentOccupancy = f.Boxes?
                .Count(b => (b.Status == BoxStatusEnum.InProgress || b.Status == BoxStatusEnum.Completed) &&
                           b.IsActive &&
                           b.Project != null &&
                           b.Project.Status != ProjectStatusEnum.OnHold &&
                           b.Project.Status != ProjectStatusEnum.Closed &&
                           b.Project.Status != ProjectStatusEnum.Archived) ?? 0;
            var availableCapacity = f.Capacity.HasValue 
                ? Math.Max(0, f.Capacity.Value - currentOccupancy) 
                : 0;
            var isFull = f.Capacity.HasValue && currentOccupancy >= f.Capacity.Value;
            
            // Count dispatched boxes (including from all projects, even OnHold/Closed/Archived)
            var dispatchedCount = f.Boxes?
                .Count(b => b.Status == BoxStatusEnum.Dispatched && b.IsActive) ?? 0;

            return f.Adapt<FactoryDto>() with
            {
                CurrentOccupancy = currentOccupancy,
                AvailableCapacity = availableCapacity,
                IsFull = isFull,
                DispatchedBoxesCount = dispatchedCount
            };
        }).ToList();

        return Result.Success(dtos);
    }
}

