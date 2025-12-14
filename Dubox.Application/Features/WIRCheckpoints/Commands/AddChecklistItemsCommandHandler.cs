using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public class AddChecklistItemsCommandHandler : IRequestHandler<AddChecklistItemsCommand, Result<CreateWIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddChecklistItemsCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<CreateWIRCheckpointDto>> Handle(AddChecklistItemsCommand request, CancellationToken cancellationToken)
        {
            var wirRepository = _unitOfWork.Repository<WIRCheckpoint>();
            var wirChecklistRepository = _unitOfWork.Repository<WIRChecklistItem>();
            var predefinedRepository = _unitOfWork.Repository<PredefinedChecklistItem>();

            var wir = await wirRepository.GetByIdAsync(request.WIRId, cancellationToken);

            if (wir == null)
                return Result.Failure<CreateWIRCheckpointDto>("WIR Checkpoint not found");

            // Get existing checklist items to determine the next sequence number
            var existingItems = await wirChecklistRepository.FindAsync(x => x.WIRId == request.WIRId, cancellationToken);
            var maxSequence = existingItems.Any() ? existingItems.Max(x => x.Sequence) : 0;

            // Validate that all predefined item IDs exist and are active
            var predefinedItems = await predefinedRepository.FindAsync(
                x => request.PredefinedItemIds.Contains(x.PredefinedItemId) && x.IsActive,
                cancellationToken);

            if (predefinedItems.Count() != request.PredefinedItemIds.Count)
            {
                var foundIds = predefinedItems.Select(x => x.PredefinedItemId).ToList();
                var missingIds = request.PredefinedItemIds.Except(foundIds).ToList();
                return Result.Failure<CreateWIRCheckpointDto>($"One or more predefined checklist items not found or inactive. Missing IDs: {string.Join(", ", missingIds)}");
            }

            // Check if any of these predefined items are already added to this checkpoint
            var existingPredefinedIds = existingItems
                .Where(x => x.PredefinedItemId.HasValue)
                .Select(x => x.PredefinedItemId!.Value)
                .ToList();

            var duplicates = request.PredefinedItemIds.Intersect(existingPredefinedIds).ToList();
            if (duplicates.Any())
            {
                return Result.Failure<CreateWIRCheckpointDto>($"One or more predefined items are already added to this checkpoint. Duplicate IDs: {string.Join(", ", duplicates)}");
            }

            // Clone the predefined items to the checkpoint
            var newItems = predefinedItems
                .OrderBy(x => x.Sequence)
                .Select((predefined, index) => new WIRChecklistItem
                {
                    WIRId = request.WIRId,
                    CheckpointDescription = predefined.CheckpointDescription,

                    Sequence = maxSequence + index + 1,
                    Status = CheckListItemStatusEnum.Pending,
                    Remarks = null,
                    PredefinedItemId = predefined.PredefinedItemId
                })
                .ToList();

            await wirChecklistRepository.AddRangeAsync(newItems, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = wir.Adapt<CreateWIRCheckpointDto>();
            return Result.Success(dto);
        }
    }

}
