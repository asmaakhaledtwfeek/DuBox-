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

            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Checklist Items list is required.")
                .NotEmpty()
                .WithMessage("Checklist Items list cannot be empty.");

            RuleForEach(x => x.Items)
                .SetValidator(new ChecklistItemForCreateValidator());
            RuleFor(x => x.Items)
          .Must(items => items.Select(i => i.Sequence).Distinct().Count() == items.Count)
          .WithMessage("Sequence values must be unique.");

            RuleFor(x => x.Items)
           .Must(SequencesAreContinuous)
           .WithMessage("Sequence values must be continuous starting from 1.");

        }
        private bool SequencesAreContinuous(List<ChecklistItemForCreate> items)
        {
            if (items == null || !items.Any())
                return true;
            var seqs = items.Select(i => i.Sequence).OrderBy(s => s).ToList();

            for (int i = 0; i < seqs.Count; i++)
            {
                if (seqs[i] != i + 1)
                    return false;
            }
            return true;
        }
    }
    public class ChecklistItemForCreateValidator : AbstractValidator<ChecklistItemForCreate>
    {
        public ChecklistItemForCreateValidator()
        {
            RuleFor(x => x.CheckpointDescription)
                .NotEmpty()
                .WithMessage("Checkpoint description is required.")
                .MaximumLength(500)
                .WithMessage("Checkpoint description cannot exceed 500 characters.");

            RuleFor(x => x.Sequence)
                .GreaterThan(0)
                .WithMessage("Sequence must be greater than 0.");

            RuleFor(x => x.ReferenceDocument)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceDocument))
                .WithMessage("Reference document cannot exceed 200 characters.");
        }
    }

}
