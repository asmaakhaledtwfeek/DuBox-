using Dubox.Application.DTOs;
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
            .ToListAsync(cancellationToken);

        var dtos = factories.Select(f =>
        {
            // Calculate current occupancy from actual boxes, excluding NotStarted, Dispatched, and OnHold
            var currentOccupancy = f.Boxes?
                .Count(b => b.Status != BoxStatusEnum.NotStarted && b.Status != BoxStatusEnum.Dispatched && b.Status != BoxStatusEnum.OnHold) ?? 0;
            var availableCapacity = f.Capacity.HasValue 
                ? Math.Max(0, f.Capacity.Value - currentOccupancy) 
                : 0;
            var isFull = f.Capacity.HasValue && currentOccupancy >= f.Capacity.Value;

            return f.Adapt<FactoryDto>() with
            {
                CurrentOccupancy = currentOccupancy,
                AvailableCapacity = availableCapacity,
                IsFull = isFull
            };
        }).ToList();

        return Result.Success(dtos);
    }
}

