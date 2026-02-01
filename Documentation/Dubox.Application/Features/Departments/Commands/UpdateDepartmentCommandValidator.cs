using FluentValidation;


namespace Dubox.Application.Features.Departments.Commands
{

    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage("Department ID is required for updating the department.");

            RuleFor(x => x.DepartmentName)
                .MaximumLength(100)
                .WithMessage("Department name cannot exceed 100 characters.")
                .MinimumLength(3)
                .WithMessage("Department name must be at least 3 characters long.")
                .Must(name => name?.ToLower() != "string")
                .WithMessage("Department name cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.DepartmentName));

            RuleFor(x => x.Code)
                .MaximumLength(15)
                .WithMessage("Department code cannot exceed 15 characters.")
                .MinimumLength(2)
                .WithMessage("Department code must be at least 2 characters long.")
                .Must(code => code?.ToLower() != "string")
                .WithMessage("Department code cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.Code));

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.")
                .Must(desc => desc?.ToLower() != "string")
                .WithMessage("Description cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Location)
                .MaximumLength(500)
                .WithMessage("Location cannot exceed 500 characters.")
                .Must(loc => loc?.ToLower() != "string")
                .WithMessage("Location cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.Location));

            RuleFor(x => x.ManagerId)
                .Must(BeAValidGuid)
                .WithMessage("Invalid ManagerId format.")
                .When(x => x.ManagerId.HasValue);

        }

        private bool BeAValidGuid(Guid? guid)
        {
            return guid != Guid.Empty;
        }
    }

}
