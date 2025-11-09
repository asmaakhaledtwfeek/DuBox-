using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.Boxes.Commands
{
    public class UpdateBoxCommandValidator : AbstractValidator<UpdateBoxCommand>
    {
        public UpdateBoxCommandValidator()
        {
            RuleFor(x => x.BoxId)
                .NotEmpty().WithMessage("Box ID is required for updating the box.");

            RuleFor(x => x.BoxTag)
                .MaximumLength(100).WithMessage("Box tag cannot exceed 100 characters")
                .Must(tag => tag?.ToLower() != "string").WithMessage("Box tag cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.BoxTag));

            RuleFor(x => x.BoxName)
                .MaximumLength(200).WithMessage("Box name cannot exceed 200 characters")
                .Must(name => name?.ToLower() != "string").WithMessage("Box name cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.BoxName));

            RuleFor(x => x.BoxType)
                .MaximumLength(100).WithMessage("Box type cannot exceed 100 characters")
                .Must(type => type?.ToLower() != "string").WithMessage("Box type cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.BoxType));

            RuleFor(x => x.Floor)
                .MaximumLength(50).WithMessage("Floor cannot exceed 50 characters")
                .Must(floor => floor?.ToLower() != "string").WithMessage("Floor cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.Floor));

            RuleFor(x => x.Building)
                .MaximumLength(100).WithMessage("Building cannot exceed 100 characters")
                .Must(b => b?.ToLower() != "string").WithMessage("Building cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.Building));

            RuleFor(x => x.Zone)
                .MaximumLength(100).WithMessage("Zone cannot exceed 100 characters")
                .Must(z => z?.ToLower() != "string").WithMessage("Zone cannot be the default value 'string'.")
                .When(x => !string.IsNullOrEmpty(x.Zone));

            RuleFor(x => x.Length)
                .GreaterThan(0).WithMessage("Length must be greater than 0").When(x => x.Length.HasValue);
            RuleFor(x => x.Width)
                .GreaterThan(0).WithMessage("Width must be greater than 0").When(x => x.Width.HasValue);
            RuleFor(x => x.Height)
                .GreaterThan(0).WithMessage("Height must be greater than 0").When(x => x.Height.HasValue);

            RuleFor(x => x.Status)
                .Must(status => status.HasValue && Enum.IsDefined(typeof(BoxStatusEnum), status.Value))
                .WithMessage("Invalid status value.")
                .When(x => x.Status.HasValue);

            RuleFor(x => x.PlannedEndDate)
                .LessThanOrEqualTo(DateTime.Now.AddYears(1)).WithMessage("Planned end date cannot be more than 1 year in the future")
                .When(x => x.PlannedEndDate.HasValue);
        }
    }
}
