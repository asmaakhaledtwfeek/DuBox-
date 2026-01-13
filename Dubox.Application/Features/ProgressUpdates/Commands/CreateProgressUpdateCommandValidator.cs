using FluentValidation;

namespace Dubox.Application.Features.ProgressUpdates.Commands
{
    public class CreateProgressUpdateCommandValidator : AbstractValidator<CreateProgressUpdateCommand>
    {
        public CreateProgressUpdateCommandValidator()
        {
            RuleFor(x => x.BoxId)
                .NotEmpty().WithMessage("Box ID is required.");

            RuleFor(x => x.BoxActivityId)
                .NotEmpty().WithMessage("Box Activity ID is required.");

            RuleFor(x => x.ProgressPercentage)
                .NotNull().WithMessage("Progress Percentage is required.")
                .Must(p => p >= 0).WithMessage("Progress Percentage cannot be less than 0.")
                .Must(p => p <= 100).WithMessage("Progress Percentage cannot exceed 100.");

            RuleFor(x => x.WorkDescription)
                .MaximumLength(1000).WithMessage("Work Description cannot exceed 1000 characters.");

            RuleFor(x => x.UpdateMethod)
                .NotEmpty().WithMessage("Update Method is required.");

            // Validate files if provided
            When(x => x.Files != null && x.Files.Count > 0, () =>
            {
                RuleForEach(x => x.Files!)
                    .Must(file => file != null && file.Length > 0)
                    .WithMessage("File cannot be empty.")
                    .Must(file => file != null && file.Length <= 10_485_760)
                    .WithMessage("Each file size cannot exceed 10 MB.");

                RuleFor(x => x.Files!)
                    .Must(files => files.Count <= 10)
                    .WithMessage("Maximum 10 files allowed.");
            });

            // Validate image URLs if provided
            When(x => x.ImageUrls != null && x.ImageUrls.Count > 0, () =>
            {
                RuleForEach(x => x.ImageUrls!)
                    .Must(url => !string.IsNullOrWhiteSpace(url))
                    .WithMessage("Image URL cannot be empty.")
                    .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("Image URL must be a valid URL.");

                RuleFor(x => x.ImageUrls!)
                    .Must(urls => urls.Count <= 10)
                    .WithMessage("Maximum 10 image URLs allowed.");
            });

            // Ensure at least one image source if any provided
            RuleFor(x => x)
                .Must(cmd =>
                    (cmd.Files == null || cmd.Files.Count == 0) &&
                    (cmd.ImageUrls == null || cmd.ImageUrls.Count == 0) ||
                    (cmd.Files != null && cmd.Files.Count > 0) ||
                    (cmd.ImageUrls != null && cmd.ImageUrls.Count > 0))
                .WithMessage("At least one file or image URL must be provided if images are specified.");

        }
    }
}
