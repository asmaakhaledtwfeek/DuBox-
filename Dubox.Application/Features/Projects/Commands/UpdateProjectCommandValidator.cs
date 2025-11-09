using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.Projects.Commands
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
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

            RuleFor(x => x.Description)
              .MaximumLength(500)
              .WithMessage("Description cannot exceed 500 characters")
              .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Location)
              .MaximumLength(200)
              .WithMessage("Location cannot exceed 200 characters")
              .When(x => !string.IsNullOrEmpty(x.Location));

            RuleFor(x => x.StartDate)
               .LessThanOrEqualTo(DateTime.Now.AddYears(1))
               .WithMessage("Start date cannot be more than 1 year in the future")
               .When(x => x.StartDate.HasValue);

            RuleFor(x => x.PlannedEndDate)
              .GreaterThan(x => x.StartDate)
              .WithMessage("Planned end date must be after start date")
              .When(x => x.PlannedEndDate.HasValue && x.StartDate.HasValue);

            RuleFor(x => x.ActualEndDate)
               .GreaterThan(x => x.StartDate)
               .WithMessage("Actual end date must be after start date")
               .When(x => x.ActualEndDate.HasValue && x.StartDate.HasValue);
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid project status");
            RuleFor(x => x.ActualEndDate)
            .NotEmpty()
            .WithMessage("Actual end date is required when project status is Completed")
            .When(x => (ProjectStatusEnum)x.Status == ProjectStatusEnum.Completed);
            RuleFor(x => x.Status)
            .Must(status => Enum.IsDefined(typeof(ProjectStatusEnum), status))
            .WithMessage("Invalid status value.");

        }
    }
}
