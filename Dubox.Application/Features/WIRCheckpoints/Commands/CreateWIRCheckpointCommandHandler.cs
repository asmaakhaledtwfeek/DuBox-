using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;


namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public class CreateWIRCheckpointCommandHandler : IRequestHandler<CreateWIRCheckpointCommand, Result<CreateWIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentUserService _currentUserService;

        public CreateWIRCheckpointCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<CreateWIRCheckpointDto>> Handle(CreateWIRCheckpointCommand request, CancellationToken cancellationToken)
        {
            var boxActicity = await _unitOfWork.Repository<BoxActivity>().GetByIdAsync(request.BoxActivityId);
            if (boxActicity == null)
                return Result.Failure<CreateWIRCheckpointDto>("Box Activity not fount");

            var currentUserName = _currentUserService.Username;

            var checkpoint = request.Adapt<WIRCheckpoint>();
            checkpoint.BoxId = boxActicity.BoxId;
            checkpoint.Status = WIRCheckpointStatusEnum.Pending;
            checkpoint.CreatedDate = DateTime.UtcNow;
            checkpoint.RequestedDate = DateTime.UtcNow;
            checkpoint.RequestedBy = currentUserName;
            await _unitOfWork.Repository<WIRCheckpoint>().AddAsync(checkpoint);
            await _unitOfWork.CompleteAsync(cancellationToken);
            var dto = checkpoint.Adapt<CreateWIRCheckpointDto>();

            return Result.Success(dto);
        }
    }
}
