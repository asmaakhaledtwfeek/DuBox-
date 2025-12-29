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
                .WithMessage("Invalid project status value.")
                .Must(status => status != ProjectStatusEnum.Completed)
                .WithMessage("Project status cannot be manually set to 'Completed'. It is set automatically when progress reaches 100%.");

            RuleFor(x => x)
                .MustAsync(NotBeArchivedProject)
                .WithMessage("Cannot change status of an archived project. Archived projects are locked and cannot be modified.");
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
    }
}
