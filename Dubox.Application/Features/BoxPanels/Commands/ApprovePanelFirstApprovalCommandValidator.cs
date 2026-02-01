using FluentValidation;
using Dubox.Application.Validators;

namespace Dubox.Application.Features.BoxPanels.Commands
{
    public class ApprovePanelFirstApprovalCommandValidator : AbstractValidator<ApprovePanelFirstApprovalCommand>
    {
        public ApprovePanelFirstApprovalCommandValidator()
        {
            RuleFor(x => x.BoxPanelId)
                .NotEmpty().WithMessage("Box Panel ID is required.");

            RuleFor(x => x.ApprovalStatus)
                .NotEmpty().WithMessage("Approval status is required.")
                .Must(status => status == "Approved" || status == "Rejected")
                .WithMessage("Approval status must be either 'Approved' or 'Rejected'.");

            RuleFor(x => x.Notes)
                .MustBeMeaningfulTextWhenProvided();
        }
    }
}
