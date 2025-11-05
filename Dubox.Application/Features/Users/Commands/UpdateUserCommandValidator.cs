using FluentValidation;

namespace Dubox.Application.Features.Users.Commands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required for updating the user.");

            RuleFor(x => x.Email)
             .Must(e => e?.ToLower() != "string").WithMessage("Email cannot be the default value 'string'.")
             .When(x => !string.IsNullOrEmpty(x.Email))
             .NotEmpty().WithMessage("Email address cannot be empty if sent.")
             .EmailAddress().WithMessage("A valid email address is required.")
             .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

            RuleFor(x => x.FullName)
             .Must(n => n?.ToLower() != "string").WithMessage("Full Name cannot be the default value 'string'.")
             .When(x => !string.IsNullOrEmpty(x.FullName))
             .MaximumLength(200).WithMessage("Full Name must not exceed 150 characters.");


        }

    }
}
