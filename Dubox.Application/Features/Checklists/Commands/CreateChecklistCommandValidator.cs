using FluentValidation;

namespace Dubox.Application.Features.Checklists.Commands;

public class CreateChecklistCommandValidator : AbstractValidator<CreateChecklistCommand>
{
    public CreateChecklistCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Checklist name is required")
            .MaximumLength(200).WithMessage("Checklist name cannot exceed 200 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Checklist code is required")
            .MaximumLength(50).WithMessage("Checklist code cannot exceed 50 characters");

        RuleFor(x => x.Discipline)
            .NotEmpty().WithMessage("Discipline is required")
            .MaximumLength(50).WithMessage("Discipline cannot exceed 50 characters");

        RuleFor(x => x.SubDiscipline)
            .MaximumLength(50).WithMessage("Sub-discipline cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.SubDiscipline));

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.WIRCode)
            .MaximumLength(50).WithMessage("WIR code cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.WIRCode));
    }
}
