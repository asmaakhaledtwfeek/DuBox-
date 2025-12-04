using FluentValidation;

namespace Dubox.Application.Features.ProgressUpdates.Commands
{
    public class CreateProgressUpdateCommandValidator : AbstractValidator<CreateProgressUpdateCommand>
    {
        public CreateProgressUpdateCommandValidator()
        {
            RuleFor(x => x.BoxId)
                .NotEmpty().WithMessage("Box ID is required.");

            RuleFor(x => x.BoxActivityId)
                .NotEmpty().WithMessage("Box Activity ID is required.");

            RuleFor(x => x.ProgressPercentage)
                .NotNull().WithMessage("Progress Percentage is required.")
                .Must(p => p >= 0).WithMessage("Progress Percentage cannot be less than 0.")
                .Must(p => p <= 100).WithMessage("Progress Percentage cannot exceed 100.");

            RuleFor(x => x.WorkDescription)
                .MaximumLength(1000).WithMessage("Work Description cannot exceed 1000 characters.");

            RuleFor(x => x.UpdateMethod)
                .NotEmpty().WithMessage("Update Method is required.");

            // Validate file if provided
            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File!.Length)
                    .GreaterThan(0).WithMessage("File cannot be empty.")
                    .LessThanOrEqualTo(10_485_760).WithMessage("File size cannot exceed 10 MB.");
            });

        }
    }
}
