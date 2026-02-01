using FluentValidation;
using Dubox.Application.Validators;

namespace Dubox.Application.Features.BoxPanels.Commands
{
    public class ApprovePanelSecondApprovalCommandValidator : AbstractValidator<ApprovePanelSecondApprovalCommand>
    {
        public ApprovePanelSecondApprovalCommandValidator()
        {
            RuleFor(x => x.BoxPanelId)
                .NotEmpty().WithMessage("Box Panel ID is required.");

            RuleFor(x => x.ApprovalStatus)
                .NotEmpty().WithMessage("Approval status is required.")
                .Must(status => status == "Approved" || status == "Rejected" || status == "ConditionalApproval")
                .WithMessage("Approval status must be 'Approved', 'Rejected', or 'ConditionalApproval'.");

            RuleFor(x => x.Notes)
                .MustBeMeaningfulTextWhenProvided();
        }
    }
}
