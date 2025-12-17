using FluentValidation;

namespace Dubox.Application.Features.Teams.Commands;

public class AssignGroupLeaderCommandValidator : AbstractValidator<AssignGroupLeaderCommand>
{
    public AssignGroupLeaderCommandValidator()
    {
        RuleFor(x => x.TeamGroupId)
            .NotEmpty()
            .WithMessage("Team group ID is required");

        RuleFor(x => x.TeamMemberId)
            .NotEmpty()
            .WithMessage("Team member ID is required");
    }
}

