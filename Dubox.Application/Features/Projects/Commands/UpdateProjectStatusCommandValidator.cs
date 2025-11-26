using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.Projects.Commands
{
    public class UpdateProjectStatusCommandValidator : AbstractValidator<UpdateProjectStatusCommand>
    {
        public UpdateProjectStatusCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required for updating the project status.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Project status is required.")
                .Must(status => Enum.IsDefined(typeof(ProjectStatusEnum), status))
                .WithMessage("Invalid project status value.")
                .Must(status => status != ProjectStatusEnum.Completed)
                .WithMessage("Project status cannot be manually set to 'Completed'. It is set automatically when progress reaches 100%.");

        }
    }
}
