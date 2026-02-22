using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public class BulkCreateActivityCheckListItemsCommandHandler : IRequestHandler<BulkCreateActivityCheckListItemsCommand, Result<int>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public BulkCreateActivityCheckListItemsCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> Handle(BulkCreateActivityCheckListItemsCommand request, CancellationToken cancellationToken)
    {
        if (request.ActivityMasterId == null && request.ActivityTemplateActivityId == null)
        {
            return Result.Failure<int>(new Error(
                "ActivityCheckListItem.InvalidActivity",
                "Either ActivityMasterId or ActivityTemplateActivityId must be provided."));
        }

        if (request.ActivityMasterId != null && request.ActivityTemplateActivityId != null)
        {
            return Result.Failure<int>(new Error(
                "ActivityCheckListItem.InvalidActivity",
                "Cannot provide both ActivityMasterId and ActivityTemplateActivityId. Only one is allowed."));
        }

        if (request.ActivityMasterId != null)
        {
            var activityMasterExists = await _context.ActivityMasters
                .AnyAsync(am => am.ActivityMasterId == request.ActivityMasterId, cancellationToken);

            if (!activityMasterExists)
            {
                return Result.Failure<int>(new Error(
                    "ActivityCheckListItem.ActivityMasterNotFound",
                    "The specified ActivityMaster does not exist."));
            }
        }

        if (request.ActivityTemplateActivityId != null)
        {
            var activityTemplateActivityExists = await _context.ActivityTemplateActivities
                .AnyAsync(ata => ata.ActivityTemplateActivityId == request.ActivityTemplateActivityId, cancellationToken);

            if (!activityTemplateActivityExists)
            {
                return Result.Failure<int>(new Error(
                    "ActivityCheckListItem.ActivityTemplateActivityNotFound",
                    "The specified ActivityTemplateActivity does not exist."));
            }
        }

        var requestedPredefinedIds = request.ChecklistItems
            .Select(ci => ci.PredefinedChecklistItemId)
            .Distinct()
            .ToList();

        var existingPredefinedItemsCount = await _context.PredefinedChecklistItems
            .CountAsync(pci => requestedPredefinedIds.Contains(pci.PredefinedItemId), cancellationToken);

        if (existingPredefinedItemsCount != requestedPredefinedIds.Count)
        {
            return Result.Failure<int>(new Error(
                "ActivityCheckListItem.PredefinedChecklistItemsNotFound",
                "One or more PredefinedChecklistItem IDs do not exist."));
        }

        var existingItems = await _context.ActivityCheckListItems
            .Where(aci =>
                (request.ActivityMasterId != null && aci.ActivityMasterId == request.ActivityMasterId) ||
                (request.ActivityTemplateActivityId != null && aci.ActivityTemplateActivityId == request.ActivityTemplateActivityId))
            .ToListAsync(cancellationToken);

        var itemsToRemove = existingItems
            .Where(ei => !requestedPredefinedIds.Contains(ei.PredefinedChecklistItemId))
            .ToList();

        if (itemsToRemove.Any())
        {
            _context.ActivityCheckListItems.RemoveRange(itemsToRemove);
        }

        var existingIds = existingItems.Select(ei => ei.PredefinedChecklistItemId).ToList();
        var idsToAdd = requestedPredefinedIds.Except(existingIds).ToList();

        if (idsToAdd.Any())
        {
            var newItems = idsToAdd.Select(id => new ActivityCheckListItem
            {
                ActivityMasterId = request.ActivityMasterId,
                ActivityTemplateActivityId = request.ActivityTemplateActivityId,
                PredefinedChecklistItemId = id,
                IsActive = true,
                CreatedBy = _currentUserService.UserId,
                CreatedDate = DateTime.UtcNow
            }).ToList();

            _context.ActivityCheckListItems.AddRange(newItems);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(requestedPredefinedIds.Count);
    }
}
