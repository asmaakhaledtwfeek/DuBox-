using FluentValidation;

namespace Dubox.Application.Features.Groups.Commands;

public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
{
    public UpdateGroupCommandValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty().WithMessage("Group ID is required for updating the group.");

        RuleFor(x => x.GroupName)
            .NotEmpty().WithMessage("Group name is required.")
            .MaximumLength(100).WithMessage("Group name cannot exceed 100 characters.")
            .MinimumLength(2).WithMessage("Group name must be at least 2 characters long.")
            .Must(name => name?.ToLower() != "string")
            .WithMessage("Group name cannot be the default value 'string'.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .Must(desc => desc?.ToLower() != "string")
            .WithMessage("Description cannot be the default value 'string'.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}


