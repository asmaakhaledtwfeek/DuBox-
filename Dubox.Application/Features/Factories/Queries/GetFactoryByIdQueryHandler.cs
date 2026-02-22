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
        // Load factory with Sections → Parts (2-level only).
        // FreezingCells are loaded via a separate explicit query below because
        // chaining ThenInclude after a filtered/ordered Include is unreliable in EF Core
        // and produces empty collections even when data exists.
        var factory = await _dbContext.Factories
            .Include(f => f.Sections.OrderBy(s => s.DisplayOrder))
                .ThenInclude(s => s.Parts)
            .Include(f => f.Boxes)
                .ThenInclude(b => b.Project)
            .FirstOrDefaultAsync(f => f.FactoryId == request.FactoryId, cancellationToken);

        if (factory == null)
            return Result.Failure<FactoryDto>("Factory not found");

        // Collect all PartIds across all sections so we can batch-load FreezingCells
        var partIds = factory.Sections
            .SelectMany(s => s.Parts)
            .Select(p => p.PartId)
            .ToList();

        // Load FreezingCells for every part in a single query, then distribute to each part
        if (partIds.Count > 0)
        {
            var freezingCells = await _dbContext.FreezingCells
                .Where(f => partIds.Contains(f.FactorySectionPartId))
                .ToListAsync(cancellationToken);

            // Group by FactorySectionPartId for O(1) lookup
            var freezingCellsByPart = freezingCells
                .GroupBy(f => f.FactorySectionPartId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var section in factory.Sections)
            {
                foreach (var part in section.Parts)
                {
                    part.FreezingCells = freezingCellsByPart.TryGetValue(part.PartId, out var cells)
                        ? cells
                        : new List<FreezingCell>();
                }
            }
        }

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

