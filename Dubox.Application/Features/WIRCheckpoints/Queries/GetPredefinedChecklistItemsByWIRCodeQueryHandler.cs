using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries;

public class GetPredefinedChecklistItemsByWIRCodeQueryHandler
    : IRequestHandler<GetPredefinedChecklistItemsByWIRCodeQuery, Result<List<PredefinedChecklistItemWithChecklistDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPredefinedChecklistItemsByWIRCodeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<PredefinedChecklistItemWithChecklistDto>>> Handle(GetPredefinedChecklistItemsByWIRCodeQuery request, CancellationToken cancellationToken)
    {
        var wIRCheckpoint = await _unitOfWork.Repository<WIRCheckpoint>().GetByIdAsync(request.WIRCheckPointId);
        if (wIRCheckpoint == null)
            return Result.Failure<List<PredefinedChecklistItemWithChecklistDto>>("WIR checkpoint not found");
        var checklistIds = _unitOfWork.Repository<Checklist>().Get().Where(ch => ch.WIRCode == wIRCheckpoint.WIRCode)
            .Select(c => c.ChecklistId).ToList();
        var sectionIds = _unitOfWork.Repository<ChecklistSection>().Get().Where(s => s.ChecklistId.HasValue && checklistIds.Contains(s.ChecklistId.Value))
            .Select(s => s.ChecklistSectionId).ToList();

        var items = _unitOfWork.Repository<PredefinedChecklistItem>().GetWithSpec(new GetPredefineChecklistItemsWithIncludesSpecification(sectionIds)).Data.ToList();
        var dtos = items.Adapt<List<PredefinedChecklistItemWithChecklistDto>>();

        return Result.Success(dtos);
    }
}

