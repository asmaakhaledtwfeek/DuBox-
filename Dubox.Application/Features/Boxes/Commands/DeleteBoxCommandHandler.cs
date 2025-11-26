using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public class DeleteBoxCommandHandler : IRequestHandler<DeleteBoxCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Box> _genericRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteBoxCommandHandler(IUnitOfWork unitOfWork, IGenericRepository<Box> genericRepository, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = genericRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(DeleteBoxCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        try
        {
            var box = _unitOfWork.Repository<Box>().GetEntityWithSpec(new GetBoxByIdWithIncludesSpecification(request.BoxId));
            if (box == null)
                return Result.Failure<bool>("Box not found");

            var projectId = box.ProjectId;
            var boxTag = box.BoxTag;

            _unitOfWork.Repository<Box>().Delete(box);

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

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(projectId, cancellationToken);
            if (project != null)
            {
                var oldTotalBoxes = project.TotalBoxes;

                var boxCount = await _unitOfWork.Repository<Box>()
                    .CountAsync(b => b.ProjectId == projectId && b.BoxId != request.BoxId, cancellationToken);

                project.TotalBoxes = boxCount;
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

