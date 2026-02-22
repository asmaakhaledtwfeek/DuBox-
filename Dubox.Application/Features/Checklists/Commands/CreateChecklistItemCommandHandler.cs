using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Checklists.Commands;

public class CreateChecklistItemCommandHandler : IRequestHandler<CreateChecklistItemCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _context;

    public CreateChecklistItemCommandHandler(IUnitOfWork unitOfWork, IDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateChecklistItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if section exists
            var section = await _unitOfWork.Repository<ChecklistSection>()
                .GetByIdAsync(request.ChecklistSectionId);

            if (section == null)
            {
                return Result.Failure<Guid>(new Error(
                    "ChecklistSection.NotFound",
                    "Checklist section not found"));
            }

            // Check for duplicate sequence in the same section
            var duplicateExists = await _context.PredefinedChecklistItems
                .AnyAsync(item => 
                    item.ChecklistSectionId == request.ChecklistSectionId && 
                    item.Sequence == request.Sequence &&
                    item.IsActive, 
                    cancellationToken);

            if (duplicateExists)
            {
                return Result.Failure<Guid>(new Error(
                    "ChecklistItem.DuplicateSequence",
                    $"An item with sequence number {request.Sequence} already exists in this section. Please use a different sequence number."));
            }

            // Create new item
            var item = new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.NewGuid(),
                ChecklistSectionId = request.ChecklistSectionId,
                Description = request.Description,
                Sequence = request.Sequence,
                Reference = request.Reference,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Repository<PredefinedChecklistItem>().AddAsync(item);
            await _unitOfWork.CompleteAsync();

            return Result.Success(item.PredefinedItemId);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(new Error(
                "ChecklistItem.CreateFailed",
                $"Failed to create checklist item: {ex.Message}"));
        }
    }
}
