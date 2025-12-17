using FluentValidation;

namespace Dubox.Application.Features.Teams.Commands;

public class CreateTeamGroupCommandValidator : AbstractValidator<CreateTeamGroupCommand>
{
    public CreateTeamGroupCommandValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty().WithMessage("Team ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Team ID cannot be an empty GUID.");

        RuleFor(x => x.GroupTag)
            .NotEmpty().WithMessage("Group Tag is required.")
            .MaximumLength(50).WithMessage("Group Tag must not exceed 50 characters.");

        RuleFor(x => x.GroupType)
            .NotEmpty().WithMessage("Group Type is required.")
            .MaximumLength(100).WithMessage("Group Type must not exceed 100 characters.");
    }
}

