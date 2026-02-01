using FluentValidation;

namespace Dubox.Application.Features.Materials.Commands
{
    public class UpdateMaterialCommandValidator : AbstractValidator<UpdateMaterialCommand>
    {
        public UpdateMaterialCommandValidator()
        {

            RuleFor(x => x.MaterialId)
                .NotEmpty()
                .WithMessage("Material ID Requiired.");

            RuleFor(x => x.MaterialCode)
                .MaximumLength(50)
                .WithMessage("Material Code cannot exceed 50 characters.")
                .When(x => x.MaterialCode != null);

            //RuleFor(x => x.MaterialCode)
            //    .MustAsync(BeUniqueCode)
            //    .WithMessage("Material Code already exists.")
            //    .When(x => !string.IsNullOrWhiteSpace(x.MaterialCode));

            RuleFor(x => x.MaterialName)
                .MaximumLength(200)
                .WithMessage("Material Name cannot exceed 200 characters.")
                .When(x => x.MaterialName != null);

            RuleFor(x => x.MaterialCategory)
                .MaximumLength(100)
                .WithMessage("Material Category cannot exceed 100 characters.")
                .When(x => x.MaterialCategory != null);

            RuleFor(x => x.Unit)
                .MaximumLength(50)
                .WithMessage("Unit cannot exceed 50 characters.")
                .When(x => x.Unit != null);

            RuleFor(x => x.UnitCost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Unit Cost must be greater than or equal to 0.")
                .When(x => x.UnitCost.HasValue);

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum Stock must be greater than or equal to 0.")
                .When(x => x.MinimumStock.HasValue);

            RuleFor(x => x.ReorderLevel)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Reorder Level must be greater than or equal to 0.")
                .When(x => x.ReorderLevel.HasValue);

            RuleFor(x => x.SupplierName)
                .MaximumLength(200)
                .WithMessage("Supplier Name cannot exceed 200 characters.")
                .When(x => x.SupplierName != null);
        }

        //private async Task<bool> BeUniqueCode(UpdateMaterialCommand command, string? code, CancellationToken cancellationToken)
        //{
        //    if (string.IsNullOrWhiteSpace(code))
        //        return true;

        //    var spec = new GetMaterialByCodeSpecification(code);
        //    var existingMaterial = await _unitOfWork.Repository<Material>().FindAsync(m=>m.MaterialCode == code);
        //    if (existingMaterial == null)
        //        return true;
        //    return existingMaterial.Id == command.MaterialId;
        //}
    }
}
