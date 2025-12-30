using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public class DeleteBoxCommandHandler : IRequestHandler<DeleteBoxCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Box> _genericRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private readonly IProjectProgressService _projectProgressService;

    public DeleteBoxCommandHandler(IUnitOfWork unitOfWork, IGenericRepository<Box> genericRepository, ICurrentUserService currentUserService, IProjectTeamVisibilityService visibilityService , IProjectProgressService projectProgressService)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = genericRepository;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
        _projectProgressService = projectProgressService;
    }

    public async Task<Result<bool>> Handle(DeleteBoxCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        try
        {
            var box = _unitOfWork.Repository<Box>().GetEntityWithSpec(new GetBoxByIdWithIncludesSpecification(request.BoxId));
            if (box == null)
                return Result.Failure<bool>("Box not found");

            // Check if project is archived
            var isArchived = await _visibilityService.IsProjectArchivedAsync(box.ProjectId, cancellationToken);
            if (isArchived)
            {
                return Result.Failure<bool>("Cannot delete boxes from an archived project. Archived projects are read-only.");
            }

            // Check if project is on hold
            var isOnHold = await _visibilityService.IsProjectOnHoldAsync(box.ProjectId, cancellationToken);
            if (isOnHold)
            {
                return Result.Failure<bool>("Cannot delete boxes from a project on hold. Projects on hold only allow status changes.");
            }

            // Check if box is Dispatched - cannot delete
            if (box.Status == BoxStatusEnum.Dispatched)
            {
                return Result.Failure<bool>("Cannot delete a dispatched box. Dispatched boxes are read-only and no actions are allowed.");
            }

            var projectId = box.ProjectId;
            var boxTag = box.BoxTag;
            box.IsActive = false;
            box.DeletedDated = DateTime.UtcNow;
            _unitOfWork.Repository<Box>().Update(box);
            
            var boxLog = new AuditLog
            {
                TableName = nameof(Box),
                RecordId = box.BoxId,
                Action = "Deletion (Cascade)",
                OldValues = $"Tag: {boxTag}, Project ID: {projectId}, Status: {box.Status.ToString()}",
                NewValues = "N/A (Entity Deleted)",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Box '{boxTag}' was deleted. Related Activities, Progress, and WIR records were deleted via cascade."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(boxLog, cancellationToken);
            await _unitOfWork.CompleteAsync();

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(projectId, cancellationToken);
            if (project != null)
            {
                var oldTotalBoxes = project.TotalBoxes;

                var boxCount = await _unitOfWork.Repository<Box>()
                    .CountAsync(b => b.ProjectId == projectId && b.BoxId != request.BoxId, cancellationToken);

                project.TotalBoxes = boxCount;
               await _projectProgressService.UpdateProjectProgressAsync(projectId, currentUserId, $"Project progress recalculated due to delete '{box.BoxTag}'.",cancellationToken);
                _unitOfWork.Repository<Project>().Update(project);

                if (oldTotalBoxes != project.TotalBoxes)
                {
                    var projectLog = new AuditLog
                    {
                        TableName = nameof(Project),
                        RecordId = project.ProjectId,
                        Action = "TotalBoxesUpdate",
                        OldValues = $"TotalBoxes: {oldTotalBoxes}",
                        NewValues = $"TotalBoxes: {project.TotalBoxes}",
                        ChangedBy = currentUserId,
                        ChangedDate = DateTime.UtcNow,
                        Description = $"Total box count decremented due to Box '{boxTag}' deletion."
                    };
                    await _unitOfWork.Repository<AuditLog>().AddAsync(projectLog, cancellationToken);
                }
            }
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Error deleting box: {ex.Message}");
        }
    }
}

