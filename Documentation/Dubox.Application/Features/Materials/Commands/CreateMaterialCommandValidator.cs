using FluentValidation;

namespace Dubox.Application.Features.Materials.Commands
{
    public class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
    {
        public CreateMaterialCommandValidator()
        {
            RuleFor(x => x.MaterialCode)
             .NotEmpty().WithMessage("Material Code is required.")
             .MaximumLength(50).WithMessage("Material Code must not exceed 50 characters.");

            RuleFor(x => x.MaterialName)
                .NotEmpty().WithMessage("Material Name is required.")
                .MaximumLength(100).WithMessage("Material Name must not exceed 100 characters.");

            RuleFor(x => x.MaterialCategory)
                .MaximumLength(50).WithMessage("Material Category must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.MaterialCategory));

            RuleFor(x => x.Unit)
                .MaximumLength(20).WithMessage("Unit must not exceed 20 characters.")
                .When(x => !string.IsNullOrEmpty(x.Unit));

            RuleFor(x => x.SupplierName)
                .MaximumLength(100).WithMessage("Supplier Name must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.SupplierName));

            RuleFor(x => x.UnitCost)
                .GreaterThanOrEqualTo(0).WithMessage("Unit Cost must be non-negative.")
                .When(x => x.UnitCost.HasValue);

            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0).WithMessage("Current Stock must be non-negative.")
                .When(x => x.CurrentStock.HasValue);

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum Stock must be non-negative.")
                .When(x => x.MinimumStock.HasValue);

            RuleFor(x => x.ReorderLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Reorder Level must be non-negative.")
                .When(x => x.ReorderLevel.HasValue);

            RuleFor(x => x.MinimumStock)
                .LessThanOrEqualTo(x => x.CurrentStock)
                .WithMessage("Minimum Stock cannot be greater than the Current Stock.")
                .When(x => x.MinimumStock.HasValue && x.CurrentStock.HasValue);
        }
    }
}
