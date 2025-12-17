using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;


namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public class CreateWIRCheckpointCommandHandler : IRequestHandler<CreateWIRCheckpointCommand, Result<CreateWIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public CreateWIRCheckpointCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _visibilityService = visibilityService;
        }

        public async Task<Result<CreateWIRCheckpointDto>> Handle(CreateWIRCheckpointCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<CreateWIRCheckpointDto>("Access denied. Viewer role has read-only access and cannot create WIR checkpoints.");
            }

            var boxActicity = await _unitOfWork.Repository<BoxActivity>().GetByIdAsync(request.BoxActivityId);
            if (boxActicity == null)
                return Result.Failure<CreateWIRCheckpointDto>("Box Activity not fount");

            // Load box to get project ID for authorization check
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxActicity.BoxId, cancellationToken);
            if (box == null)
                return Result.Failure<CreateWIRCheckpointDto>("Box not found");

            // Verify user has access to the project
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<CreateWIRCheckpointDto>("Access denied. You do not have permission to create WIR checkpoints for this project.");
            }

            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId);
            var currentUserName = user != null ? user.FullName : string.Empty;
            var existCheckpoint = _unitOfWork.Repository<WIRCheckpoint>().FindAsync(c => c.BoxId == boxActicity.BoxId && c.WIRCode == request.WIRNumber).Result.FirstOrDefault();

            if (existCheckpoint != null)
            {
                var existCheckpointId = existCheckpoint.WIRId;
                existCheckpoint = request.Adapt<WIRCheckpoint>();
                existCheckpoint.WIRId = existCheckpointId;
                existCheckpoint.RequestedBy = currentUserName;
                existCheckpoint.BoxId = boxActicity.BoxId;
                existCheckpoint.WIRCode = request.WIRNumber;
                _unitOfWork.Repository<WIRCheckpoint>().Update(existCheckpoint);
                var existDto = existCheckpoint.Adapt<CreateWIRCheckpointDto>();
                existDto.WIRNumber = existCheckpoint.WIRCode;
                return Result.Success(existDto);
            }
            var checkpoint = request.Adapt<WIRCheckpoint>();
            checkpoint.WIRCode = request.WIRNumber;
            checkpoint.BoxId = boxActicity.BoxId;
            checkpoint.Status = WIRCheckpointStatusEnum.Pending;
            checkpoint.CreatedDate = DateTime.UtcNow;
            checkpoint.RequestedDate = DateTime.UtcNow;
            checkpoint.RequestedBy = currentUserName;
            await _unitOfWork.Repository<WIRCheckpoint>().AddAsync(checkpoint);

            await _unitOfWork.CompleteAsync(cancellationToken);
            var dto = checkpoint.Adapt<CreateWIRCheckpointDto>();
            dto.WIRNumber = checkpoint.WIRCode;
            return Result.Success(dto);
        }
    }
}
