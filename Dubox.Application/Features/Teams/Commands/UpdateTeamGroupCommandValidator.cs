using FluentValidation;

namespace Dubox.Application.Features.Teams.Commands;

public class UpdateTeamGroupCommandValidator : AbstractValidator<UpdateTeamGroupCommand>
{
    public UpdateTeamGroupCommandValidator()
    {
        RuleFor(x => x.TeamGroupId)
            .NotEmpty()
            .WithMessage("Team Group ID is required");

        RuleFor(x => x.GroupTag)
            .NotEmpty()
            .WithMessage("Group Tag is required")
            .MaximumLength(50)
            .WithMessage("Group Tag must not exceed 50 characters");

        RuleFor(x => x.GroupType)
            .NotEmpty()
            .WithMessage("Group Type is required")
            .MaximumLength(100)
            .WithMessage("Group Type must not exceed 100 characters");
    }
}

