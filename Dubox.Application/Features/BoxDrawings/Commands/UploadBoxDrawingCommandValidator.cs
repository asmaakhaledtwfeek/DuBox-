using FluentValidation;

namespace Dubox.Application.Features.BoxDrawings.Commands;

public class UploadBoxDrawingCommandValidator : AbstractValidator<UploadBoxDrawingCommand>
{
    private static readonly string[] AllowedExtensions = { ".pdf", ".dwg" };
    private const long MaxFileSize = 50 * 1024 * 1024; // 50 MB

    public UploadBoxDrawingCommandValidator()
    {
        RuleFor(x => x.BoxId)
            .NotEmpty()
            .WithMessage("Box ID is required.");

        // Either DrawingUrl or File must be provided
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.DrawingUrl) || (x.File != null && x.File.Length > 0))
            .WithMessage("Either a drawing URL or a file must be provided.");

        // Validate file if provided
        When(x => x.File != null && x.File.Length > 0, () =>
        {
            RuleFor(x => x.File!)
                .Must(file => file.Length <= MaxFileSize)
                .WithMessage($"File size cannot exceed {MaxFileSize / (1024 * 1024)} MB.");

            RuleFor(x => x.FileName)
                .NotEmpty()
                .WithMessage("File name is required when uploading a file.")
                .Must(fileName => IsValidFileExtension(fileName))
                .WithMessage($"Only {string.Join(", ", AllowedExtensions)} files are allowed.");
        });

        // Validate URL if provided
        When(x => !string.IsNullOrWhiteSpace(x.DrawingUrl), () =>
        {
            RuleFor(x => x.DrawingUrl!)
                .MaximumLength(1000)
                .WithMessage("Drawing URL cannot exceed 1000 characters.");
        });
    }

    private static bool IsValidFileExtension(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return false;

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return AllowedExtensions.Contains(extension);
    }
}

