using FluentValidation;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public class AddChecklistItemsCommandValidator : AbstractValidator<AddChecklistItemsCommand>
    {
        public AddChecklistItemsCommandValidator()
        {
            RuleFor(x => x.WIRId)
                .NotEmpty()
                .WithMessage("WIRId is required.");

            RuleFor(x => x.PredefinedItemIds)
                .NotNull()
                .WithMessage("Predefined Item IDs list is required.")
                .NotEmpty()
                .WithMessage("At least one predefined item ID must be provided.");

            RuleForEach(x => x.PredefinedItemIds)
                .NotEmpty()
                .WithMessage("Predefined Item ID cannot be empty.");

            RuleFor(x => x.PredefinedItemIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Predefined Item IDs must be unique.");
        }
    }
}
