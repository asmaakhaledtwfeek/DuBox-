using Dubox.Domain.Abstraction;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public class CreateActivityCheckListItemCommandValidator : AbstractValidator<CreateActivityCheckListItemCommand>
{
    private readonly IDbContext _context;

    public CreateActivityCheckListItemCommandValidator(IDbContext context)
    {
        _context = context;

        RuleFor(x => x.ActivityMasterId)
            .NotEmpty()
            .When(x => x.ActivityTemplateActivityId == null)
            .WithMessage("Either ActivityMasterId or ActivityTemplateActivityId must be provided");

        RuleFor(x => x.ActivityTemplateActivityId)
            .NotEmpty()
            .When(x => x.ActivityMasterId == null)
            .WithMessage("Either ActivityMasterId or ActivityTemplateActivityId must be provided");

        RuleFor(x => x.PredefinedChecklistItemId)
            .NotEmpty()
            .WithMessage("PredefinedChecklistItemId is required");

        RuleFor(x => x.Sequence)
            .GreaterThan(0)
            .WithMessage("Sequence must be greater than 0")
            .MustAsync(async (command, sequence, cancellation) => 
                await BeUniqueSequenceInActivity(
                    command.ActivityMasterId, 
                    command.ActivityTemplateActivityId, 
                    sequence, 
                    cancellation))
            .WithMessage("An item with this sequence number already exists for this activity. Please use a different sequence number.");
    }

    private async Task<bool> BeUniqueSequenceInActivity(
        Guid? activityMasterId, 
        Guid? activityTemplateActivityId, 
        int sequence, 
        CancellationToken cancellationToken)
    {
        if (activityMasterId.HasValue)
        {
            var exists = await _context.ActivityCheckListItems
                .AnyAsync(item => 
                    item.ActivityMasterId == activityMasterId && 
                    item.Sequence == sequence &&
                    item.IsActive, 
                    cancellationToken);

            return !exists;
        }
        
        if (activityTemplateActivityId.HasValue)
        {
            var exists = await _context.ActivityCheckListItems
                .AnyAsync(item => 
                    item.ActivityTemplateActivityId == activityTemplateActivityId && 
                    item.Sequence == sequence &&
                    item.IsActive, 
                    cancellationToken);

            return !exists;
        }

        return true;
    }
}
