using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.Boxes.Commands
{
    internal class UpdateBoxCommandValidator : AbstractValidator<UpdateBoxCommand>
    {
        public UpdateBoxCommandValidator()
        {
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

            RuleFor(x => x.Status)
            .Must(status => Enum.IsDefined(typeof(BoxStatusEnum), status))
            .WithMessage("Invalid status value.");

            RuleFor(x => x.PlannedEndDate)
               .LessThanOrEqualTo(DateTime.Now.AddYears(1))
               .WithMessage("Start date cannot be more than 1 year in the future")
               .When(x => x.PlannedEndDate.HasValue);
        }
    }
}
