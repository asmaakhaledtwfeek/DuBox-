using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class UpdateProjectBoxTypeCommandHandler : IRequestHandler<UpdateProjectBoxTypeCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBoxActivityService _boxActivityService;
    private readonly IBoxTypeTemplateAutoAssignmentService _boxTypeTemplateService;

    public UpdateProjectBoxTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IBoxActivityService boxActivityService,
        IBoxTypeTemplateAutoAssignmentService boxTypeTemplateService)
    {
        _unitOfWork = unitOfWork;
        _boxActivityService = boxActivityService;
        _boxTypeTemplateService = boxTypeTemplateService;
    }

    public async Task<Result<bool>> Handle(UpdateProjectBoxTypeCommand request, CancellationToken cancellationToken)
    {
        // Verify project exists
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<bool>("Project not found");

        // Get the box type
        var boxType = _unitOfWork.Repository<ProjectBoxType>()
            .Get()
            .FirstOrDefault(bt => bt.Id == request.BoxTypeId && bt.ProjectId == request.ProjectId);

        if (boxType == null)
            return Result.Failure<bool>("Box type not found or does not belong to this project");

        // ── Activity Template ────────────────────────────────────────────────
        if (request.ActivityTemplateId.HasValue)
        {
            var actTemplate = _unitOfWork.Repository<ActivityTemplate>()
                .Get()
                .FirstOrDefault(t => t.ActivityTemplateId == request.ActivityTemplateId.Value);

            if (actTemplate == null)
                return Result.Failure<bool>("Activity template not found");

            if (!actTemplate.IsActive)
                return Result.Failure<bool>("Activity template is not active");

            var oldActivityTemplateId = boxType.ActivityTemplateId;
            boxType.ActivityTemplateId = request.ActivityTemplateId.Value;
            _unitOfWork.Repository<ProjectBoxType>().Update(boxType);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Propagate to NotStarted / ReadyToStart boxes under this box type
            if (oldActivityTemplateId != request.ActivityTemplateId.Value)
            {
                await _boxTypeTemplateService.PropagateActivityTemplateToBoxTypeBoxesAsync(
                    request.BoxTypeId,
                    request.ActivityTemplateId.Value,
                    cancellationToken);
            }
        }

        // ── Material Template ────────────────────────────────────────────────
        if (request.MaterialTemplateId.HasValue)
        {
            // Determine old material template (if any) for the box type
            var currentUser = "System";
            var result = await _boxTypeTemplateService.ChangeMaterialTemplateForBoxTypeAsync(
                request.BoxTypeId,
                request.MaterialTemplateId.Value,
                oldMaterialTemplateId: null,
                assignedBy: currentUser,
                cancellationToken);

            if (!result.IsSuccess)
                return Result.Failure<bool>(result.Error!);
        }

        return Result.Success(true);
    }
}
