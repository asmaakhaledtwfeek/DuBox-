using FluentValidation;

namespace Dubox.Application.Features.Teams.Commands;

public class AddTeamMemberCommandValidator : AbstractValidator<AddTeamMemberCommand>
{
    public AddTeamMemberCommandValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty().WithMessage("Team ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Team ID cannot be an empty GUID.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(100).WithMessage("First Name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(100).WithMessage("Last Name must not exceed 100 characters.");

        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("Employee Code is required.")
            .MaximumLength(50).WithMessage("Employee Code must not exceed 50 characters.");

        // Email is required only if IsCreateAccount is true
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required when creating an account.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
            .When(x => x.IsCreateAccount);

        // TemporaryPassword is required only if IsCreateAccount is true
        RuleFor(x => x.TemporaryPassword)
            .NotEmpty().WithMessage("Temporary Password is required when creating an account.")
            .MinimumLength(6).WithMessage("Temporary Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Temporary Password must not exceed 100 characters.")
            .When(x => x.IsCreateAccount);
    }
}

