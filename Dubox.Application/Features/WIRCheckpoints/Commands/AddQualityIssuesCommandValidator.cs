namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    using FluentValidation;

    public class AddQualityIssuesCommandValidator
        : AbstractValidator<AddQualityIssuesCommand>
    {
        public AddQualityIssuesCommandValidator()
        {
            RuleFor(x => x.WIRId)
                .NotEmpty()
                .WithMessage("WIRId is required.");

            RuleFor(x => x.Issues)
                .NotNull()
                .WithMessage("Issues list is required.")
                .NotEmpty()
                .WithMessage("Issues list cannot be empty.");

            RuleForEach(x => x.Issues)
                .SetValidator(new QualityIssueItemValidator());
        }
    }

    public class QualityIssueItemValidator : AbstractValidator<QualityIssueItem>
    {
        public QualityIssueItemValidator()
        {
            RuleFor(x => x.IssueType)
                .IsInEnum()
                .WithMessage("Invalid IssueType.");

            RuleFor(x => x.Severity)
                .IsInEnum()
                .WithMessage("Invalid Severity.");

            RuleFor(x => x.IssueDescription)
                .NotEmpty()
                .WithMessage("IssueDescription is required.")
                .MaximumLength(1000)
                .WithMessage("IssueDescription cannot exceed 1000 characters.");

            RuleFor(x => x.AssignedTo)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.AssignedTo))
                .WithMessage("AssignedTo cannot exceed 200 characters.");

            RuleFor(x => x.PhotoPath)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.PhotoPath))
                .WithMessage("PhotoPath cannot exceed 500 characters.");

            RuleFor(x => x.DueDate)
                .Must(d => !d.HasValue || d.Value.Date >= DateTime.Today)
                .WithMessage("DueDate must be today or in the future.");
        }
    }

}
