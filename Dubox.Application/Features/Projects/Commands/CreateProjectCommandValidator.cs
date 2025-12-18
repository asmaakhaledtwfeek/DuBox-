using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.Projects.Commands
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.ProjectCode)
            .NotEmpty()
            .WithMessage("Project code is required")
            .MaximumLength(50)
            .WithMessage("Project code cannot exceed 50 characters")
            //.Matches(@"^[a-zA-Z0-9-_]+$")
            //.WithMessage("Project code can only contain letters, numbers, hyphens and underscores")
            ;
            RuleFor(x => x.ProjectName)
            .NotEmpty()
            .WithMessage("Project name is required")
            .MaximumLength(200)
            .WithMessage("Project name cannot exceed 200 characters")
            .MinimumLength(3)
            .WithMessage("Project name must be at least 3 characters");

            RuleFor(x => x.ClientName)
            .MaximumLength(200)
            .WithMessage("Client name cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.ClientName));

            RuleFor(x => x.Location)
                    .NotEmpty().WithMessage("Project location is required.")
                  .Must(location => Enum.IsDefined(typeof(ProjectLocationEnum), location))
                  .WithMessage("Invalid value value.");

            RuleFor(x => x.Duration)
            .GreaterThan(0)
            .WithMessage("Duration must be a positive number")
            .NotEqual(0)
            .WithMessage("Duration is required and must be greater than zero");

            RuleFor(x => x.PlannedStartDate)
            .LessThanOrEqualTo(DateTime.Now.AddYears(1))
            .WithMessage("Planned start date cannot be more than 1 year in the future")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Planned start date cannot be in the past");

            RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
