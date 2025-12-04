using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public class UpdateQualityIssueStatusCommandValidator : AbstractValidator<UpdateQualityIssueStatusCommand>
    {
        public UpdateQualityIssueStatusCommandValidator()
        {
            RuleFor(x => x.IssueId)
                .NotEmpty()
                .WithMessage("IssueId is required.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid status value.");

            When(x => x.Status == QualityIssueStatusEnum.Resolved
                   || x.Status == QualityIssueStatusEnum.Closed, () =>
                   {
                       RuleFor(x => x.ResolutionDescription)
                       .NotEmpty()
                       .WithMessage("Resolution description is required when resolving or closing an issue.");
                   });

            RuleFor(x => x.Photo)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Photo))
                .WithMessage("Photo path cannot exceed 500 characters.");

            When(x => x.Status == QualityIssueStatusEnum.InProgress, () =>
            {
                RuleFor(x => x.ResolutionDescription)
                    .Empty()
                    .WithMessage("Resolution description should only be added when resolving or closing an issue.");
            });
        }
    }

}
