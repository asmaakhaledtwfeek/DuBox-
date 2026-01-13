using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Factories.Queries;

public class GetFactoryByIdQueryHandler : IRequestHandler<GetFactoryByIdQuery, Result<FactoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;

    public GetFactoryByIdQueryHandler(IUnitOfWork unitOfWork, IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<FactoryDto>> Handle(GetFactoryByIdQuery request, CancellationToken cancellationToken)
    {
        var factory = await _dbContext.Factories
            .Include(f => f.Boxes)
                .ThenInclude(b => b.Project)
            .FirstOrDefaultAsync(f => f.FactoryId == request.FactoryId, cancellationToken);

        if (factory == null)
            return Result.Failure<FactoryDto>("Factory not found");

        // Calculate current occupancy: Only InProgress or Completed boxes from active projects
        // Exclude boxes from OnHold, Closed, or Archived projects
        var currentOccupancy = factory.Boxes?
            .Count(b => (b.Status == BoxStatusEnum.InProgress || b.Status == BoxStatusEnum.Completed) &&
                       b.IsActive &&
                       b.Project != null &&
                       b.Project.Status != ProjectStatusEnum.OnHold &&
                       b.Project.Status != ProjectStatusEnum.Closed &&
                       b.Project.Status != ProjectStatusEnum.Archived) ?? 0;
        var availableCapacity = factory.Capacity.HasValue 
            ? Math.Max(0, factory.Capacity.Value - currentOccupancy) 
            : 0;
        var isFull = factory.Capacity.HasValue && currentOccupancy >= factory.Capacity.Value;
        
        // Count dispatched boxes (including from all projects, even OnHold/Closed/Archived)
        var dispatchedCount = factory.Boxes?
            .Count(b => b.Status == BoxStatusEnum.Dispatched && b.IsActive) ?? 0;

        var dto = factory.Adapt<FactoryDto>() with
        {
            CurrentOccupancy = currentOccupancy,
            AvailableCapacity = availableCapacity,
            IsFull = isFull,
            DispatchedBoxesCount = dispatchedCount
        };

        return Result.Success(dto);
    }
}

