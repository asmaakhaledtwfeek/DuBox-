using FluentValidation;

namespace Dubox.Application.Features.Teams.Commands
{
    public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(x => x.TeamCode)
                .NotEmpty().WithMessage("Team Code is required.")
                .MaximumLength(50).WithMessage("Team Code must not exceed 50 characters.");

            RuleFor(x => x.TeamName)
                .NotEmpty().WithMessage("Team Name is required.")
                .MaximumLength(200).WithMessage("Team Name must not exceed 200 characters.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Department ID cannot be an empty GUID.");

            RuleFor(x => x.Trade)
                .MaximumLength(100).WithMessage("Trade name must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Trade))
                ;
        }
    }
}
