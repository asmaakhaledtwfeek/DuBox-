using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands;

public class DeleteChecklistItemCommandHandler : IRequestHandler<DeleteChecklistItemCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteChecklistItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteChecklistItemCommand request, CancellationToken cancellationToken)
    {
        var checklistItemRepository = _unitOfWork.Repository<WIRChecklistItem>();
        var checklistItem = await checklistItemRepository.GetByIdAsync(request.ChecklistItemId, cancellationToken);

        if (checklistItem == null)
            return Result.Failure<bool>("Checklist item not found");

        checklistItemRepository.Delete(checklistItem);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }
}

