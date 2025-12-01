using FluentValidation;

namespace Dubox.Application.Features.Roles.Commands;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required for updating the role.");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100).WithMessage("Role name cannot exceed 100 characters.")
            .MinimumLength(2).WithMessage("Role name must be at least 2 characters long.")
            .Must(name => name?.ToLower() != "string")
            .WithMessage("Role name cannot be the default value 'string'.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .Must(desc => desc?.ToLower() != "string")
            .WithMessage("Description cannot be the default value 'string'.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}


