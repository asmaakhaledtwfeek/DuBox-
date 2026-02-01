using FluentValidation;
using Dubox.Application.Validators;

namespace Dubox.Application.Features.IssueComments.Commands
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("Comment ID is required.");

            RuleFor(x => x.CommentText)
                .NotEmpty().WithMessage("Comment text is required.")
                .MustBeMeaningfulText();
        }
    }
}
