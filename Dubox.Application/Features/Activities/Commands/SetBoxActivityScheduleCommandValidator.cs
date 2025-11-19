using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using FluentValidation;

namespace Dubox.Application.Features.Activities.Commands
{
    public class SetBoxActivityScheduleCommandValidator : AbstractValidator<SetBoxActivityScheduleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetBoxActivityScheduleCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.ActivityId)
                .NotEmpty().WithMessage("Activity ID is required.");

            RuleFor(x => x.PlannedStartDate)
                .NotEmpty().WithMessage("Planned start date is required.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than zero.");

            RuleFor(x => x)
                .CustomAsync(ValidateActivitySchedule)
                .When(x => x.PlannedStartDate != DateTime.MinValue && x.Duration > 0);
        }

        private async Task ValidateActivitySchedule(SetBoxActivityScheduleCommand command, ValidationContext<SetBoxActivityScheduleCommand> context,
         CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Repository<BoxActivity>().GetByIdAsync(command.ActivityId, cancellationToken);

            if (activity == null)
            {
                context.AddFailure("ActivityId", "Box Activity not found.");
                return;
            }

            await CheckScheduleValidity(command, activity, context, cancellationToken);
        }

        private async Task CheckScheduleValidity(SetBoxActivityScheduleCommand command, BoxActivity activity,
            ValidationContext<SetBoxActivityScheduleCommand> context, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(activity.BoxId, cancellationToken);
            if (box == null)
            {
                context.AddFailure("BoxId", "Parent Box not found for this activity.");
                return;
            }

            if (activity.ActualStartDate.HasValue && activity.PlannedStartDate != command.PlannedStartDate)
            {
                context.AddFailure("PlannedStartDate", "Cannot modify planned start date because actual start date has already been recorded.");
                return;
            }

            if (!box.PlannedStartDate.HasValue || !box.PlannedEndDate.HasValue)
                return;

            var newPlannedEndDate = command.PlannedStartDate.AddDays(command.Duration);

            if (command.PlannedStartDate < box.PlannedStartDate.Value || newPlannedEndDate > box.PlannedEndDate.Value)
            {
                context.AddFailure(
                   "PlannedStartDate",
                   $"Activity schedule must fall within the parent box's planned dates ({box.PlannedStartDate.Value:d} to {box.PlannedEndDate.Value:d})."
               );
            }
        }
    }
}
