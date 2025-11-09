using FluentValidation;

namespace Dubox.Application.Features.Auth.Commands
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .MaximumLength(50).WithMessage("New password must not exceed 50 characters.")
                .Matches(@"[A-Z]+").WithMessage("New password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("New password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("New password must contain at least one number.");

            RuleFor(x => x.NewPassword)
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password must be different from the current password.");
        }
    }
}
