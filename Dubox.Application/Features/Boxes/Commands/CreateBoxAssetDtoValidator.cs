using Dubox.Application.DTOs;
using FluentValidation;

namespace Dubox.Application.Features.Boxes.Commands
{
    public class CreateBoxAssetDtoValidator : AbstractValidator<CreateBoxAssetDto>
    {
        public CreateBoxAssetDtoValidator()
        {
            RuleFor(x => x.AssetType)
            .NotEmpty().WithMessage("AssetType is required.")
            .MaximumLength(100).WithMessage("AssetType must not exceed 100 characters.");

            RuleFor(x => x.AssetCode)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.AssetCode))
                .WithMessage("AssetCode must not exceed 100 characters.");

            RuleFor(x => x.AssetName)
                .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.AssetName))
                .WithMessage("AssetName must not exceed 200 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Unit)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Unit))
                .WithMessage("Unit must not exceed 100 characters.");

            RuleFor(x => x.Specifications)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Specifications))
                .WithMessage("Specifications must not exceed 500 characters.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes must not exceed 500 characters.");
        }
    }

}
