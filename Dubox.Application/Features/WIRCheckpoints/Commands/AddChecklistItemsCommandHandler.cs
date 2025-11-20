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
            var wir = await _unitOfWork.Repository<WIRCheckpoint>().GetByIdAsync(request.WIRId);

            if (wir is null)
                return Result.Failure<CreateWIRCheckpointDto>("WIR Checkpoint not found");

            var currentUser = _currentUserService.Username;

            var newItems = request.Items.Select(item => new WIRChecklistItem
            {
                WIRId = request.WIRId,
                CheckpointDescription = item.CheckpointDescription,
                ReferenceDocument = item.ReferenceDocument,
                Sequence = item.Sequence,
                Status = CheckListItemStatusEnum.Pending,
                Remarks = null,
            }).ToList();

            await _unitOfWork.Repository<WIRChecklistItem>()
                .AddRangeAsync(newItems, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = wir.Adapt<CreateWIRCheckpointDto>();

            return Result.Success(dto);
        }
    }

}
