using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
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
            var checkpoint = request.Adapt<WIRCheckpoint>();
            checkpoint.BoxId = boxActicity.BoxId;
            checkpoint.Status = WIRCheckpointStatusEnum.Pending;
            checkpoint.CreatedDate = DateTime.UtcNow;
            checkpoint.RequestedDate = DateTime.UtcNow;
            checkpoint.RequestedBy = currentUserName;
            await _unitOfWork.Repository<WIRCheckpoint>().AddAsync(checkpoint);
            
            // Clone all active predefined checklist items to this checkpoint
            var predefinedItems = await _unitOfWork.Repository<PredefinedChecklistItem>()
                .FindAsync(x => x.IsActive, cancellationToken);
            
            var checklistItems = predefinedItems
                .OrderBy(x => x.Sequence)
                .Select((predefined, index) => new WIRChecklistItem
                {
                    WIRId = checkpoint.WIRId,
                    CheckpointDescription = predefined.CheckpointDescription,
                    ReferenceDocument = predefined.ReferenceDocument,
                    Sequence = index + 1,
                    Status = CheckListItemStatusEnum.Pending,
                    Remarks = null,
                    PredefinedItemId = predefined.PredefinedItemId
                })
                .ToList();
            
            if (checklistItems.Any())
            {
                await _unitOfWork.Repository<WIRChecklistItem>().AddRangeAsync(checklistItems, cancellationToken);
            }
            
            await _unitOfWork.CompleteAsync(cancellationToken);
            var dto = checkpoint.Adapt<CreateWIRCheckpointDto>();

            return Result.Success(dto);
        }
    }
}
