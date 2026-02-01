using FluentValidation;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public class CreateQualityIssueCommandValidator : AbstractValidator<CreateQualityIssueCommand>
    {
        public CreateQualityIssueCommandValidator()
        {
            RuleFor(x => x.BoxId)
                .NotEmpty().WithMessage("Box ID is required.");

            RuleFor(x => x.IssueType)
                .IsInEnum().WithMessage("Invalid issue type.");

            RuleFor(x => x.Severity)
                .IsInEnum().WithMessage("Invalid severity level.");

            RuleFor(x => x.IssueDescription)
                .NotEmpty().WithMessage("Issue description is required.")
                .MaximumLength(2000).WithMessage("Issue description cannot exceed 2000 characters.");


            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Due date cannot be in the past.")
                .When(x => x.DueDate.HasValue);
        }
    }
}

