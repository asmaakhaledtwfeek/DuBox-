using FluentValidation;

namespace Dubox.Application.Features.Teams.Commands;

public class AssignTeamLeaderCommandValidator : AbstractValidator<AssignTeamLeaderCommand>
{
    public AssignTeamLeaderCommandValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("Team group ID is required");

        RuleFor(x => x.TeamMemberId)
            .NotEmpty()
            .WithMessage("Team member ID is required");
    }
}

