using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

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
        var predefinedItems = await _unitOfWork.Repository<PredefinedChecklistItem>()
            .FindAsync(x => x.IsActive, cancellationToken);

        var items = predefinedItems
            .OrderBy(x => x.Sequence)
            .Adapt<List<PredefinedChecklistItemDto>>();

        return Result.Success(items);
    }
}

