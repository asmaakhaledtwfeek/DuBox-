namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    using FluentValidation;
    using Dubox.Application.Validators;

    public class CreateWIRCheckpointCommandValidator : AbstractValidator<CreateWIRCheckpointCommand>
    {
        public CreateWIRCheckpointCommandValidator()
        {
            RuleFor(x => x.BoxActivityId)
                .NotEmpty().WithMessage("Box Activity ID is required for a WIR Checkpoint.");

            RuleFor(x => x.WIRNumber)
                .NotEmpty().WithMessage("WIR Number is required and cannot be empty.")
                .MaximumLength(20).WithMessage("WIR Number cannot exceed 20 characters.");

            RuleFor(x => x.WIRName)
                .MaximumLength(1000).WithMessage("WIR Name cannot exceed 200 characters.");

            RuleFor(x => x.WIRDescription)
                .MustBeMeaningfulTextWhenProvided()
                .MaximumLength(1000).WithMessage("WIR Description cannot exceed 500 characters.");

            RuleFor(x => x.Comments)
                .MustBeMeaningfulTextWhenProvided()
                .MaximumLength(1000).When(x => x.Comments != null)
                .WithMessage("Comments cannot exceed 1000 characters.");

            RuleFor(x => x.AttachmentPath)
                .MaximumLength(500).When(x => x.AttachmentPath != null)
                .WithMessage("Attachment Path cannot exceed 500 characters.");
        }


    }
}
