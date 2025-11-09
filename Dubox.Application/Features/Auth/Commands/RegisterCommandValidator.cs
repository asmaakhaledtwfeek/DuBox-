using FluentValidation;

namespace Dubox.Application.Features.Auth.Commands
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {


        public RegisterCommandValidator()
        {

            RuleFor(x => x.Email)
             .NotEmpty().WithMessage("Email address is required.")
             .EmailAddress().WithMessage("A valid email address is required.")
             .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(50).WithMessage("Password must not exceed 15 characters.")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.");
            RuleFor(x => x.FullName)
           .NotEmpty().WithMessage("Full Name address is required.");
        }

    }
}
