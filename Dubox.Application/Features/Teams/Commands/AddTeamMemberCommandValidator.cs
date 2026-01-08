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

        // Email and TemporaryPassword is required only if IsCreateAccount is true
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required when creating an account.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
            .When(x => x.IsCreateAccount);

        RuleFor(x => x.TemporaryPassword)
            .NotEmpty().WithMessage("Temporary Password is required when creating an account.")
             .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(50).WithMessage("Password must not exceed 15 characters.")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
            .When(x => x.IsCreateAccount);
    }
}

