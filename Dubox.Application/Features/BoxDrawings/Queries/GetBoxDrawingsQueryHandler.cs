using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxDrawings.Queries;

public class GetBoxDrawingsQueryHandler : IRequestHandler<GetBoxDrawingsQuery, Result<List<BoxDrawingDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBoxDrawingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<BoxDrawingDto>>> Handle(GetBoxDrawingsQuery request, CancellationToken cancellationToken)
    {
        // Verify box exists
        var boxExists = await _unitOfWork.Repository<Box>()
            .IsExistAsync(x => x.BoxId == request.BoxId, cancellationToken);
        
        if (!boxExists)
        {
            return Result.Failure<List<BoxDrawingDto>>("Box not found");
        }

        // Get all active box drawings for the specified box
        var boxDrawings = _unitOfWork.Repository<BoxDrawing>()
            .Get()
            .Where(bd => bd.BoxId == request.BoxId && bd.IsActive)
            .OrderByDescending(bd => bd.CreatedDate)
            .ToList();

        // Map to DTOs
        var boxDrawingDtos = boxDrawings.Adapt<List<BoxDrawingDto>>();

        return Result.Success(boxDrawingDtos);
    }
}

