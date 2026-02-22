using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public class CreateActivityCheckListItemCommandHandler : IRequestHandler<CreateActivityCheckListItemCommand, Result<Guid>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateActivityCheckListItemCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateActivityCheckListItemCommand request, CancellationToken cancellationToken)
    {
        // Validate: Must have either ActivityMasterId or ActivityTemplateActivityId, but not both
        if (request.ActivityMasterId == null && request.ActivityTemplateActivityId == null)
        {
            return Result.Failure<Guid>(new Error(
                "ActivityCheckListItem.InvalidActivity",
                "Either ActivityMasterId or ActivityTemplateActivityId must be provided."));
        }

        if (request.ActivityMasterId != null && request.ActivityTemplateActivityId != null)
        {
            return Result.Failure<Guid>(new Error(
                "ActivityCheckListItem.InvalidActivity",
                "Cannot provide both ActivityMasterId and ActivityTemplateActivityId. Only one is allowed."));
        }

        // Validate: Check if ActivityMaster exists
        if (request.ActivityMasterId != null)
        {
            var activityMasterExists = await _context.ActivityMasters
                .AnyAsync(am => am.ActivityMasterId == request.ActivityMasterId, cancellationToken);

            if (!activityMasterExists)
            {
                return Result.Failure<Guid>(new Error(
                    "ActivityCheckListItem.ActivityMasterNotFound",
                    "The specified ActivityMaster does not exist."));
            }
        }

        // Validate: Check if ActivityTemplateActivity exists
        if (request.ActivityTemplateActivityId != null)
        {
            var activityTemplateActivityExists = await _context.ActivityTemplateActivities
                .AnyAsync(ata => ata.ActivityTemplateActivityId == request.ActivityTemplateActivityId, cancellationToken);

            if (!activityTemplateActivityExists)
            {
                return Result.Failure<Guid>(new Error(
                    "ActivityCheckListItem.ActivityTemplateActivityNotFound",
                    "The specified ActivityTemplateActivity does not exist."));
            }
        }

        // Validate: Check if PredefinedChecklistItem exists
        var predefinedItemExists = await _context.PredefinedChecklistItems
            .AnyAsync(pci => pci.PredefinedItemId == request.PredefinedChecklistItemId, cancellationToken);

        if (!predefinedItemExists)
        {
            return Result.Failure<Guid>(new Error(
                "ActivityCheckListItem.PredefinedChecklistItemNotFound",
                "The specified PredefinedChecklistItem does not exist."));
        }

        // Create the checklist item
        var checklistItem = new ActivityCheckListItem
        {
            ActivityMasterId = request.ActivityMasterId,
            ActivityTemplateActivityId = request.ActivityTemplateActivityId,
            PredefinedChecklistItemId = request.PredefinedChecklistItemId,
            Sequence = request.Sequence,
            IsMandatory = request.IsMandatory,
            IsActive = request.IsActive,
            CreatedBy = _currentUserService.UserId,
            CreatedDate = DateTime.UtcNow
        };

        _context.ActivityCheckListItems.Add(checklistItem);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(checklistItem.ActivityCheckListItemId);
    }
}
