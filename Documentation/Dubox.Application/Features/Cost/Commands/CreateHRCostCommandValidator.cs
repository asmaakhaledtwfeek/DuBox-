using FluentValidation;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateHRCostCommandValidator : AbstractValidator<CreateHRCostCommand>
{
    public CreateHRCostCommandValidator()
    {
        // Required field
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        // Code (optional)
        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Code));

        // Dropdown fields - Chapter, Sub Chapter, Classification, Sub Classification
        RuleFor(x => x.Chapter)
            .MaximumLength(100).WithMessage("Chapter must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Chapter));

        RuleFor(x => x.SubChapter)
            .MaximumLength(100).WithMessage("Sub Chapter must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SubChapter));

        RuleFor(x => x.Classification)
            .MaximumLength(100).WithMessage("Classification must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Classification));

        RuleFor(x => x.SubClassification)
            .MaximumLength(100).WithMessage("Sub Classification must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SubClassification));

        // Dropdown field - Units
        RuleFor(x => x.Units)
            .MaximumLength(20).WithMessage("Units must not exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.Units));

        // Dropdown field - Type
        RuleFor(x => x.Type)
            .MaximumLength(50).WithMessage("Type must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Type));

        // Dropdown field - Budget Level
        RuleFor(x => x.BudgetLevel)
            .MaximumLength(50).WithMessage("Budget Level must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.BudgetLevel));

        // Dropdown field - Status
        RuleFor(x => x.Status)
            .MaximumLength(20).WithMessage("Status must not exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.Status));

        // Free text fields - Job, Office Account, Job Cost Account, Special Account, IDL Account
        RuleFor(x => x.Job)
            .MaximumLength(100).WithMessage("Job must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Job));

        RuleFor(x => x.OfficeAccount)
            .MaximumLength(100).WithMessage("Office Account must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.OfficeAccount));

        RuleFor(x => x.JobCostAccount)
            .MaximumLength(100).WithMessage("Job Cost Account must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.JobCostAccount));

        RuleFor(x => x.SpecialAccount)
            .MaximumLength(100).WithMessage("Special Account must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SpecialAccount));

        RuleFor(x => x.IDLAccount)
            .MaximumLength(100).WithMessage("IDL Account must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.IDLAccount));
    }
}


