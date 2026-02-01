using FluentValidation;
using Dubox.Application.Validators;

namespace Dubox.Application.Features.IssueComments.Commands
{
    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(x => x.IssueId)
                .NotEmpty().WithMessage("Issue ID is required.");

            RuleFor(x => x.CommentText)
                .NotEmpty().WithMessage("Comment text is required.")
                .MustBeMeaningfulText();
        }
    }
}
