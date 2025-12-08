using FluentValidation;

namespace Dubox.Application.Features.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(50).WithMessage("Password must not exceed 50 characters.")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full Name is required.")
            .MaximumLength(200).WithMessage("Full Name must not exceed 200 characters.")
            .Must(n => n?.ToLower() != "string").WithMessage("Full Name cannot be the default value 'string'.");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Department ID cannot be empty.");
        
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive status must be specified.");
    }
}
