using FluentValidation;

namespace Dubox.Application.Features.Teams.Commands;

public class AddMembersToGroupCommandValidator : AbstractValidator<AddMembersToGroupCommand>
{
    public AddMembersToGroupCommandValidator()
    {
        RuleFor(x => x.TeamGroupId)
            .NotEmpty()
            .WithMessage("Team Group ID is required");

        RuleFor(x => x.TeamMemberIds)
            .NotNull()
            .WithMessage("Team Member IDs are required")
            .Must(ids => ids != null && ids.Any())
            .WithMessage("At least one team member must be specified");

        RuleForEach(x => x.TeamMemberIds)
            .NotEmpty()
            .WithMessage("Team Member ID cannot be empty");
    }
}

