using Dubox.Application.Features.Teams.Commands;
using FluentValidation;

public class UpdateTeamMemberCommandValidator : AbstractValidator<ComplateTeamMemberProfileCommand>
{
    public UpdateTeamMemberCommandValidator()
    {
        RuleFor(x => x.TeamMemberId)
            .NotEmpty().WithMessage("TeamMemberId is required.");

        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("Employee code is required.")
            .MaximumLength(50).WithMessage("Employee code must not exceed 50 characters.")
             .Must(type => type?.ToLower() != "string").WithMessage("Employee code cannot be the default value 'string'.");

        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("Employee name is required.")
            .MaximumLength(200).WithMessage("Employee name must not exceed 200 characters.")
             .Must(type => type?.ToLower() != "string").WithMessage("Employee name cannot be the default value 'string'.");

        RuleFor(x => x.MobileNumber)
            .MaximumLength(20).WithMessage("Mobile number must not exceed 20 characters.")
            .Matches(@"^\+?\d*$").When(x => !string.IsNullOrEmpty(x.MobileNumber))
            .WithMessage("Mobile number must contain only digits and optional leading '+'.");
    }
}

