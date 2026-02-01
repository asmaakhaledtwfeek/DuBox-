using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands;

public class UpdateChecklistItemCommandHandler : IRequestHandler<UpdateChecklistItemCommand, Result<WIRChecklistItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateChecklistItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WIRChecklistItemDto>> Handle(UpdateChecklistItemCommand request, CancellationToken cancellationToken)
    {
        var checklistItemRepository = _unitOfWork.Repository<WIRChecklistItem>();
        var checklistItem = await checklistItemRepository.GetByIdAsync(request.ChecklistItemId, cancellationToken);

        if (checklistItem == null)
            return Result.Failure<WIRChecklistItemDto>("Checklist item not found");

        // Allow editing description, reference document, status, remarks, and sequence
        // But prevent changing PredefinedItemId (it should remain linked to the original predefined item)
        if (request.CheckpointDescription != null)
            checklistItem.CheckpointDescription = request.CheckpointDescription;

        if (request.ReferenceDocument != null)
            checklistItem.ReferenceDocument = request.ReferenceDocument;

        if (request.Status.HasValue)
            checklistItem.Status = request.Status.Value;

        if (request.Remarks != null)
            checklistItem.Remarks = request.Remarks;

        if (request.Sequence.HasValue)
            checklistItem.Sequence = request.Sequence.Value;

        checklistItemRepository.Update(checklistItem);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = checklistItem.Adapt<WIRChecklistItemDto>();
        return Result.Success(dto);
    }
}

