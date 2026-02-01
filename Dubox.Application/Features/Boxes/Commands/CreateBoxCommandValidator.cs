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

            RuleFor(x => x.BoxTypeId)
            .NotEmpty()
            .WithMessage("Box type is required");
            
            //.Must(BeValidBoxType)
            //.WithMessage("Invalid box type. Valid types: Bedroom, Living Room, Kitchen, Bathroom, Office, Storage, Other");
            RuleFor(x => x.Floor)
            .NotEmpty()
            .WithMessage("Floor is required")
            .MaximumLength(50)
            .WithMessage("Floor cannot exceed 50 characters");

            RuleFor(x => x.BuildingNumber)
            .MaximumLength(100)
            .WithMessage("BuildingNumber cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.BuildingNumber));

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

            RuleFor(x => x.RevitElementId)
                .MaximumLength(100)
                .WithMessage("Revit Group Element ID cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.RevitElementId));
            RuleForEach(x => x.Assets)
                .SetValidator(new CreateBoxAssetDtoValidator())
                .When(x => x.Assets != null && x.Assets.Any());
            RuleFor(x => x)
                 .CustomAsync(async (command, context, cancellationToken) =>
                  {
                      if (!command.BoxPlannedStartDate.HasValue || !command.BoxDuration.HasValue)
                          return;
                      var result = await ValidateProjectScheduleAsync(command, cancellationToken);
                      if (!result.IsValid)
                          context.AddFailure(result.ErrorMessage!);

                  });

        }

        private async Task<(bool IsValid, string? ErrorMessage)> ValidateProjectScheduleAsync(CreateBoxCommand command, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>()
                .GetByIdAsync(command.ProjectId, cancellationToken);

            if (project == null)
                return (false, "Project not found.");

            if (!project.PlannedStartDate.HasValue || !project.PlannedEndDate.HasValue)
                return (true, null);

            var boxStart = command.BoxPlannedStartDate!.Value;
            var boxEnd = boxStart.AddDays(command.BoxDuration!.Value);

            bool startsAfterProjectStart = boxStart >= project.PlannedStartDate.Value;
            bool endsBeforeProjectEnd = boxEnd <= project.PlannedEndDate.Value;

            if (startsAfterProjectStart && endsBeforeProjectEnd)
                return (true, null);

            var errorMessage =
                $"Box schedule must be within the project range. " +
                $"Project Start: {project.PlannedStartDate:yyyy-MM-dd}, " +
                $"Project End: {project.PlannedEndDate:yyyy-MM-dd}. " +
                $"Your box schedule: {boxStart:yyyy-MM-dd} → {boxEnd:yyyy-MM-dd}.";

            return (false, errorMessage);
        }

    }
}
