using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Checklists.Commands;

public class CreateChecklistItemCommandValidator : AbstractValidator<CreateChecklistItemCommand>
{
    private readonly IDbContext _context;

    public CreateChecklistItemCommandValidator(IDbContext context)
    {
        _context = context;

        RuleFor(x => x.ChecklistSectionId)
            .NotEmpty()
            .WithMessage("Checklist section ID is required");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Item description is required")
            .MaximumLength(500)
            .WithMessage("Item description must not exceed 500 characters");

        RuleFor(x => x.Sequence)
            .GreaterThan(0)
            .WithMessage("Sequence must be greater than 0")
            .MustAsync(async (command, sequence, cancellation) => 
                await BeUniqueSequenceInSection(command.ChecklistSectionId, sequence, cancellation))
            .WithMessage("An item with this sequence number already exists in this section. Please use a different sequence number.");

        RuleFor(x => x.Reference)
            .MaximumLength(200)
            .WithMessage("Reference must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Reference));
    }

    private async Task<bool> BeUniqueSequenceInSection(Guid sectionId, int sequence, CancellationToken cancellationToken)
    {
        var exists = await _context.PredefinedChecklistItems
            .AnyAsync(item => 
                item.ChecklistSectionId == sectionId && 
                item.Sequence == sequence &&
                item.IsActive, 
                cancellationToken);

        return !exists;
    }
}
