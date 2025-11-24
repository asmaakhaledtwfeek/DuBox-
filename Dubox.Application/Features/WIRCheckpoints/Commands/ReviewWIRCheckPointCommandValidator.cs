namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    using FluentValidation;

    public class ReviewWIRCheckPointCommandValidator : AbstractValidator<ReviewWIRCheckPointCommand>
    {
        public ReviewWIRCheckPointCommandValidator()
        {
            RuleFor(x => x.WIRId)
                .NotEmpty()
                .WithMessage("WIRId is required.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid WIRCheckpoint status.");

            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Checklist items are required.")
                .NotEmpty()
                .WithMessage("Checklist items cannot be empty.");

            RuleFor(x => x.InspectorRole)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.InspectorRole))
                .WithMessage("Inspector role cannot exceed 100 characters.");

            RuleForEach(x => x.Items)
                .SetValidator(new ChecklistItemForReviewValidator());
        }
    }

    public class ChecklistItemForReviewValidator : AbstractValidator<ChecklistItemForReview>
    {
        public ChecklistItemForReviewValidator()
        {
            RuleFor(x => x.ChecklistItemId)
                .NotEmpty()
                .WithMessage("ChecklistItemId is required.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid status for checklist item.");

            RuleFor(x => x.Remarks)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
                .WithMessage("Remarks cannot exceed 500 characters.");
        }
    }

}
