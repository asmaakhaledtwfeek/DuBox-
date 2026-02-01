using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.Projects.Commands
{
    public class UpdateProjectStatusCommandValidator : AbstractValidator<UpdateProjectStatusCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectStatusCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required for updating the project status.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Project status is required.")
                .Must(status => Enum.IsDefined(typeof(ProjectStatusEnum), status))
                .WithMessage("Invalid project status value.");

            RuleFor(x => x)
                .MustAsync(NotBeArchivedProject)
                .WithMessage("Cannot change status of an archived project. Archived projects are locked and cannot be modified.");

            RuleFor(x => x)
                .MustAsync(AllowCompletedStatusWhenConditionsMet)
                .WithMessage("Project status cannot be manually set to 'Completed'. It is set automatically when progress reaches 100%, or can be manually set from Closed status when progress is 100% and all boxes are completed or dispatched.");
        }

        private async Task<bool> NotBeArchivedProject(UpdateProjectStatusCommand command, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(command.ProjectId, cancellationToken);

            if (project == null)
                return false;

            // If current status is Archived, do not allow any status change
            if (project.Status == ProjectStatusEnum.Archived)
                return false;

            return true;
        }

        private async Task<bool> AllowCompletedStatusWhenConditionsMet(UpdateProjectStatusCommand command, CancellationToken cancellationToken)
        {
            // If not trying to set to Completed, allow it
            if (command.Status != ProjectStatusEnum.Completed)
                return true;

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(command.ProjectId, cancellationToken);

            if (project == null)
                return false;

            if (project.Status == ProjectStatusEnum.Closed && project.ProgressPercentage >= 100)
            {
                var boxes = await _unitOfWork.Repository<Box>()
                    .FindAsync(b => b.ProjectId == project.ProjectId && b.IsActive, cancellationToken);

                var allBoxesCompletedOrDispatched = boxes.All(b =>
                    b.Status == BoxStatusEnum.Completed || b.Status == BoxStatusEnum.Dispatched);

                if (allBoxesCompletedOrDispatched)
                {
                    return true; // Allow the transition
                }
            }

            // Also allow from OnHold if progress is 100%
            if (project.Status == ProjectStatusEnum.OnHold && project.ProgressPercentage >= 100)
            {
                return true; // Allow the transition
            }

            return false;
        }
    }
}
