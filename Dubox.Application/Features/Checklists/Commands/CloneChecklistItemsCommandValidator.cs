using FluentValidation;

namespace Dubox.Application.Features.Checklists.Commands;

public class CloneChecklistItemsCommandValidator : AbstractValidator<CloneChecklistItemsCommand>
{
    public CloneChecklistItemsCommandValidator()
    {
        RuleFor(x => x.ItemIds)
            .NotEmpty().WithMessage("At least one item must be selected")
            .Must(x => x != null && x.Count > 0).WithMessage("Item IDs list cannot be empty");

        RuleFor(x => x.TargetWIRCode)
            .NotEmpty().WithMessage("Target WIR code is required")
            .MaximumLength(50).WithMessage("WIR code cannot exceed 50 characters");
    }
}
