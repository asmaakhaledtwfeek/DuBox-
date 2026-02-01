using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using FluentValidation;

namespace Dubox.Application.Features.Boxes.Commands
{

    public class UpdateBoxCommandValidator : AbstractValidator<UpdateBoxCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBoxCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.BoxId)
                .NotEmpty().WithMessage("Box ID is required for updating.");

            RuleFor(x => x.BoxTag)
                .MaximumLength(50).WithMessage("Box tag cannot exceed 50 characters.")
                .Must(t => t?.ToLower() != "string").WithMessage("Box Tag cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.BoxTag));

            RuleFor(x => x)
                .CustomAsync(PreCheckAndRunScheduleValidations)
                .When(x => x.PlannedStartDate.HasValue || x.Duration.HasValue);
        }

        private async Task PreCheckAndRunScheduleValidations(UpdateBoxCommand command, ValidationContext<UpdateBoxCommand> context, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(command.BoxId, cancellationToken);

            if (box == null)
            {
                context.AddFailure("BoxId", "The specified box was not found.");
                return;
            }


            //if (!CannotUpdatePlannedStartDateIfActualExists(command, box))
            //{
            //    context.AddFailure("PlannedStartDate", "Cannot modify the planned start date because the box has an actual start date and work has commenced.");
            //    return;
            //}

            await IsScheduleValidAsync(command, box, context, cancellationToken);

        }

        private bool CannotUpdatePlannedStartDateIfActualExists(UpdateBoxCommand command, Box box)
        {
            if (box.ActualStartDate.HasValue && command.PlannedStartDate.HasValue && box.PlannedStartDate.HasValue)
                return false;
            return true;
        }

        private async Task IsScheduleValidAsync(UpdateBoxCommand command, Box box, ValidationContext<UpdateBoxCommand> context, CancellationToken cancellationToken)
        {
            var newStartDate = command.PlannedStartDate ?? box.PlannedStartDate;
            var newDuration = command.Duration ?? box.Duration;

            if (!newStartDate.HasValue || !newDuration.HasValue || newDuration.Value <= 0)
                return;

            var newPlannedEndDate = newStartDate.Value.AddDays(newDuration.Value);

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(box.ProjectId, cancellationToken);

            if (project == null || !project.PlannedStartDate.HasValue || !project.PlannedEndDate.HasValue)
                return;

            if (newStartDate.Value < project.PlannedStartDate.Value || newPlannedEndDate > project.PlannedEndDate.Value)
                context.AddFailure("PlannedStartDate", $"Box schedule must fall within the project's planned dates ({project.PlannedStartDate.Value:d} to {project.PlannedEndDate.Value:d}).");

            var activities = await _unitOfWork.Repository<BoxActivity>().FindAsync(
                a => a.BoxId == command.BoxId && a.PlannedStartDate.HasValue && a.PlannedEndDate.HasValue,
                cancellationToken
            );

            if (activities.Any())
            {
                var minActivityStartDate = activities.Min(a => a.PlannedStartDate.Value);
                var maxActivityEndDate = activities.Max(a => a.PlannedEndDate.Value);

                if (minActivityStartDate < newStartDate.Value || maxActivityEndDate > newPlannedEndDate)
                    context.AddFailure("Duration", "The new box schedule conflicts with the planned dates of its activities. Adjust activities first.");

            }

        }
    }
}
