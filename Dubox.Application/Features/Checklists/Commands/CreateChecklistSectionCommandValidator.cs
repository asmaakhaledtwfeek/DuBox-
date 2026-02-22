using FluentValidation;

namespace Dubox.Application.Features.Checklists.Commands;

public class CreateChecklistSectionCommandValidator : AbstractValidator<CreateChecklistSectionCommand>
{
    public CreateChecklistSectionCommandValidator()
    {
        RuleFor(x => x.ChecklistId)
            .NotEmpty()
            .WithMessage("Checklist ID is required");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Section title is required")
            .MaximumLength(200)
            .WithMessage("Section title must not exceed 200 characters");

        RuleFor(x => x.Order)
            .GreaterThan(0)
            .WithMessage("Order must be greater than 0");
    }
}
