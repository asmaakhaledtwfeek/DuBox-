using FluentValidation;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateCostCodeCommandValidator : AbstractValidator<CreateCostCodeCommand>
{
    public CreateCostCodeCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Cost code is required.")
            .MaximumLength(50).WithMessage("Cost code must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.CostCodeLevel1)
            .MaximumLength(50).WithMessage("Cost Code Level 1 must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.CostCodeLevel1));

        RuleFor(x => x.Level1Description)
            .MaximumLength(500).WithMessage("Level 1 Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Level1Description));

        RuleFor(x => x.CostCodeLevel2)
            .MaximumLength(50).WithMessage("Cost Code Level 2 must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.CostCodeLevel2));

        RuleFor(x => x.Level2Description)
            .MaximumLength(500).WithMessage("Level 2 Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Level2Description));

        RuleFor(x => x.CostCodeLevel3)
            .MaximumLength(50).WithMessage("Cost Code Level 3 must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.CostCodeLevel3));

        RuleFor(x => x.Level3DescriptionAbbrev)
            .MaximumLength(50).WithMessage("Level 3 Description Abbreviation must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Level3DescriptionAbbrev));

        RuleFor(x => x.Level3DescriptionAmana)
            .MaximumLength(500).WithMessage("Level 3 Description AMANA must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Level3DescriptionAmana));

        RuleFor(x => x.Category)
            .MaximumLength(100).WithMessage("Category must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Category));

        RuleFor(x => x.UnitOfMeasure)
            .MaximumLength(50).WithMessage("Unit of measure must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.UnitOfMeasure));

        RuleFor(x => x.UnitRate)
            .GreaterThanOrEqualTo(0).WithMessage("Unit rate must be greater than or equal to 0.")
            .When(x => x.UnitRate.HasValue);

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .MaximumLength(10).WithMessage("Currency must not exceed 10 characters.");
    }
}

