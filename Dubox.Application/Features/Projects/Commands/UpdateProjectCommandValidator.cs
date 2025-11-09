using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.Projects.Commands
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required for updating the project.");

            RuleFor(x => x.ProjectCode)
                .MaximumLength(50).WithMessage("Project code cannot exceed 50 characters")
                .Must(code => code?.ToLower() != "string")
                .WithMessage("Project Code cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.ProjectCode));

            RuleFor(x => x.ProjectName)
                .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters")
                .MinimumLength(3).WithMessage("Project name must be at least 3 characters")
                .Must(name => name?.ToLower() != "string")
                .WithMessage("Project Name cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.ProjectName));

            RuleFor(x => x.ClientName)
                .MaximumLength(200).WithMessage("Client name cannot exceed 200 characters")
                .Must(n => n?.ToLower() != "string").WithMessage("Client Name cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.ClientName));

            RuleFor(x => x.Location)
                .MaximumLength(200).WithMessage("Location cannot exceed 200 characters")
                .Must(l => l?.ToLower() != "string").WithMessage("Location cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.Location));

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .Must(d => d?.ToLower() != "string").WithMessage("Description cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Status)
                .Must(status => status.HasValue && Enum.IsDefined(typeof(ProjectStatusEnum), status.Value))
                .WithMessage("Invalid project status value.")
                .When(x => x.Status.HasValue);

            RuleFor(x => x.ActualEndDate)
                .NotEmpty()
                .WithMessage("Actual end date is required when project status is Completed")
                .When(x => x.Status.HasValue && (ProjectStatusEnum)x.Status.Value == ProjectStatusEnum.Completed);

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(DateTime.Now.AddYears(1)).WithMessage("Start date cannot be more than 1 year in the future")
                .When(x => x.StartDate.HasValue);

            RuleFor(x => x.PlannedEndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("Planned end date must be after start date")
                .When(x => x.PlannedEndDate.HasValue && x.StartDate.HasValue);

            RuleFor(x => x.ActualEndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("Actual end date must be after start date")
                .When(x => x.ActualEndDate.HasValue && x.StartDate.HasValue);
        }
    }
}
