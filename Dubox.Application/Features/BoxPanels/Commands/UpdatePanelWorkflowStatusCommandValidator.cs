using FluentValidation;

namespace Dubox.Application.Features.BoxPanels.Commands;

public class UpdatePanelWorkflowStatusCommandValidator : AbstractValidator<UpdatePanelWorkflowStatusCommand>
{
    private const int NotesMinimumLength = 50;

    public UpdatePanelWorkflowStatusCommandValidator()
    {
        RuleFor(x => x.BoxPanelId)
            .NotEmpty().WithMessage("Box Panel ID is required.");

        RuleFor(x => x.WorkflowStatus)
            .NotEmpty().WithMessage("Workflow status is required.")
            .Must(status => status == "InProgress" || status == "Completed" || status == "PutOnHold")
            .WithMessage("Workflow status must be one of: InProgress, Completed, PutOnHold.");

        
    }
}
