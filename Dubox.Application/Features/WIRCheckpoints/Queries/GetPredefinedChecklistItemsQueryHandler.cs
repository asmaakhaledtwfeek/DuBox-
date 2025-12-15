using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRCheckpoints.Queries;

public class GetPredefinedChecklistItemsQueryHandler : IRequestHandler<GetPredefinedChecklistItemsQuery, Result<List<PredefinedChecklistItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPredefinedChecklistItemsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<PredefinedChecklistItemDto>>> Handle(GetPredefinedChecklistItemsQuery request, CancellationToken cancellationToken)
    {
        // Use Specification to load with Category and Reference
        var spec = new GetAllPredefinedItemsWithCategorySpecification();
        var predefinedItems = await _unitOfWork.Repository<PredefinedChecklistItem>()
            .GetWithSpec(spec).Data
            .ToListAsync(cancellationToken);

        // Filter by WIR number if provided
        if (!string.IsNullOrWhiteSpace(request.WIRNumber))
        {
            predefinedItems = predefinedItems
                .ToList();
        }

        // Map to DTOs with Category and Reference information
        var items = predefinedItems.Select(p => new PredefinedChecklistItemDto
        {
            PredefinedItemId = p.PredefinedItemId,
            CheckpointDescription = p.Description,

            Sequence = p.Sequence,
            IsActive = p.IsActive
        }).ToList();

        return Result.Success(items);
    }
}

