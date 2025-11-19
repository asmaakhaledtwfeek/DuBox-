using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using FluentValidation;

namespace Dubox.Application.Features.Boxes.Commands
{
    public class CreateBoxCommandValidator : AbstractValidator<CreateBoxCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateBoxCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");
            RuleFor(x => x.BoxTag)
            .NotEmpty()
            .WithMessage("Box tag is required")
            .MaximumLength(100)
            .WithMessage("Box tag cannot exceed 100 characters");
            //.Matches(@"^[a-zA-Z0-9-_]+$")
            //.WithMessage("Box tag can only contain letters, numbers, hyphens and underscores");
            RuleFor(x => x.BoxName)
            .MaximumLength(200)
            .WithMessage("Box name cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.BoxName));

            RuleFor(x => x.BoxType)
            .NotEmpty()
            .WithMessage("Box type is required")
            .MaximumLength(100)
            .WithMessage("Box type cannot exceed 100 characters");
            //.Must(BeValidBoxType)
            //.WithMessage("Invalid box type. Valid types: Bedroom, Living Room, Kitchen, Bathroom, Office, Storage, Other");
            RuleFor(x => x.Floor)
            .NotEmpty()
            .WithMessage("Floor is required")
            .MaximumLength(50)
            .WithMessage("Floor cannot exceed 50 characters");

            RuleFor(x => x.Building)
            .MaximumLength(100)
            .WithMessage("Building cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Building));

            RuleFor(x => x.Zone)
            .MaximumLength(100)
            .WithMessage("Zone cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Zone));

            RuleFor(x => x.Length)
           .GreaterThan(0)
           .WithMessage("Length must be greater than 0")
           .When(x => x.Length.HasValue);

            RuleFor(x => x.Width)
                .GreaterThan(0)
                .WithMessage("Width must be greater than 0")
                .When(x => x.Width.HasValue);

            RuleFor(x => x.Height)
                .GreaterThan(0)
                .WithMessage("Height must be greater than 0")
                .When(x => x.Height.HasValue);

            RuleFor(x => x.BIMModelReference)
                .MaximumLength(200)
                .WithMessage("BIM model reference cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.BIMModelReference));

            RuleFor(x => x.RevitElementId)
                .MaximumLength(100)
                .WithMessage("Revit element ID cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.RevitElementId));
            RuleForEach(x => x.Assets)
                .SetValidator(new CreateBoxAssetDtoValidator())
                .When(x => x.Assets != null && x.Assets.Any());

            RuleFor(x => x)
            .MustAsync(BeWithinProjectSchedule)
            .WithMessage("Box schedule must fall within the project's planned start and end dates.")
            .When(x => x.BoxPlannedStartDate.HasValue && x.BoxDuration.HasValue);
        }

        private async Task<bool> BeWithinProjectSchedule(CreateBoxCommand command, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(command.ProjectId, cancellationToken);

            if (project == null) return false;

            if (!project.PlannedStartDate.HasValue || !project.PlannedEndDate.HasValue)
                return true;

            var boxPlannedEndDate = command.BoxPlannedStartDate!.Value.AddDays(command.BoxDuration!.Value);

            bool startsAfterProjectStart = command.BoxPlannedStartDate.Value >= project.PlannedStartDate.Value;
            bool endsBeforeProjectEnd = boxPlannedEndDate <= project.PlannedEndDate.Value;

            return startsAfterProjectStart && endsBeforeProjectEnd;
        }
    }
}
