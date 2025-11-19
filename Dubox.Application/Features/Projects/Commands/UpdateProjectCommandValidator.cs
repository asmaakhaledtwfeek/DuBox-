using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using FluentValidation;

namespace Dubox.Application.Features.Projects.Commands
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

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

            RuleFor(x => x)
                .MustAsync(PreCheckAndRunTimeValidation)
                .WithMessage(" project was not found or a severe scheduling conflict occurred.")
                .When(x => x.PlannedStartDate.HasValue || x.Duration.HasValue);
        }

        private async Task<bool> PreCheckAndRunTimeValidation(UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(command.ProjectId, cancellationToken);
            if (project == null) return false;

            if (!CannotUpdatePlannedStartDateIfActualExists(command, project))
                return false;

            if (!await BeValidProjectSchedule(command, project, cancellationToken))
                return false;

            return true;
        }

        private bool CannotUpdatePlannedStartDateIfActualExists(UpdateProjectCommand command, Project project)
        {
            if (project.ActualStartDate.HasValue && command.PlannedStartDate.HasValue)
                return false;
            return true;
        }

        private async Task<bool> BeValidProjectSchedule(UpdateProjectCommand command, Project project, CancellationToken cancellationToken)
        {
            var newStartDate = command.PlannedStartDate ?? project.PlannedStartDate;
            var newDuration = command.Duration ?? project.Duration;

            if (!newStartDate.HasValue || !newDuration.HasValue || newDuration.Value <= 0)
                return true;

            var newPlannedEndDate = newStartDate.Value.AddDays(newDuration.Value);

            var relevantBoxes = await _unitOfWork.Repository<Box>().FindAsync(
                   b => b.ProjectId == command.ProjectId && b.PlannedStartDate.HasValue && b.PlannedEndDate.HasValue,
                   cancellationToken
                   );

            if (!relevantBoxes.Any())
                return true;

            var minBoxStartDate = relevantBoxes.Min(b => b.PlannedStartDate.Value);
            var maxBoxEndDate = relevantBoxes.Max(b => b.PlannedEndDate.Value);

            if (minBoxStartDate < newStartDate.Value || maxBoxEndDate > newPlannedEndDate)
                return false;

            return true;
        }
    }
}