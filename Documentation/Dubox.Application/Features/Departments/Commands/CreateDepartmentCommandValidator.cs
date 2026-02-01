namespace Dubox.Application.Features.Departments.Commands
{
    using FluentValidation;

    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(x => x.DepartmentName)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Department code is required.")
                .MaximumLength(15).WithMessage("Department code must not exceed 15 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.Location)
                .MaximumLength(500).WithMessage("Location must not exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Location));

            RuleFor(x => x.ManagerId)
                .Must(BeAValidGuid)
                .When(x => x.ManagerId.HasValue)
                .WithMessage("Invalid ManagerId format.");

        }

        private bool BeAValidGuid(Guid? guid)
        {
            return guid != Guid.Empty;
        }
    }

}
