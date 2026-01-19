using FluentValidation;
using System.Text.RegularExpressions;

namespace Dubox.Application.Features.Factories.Commands;

public class CreateFactoryCommandValidator : AbstractValidator<CreateFactoryCommand>
{
    public CreateFactoryCommandValidator()
    {
        RuleFor(x => x.FactoryCode)
            .NotEmpty().WithMessage("Factory code is required.")
            .MaximumLength(50).WithMessage("Factory code must not exceed 50 characters.");

        RuleFor(x => x.FactoryName)
            .NotEmpty().WithMessage("Factory name is required.")
            .MaximumLength(200).WithMessage("Factory name must not exceed 200 characters.");

        RuleFor(x => x.Location)
            .IsInEnum().WithMessage("Invalid location value.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
            .When(x => x.Capacity.HasValue);

        // Min Row Validation
        RuleFor(x => x.MinRow)
            .NotEmpty().WithMessage("Min row is required.")
            .GreaterThan(0).WithMessage("Min row must be greater than 0.");

        // Max Row Validation
        RuleFor(x => x.MaxRow)
            .NotEmpty().WithMessage("Max row is required.")
            .GreaterThan(0).WithMessage("Max row must be greater than 0.");

        // Min Bay Validation
        RuleFor(x => x.MinBay)
            .NotEmpty().WithMessage("Min bay is required.")
            .Length(1).WithMessage("Min bay must be exactly one character.")
            .Must(BeASingleLetter).WithMessage("Min bay must be a single letter (A-Z).");

        // Max Bay Validation
        RuleFor(x => x.MaxBay)
            .NotEmpty().WithMessage("Max bay is required.")
            .Length(1).WithMessage("Max bay must be exactly one character.")
            .Must(BeASingleLetter).WithMessage("Max bay must be a single letter (A-Z).");

        // Min Row must be less than Max Row
        RuleFor(x => x)
            .Must(x => x.MinRow < x.MaxRow)
            .WithMessage("Min row must be less than max row.")
            .WithName("MinRow");

        // Min Bay must be alphabetically before Max Bay
        RuleFor(x => x)
            .Must(x => string.Compare(x.MinBay.ToUpper(), x.MaxBay.ToUpper(), StringComparison.Ordinal) < 0)
            .WithMessage("Min bay must be alphabetically before max bay (e.g., A comes before Z).")
            .WithName("MinBay");
    }

    private bool BeASingleLetter(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Regex.IsMatch(value, @"^[a-zA-Z]$");
    }
}


