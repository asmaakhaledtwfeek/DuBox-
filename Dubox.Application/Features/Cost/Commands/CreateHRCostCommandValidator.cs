using FluentValidation;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateHRCostCommandValidator : AbstractValidator<CreateHRCostCommand>
{
    public CreateHRCostCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Code));

        RuleFor(x => x.Units)
            .MaximumLength(50).WithMessage("Units must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Units));

        RuleFor(x => x.CostType)
            .MaximumLength(100).WithMessage("Cost Type must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.CostType));

        RuleFor(x => x.Trade)
            .MaximumLength(100).WithMessage("Trade must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Trade));

        RuleFor(x => x.Position)
            .MaximumLength(100).WithMessage("Position must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Position));

        RuleFor(x => x.HourlyRate)
            .GreaterThanOrEqualTo(0).WithMessage("Hourly rate must be greater than or equal to 0.")
            .When(x => x.HourlyRate.HasValue);

        RuleFor(x => x.DailyRate)
            .GreaterThanOrEqualTo(0).WithMessage("Daily rate must be greater than or equal to 0.")
            .When(x => x.DailyRate.HasValue);

        RuleFor(x => x.MonthlyRate)
            .GreaterThanOrEqualTo(0).WithMessage("Monthly rate must be greater than or equal to 0.")
            .When(x => x.MonthlyRate.HasValue);

        RuleFor(x => x.OvertimeRate)
            .GreaterThanOrEqualTo(0).WithMessage("Overtime rate must be greater than or equal to 0.")
            .When(x => x.OvertimeRate.HasValue);

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .MaximumLength(10).WithMessage("Currency must not exceed 10 characters.");
    }
}

