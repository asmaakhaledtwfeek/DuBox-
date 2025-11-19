using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands
{

    public class UpdateBoxStatusCommandHandler : IRequestHandler<UpdateBoxStatusCommand, Result<BoxDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBoxStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<BoxDto>> Handle(UpdateBoxStatusCommand request, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);

            if (box == null)
                return Result.Failure<BoxDto>("Box not found.");
            var oldStatus = box.Status;
            var newStatus = (BoxStatusEnum)request.Status;

            if (oldStatus == newStatus)
                return Result.Success(box.Adapt<BoxDto>());
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            box.Status = newStatus;
            box.ModifiedDate = DateTime.UtcNow;
            box.ModifiedBy = currentUserId;

            _unitOfWork.Repository<Box>().Update(box);
            var log = new AuditLog
            {
                TableName = nameof(Box),
                RecordId = box.BoxId,
                Action = "StatusChange",
                OldValues = $"Status: {oldStatus.ToString()}",
                NewValues = $"Status: {newStatus.ToString()}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Box status changed manually from {oldStatus.ToString()} to {newStatus.ToString()}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(box.Adapt<BoxDto>());
        }
    }
}
