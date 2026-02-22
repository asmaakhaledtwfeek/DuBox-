using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

public class GetActivityChecklistByBoxActivityIdQueryHandler 
    : IRequestHandler<GetActivityChecklistByBoxActivityIdQuery, Result<GetActivityChecklistByBoxActivityDto>>
{
    private readonly IDbContext _context;

    public GetActivityChecklistByBoxActivityIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<GetActivityChecklistByBoxActivityDto>> Handle(
        GetActivityChecklistByBoxActivityIdQuery request, 
        CancellationToken cancellationToken)
    {
        // Get the BoxActivity with its related ActivityMaster or ActivityTemplateActivity
        var boxActivity = await _context.BoxActivities
            .Include(ba => ba.ActivityMaster)
            .Include(ba => ba.ActivityTemplateActivity)
            .FirstOrDefaultAsync(ba => ba.BoxActivityId == request.BoxActivityId, cancellationToken);

        if (boxActivity == null)
        {
            return Result.Failure<GetActivityChecklistByBoxActivityDto>("Box activity not found.");
        }

        // Determine which ID to use for fetching checklist items
        var activityMasterId = boxActivity.ActivityMasterId;
        var activityTemplateActivityId = boxActivity.ActivityTemplateActivityId;

        if (activityMasterId == null && activityTemplateActivityId == null)
        {
            return Result.Failure<GetActivityChecklistByBoxActivityDto>(
                "Box activity is not linked to an ActivityMaster or ActivityTemplateActivity.");
        }

        // Get checklist items based on the activity type
        var checklistItemsQuery = _context.ActivityCheckListItems
            .Include(aci => aci.PredefinedChecklistItem)
                .ThenInclude(pci => pci.ChecklistSection)
                    .ThenInclude(cs => cs!.Checklist)
            .Where(aci => 
                (activityMasterId != null && aci.ActivityMasterId == activityMasterId) ||
                (activityTemplateActivityId != null && aci.ActivityTemplateActivityId == activityTemplateActivityId))
            .Where(aci => aci.IsActive)
            .OrderBy(aci => aci.Sequence);

        // Get existing reviews for this box activity
        var reviews = await _context.Set<Dubox.Domain.Entities.ActivityCheckListItemReview>()
            .Include(r => r.ReviewedByUser)
            .Where(r => r.BoxActivityId == request.BoxActivityId)
            .ToListAsync(cancellationToken);

        // Combine checklist items with their reviews
        var checklistItems = await checklistItemsQuery
            .Select(aci => new ActivityCheckListItemWithReviewDto(
                aci.ActivityCheckListItemId,
                aci.ActivityMasterId,
                aci.ActivityTemplateActivityId,
                aci.PredefinedChecklistItemId,
                aci.PredefinedChecklistItem.Description,
                aci.PredefinedChecklistItem.Reference,
                aci.Sequence,
                aci.IsMandatory,
                aci.IsActive,
                // Grouping information
                aci.PredefinedChecklistItem.ChecklistSectionId,
                aci.PredefinedChecklistItem.ChecklistSection != null ? aci.PredefinedChecklistItem.ChecklistSection.Title : null,
                aci.PredefinedChecklistItem.ChecklistSection != null ? aci.PredefinedChecklistItem.ChecklistSection.Order : null,
                aci.PredefinedChecklistItem.ChecklistSection != null ? aci.PredefinedChecklistItem.ChecklistSection.ChecklistId : null,
                aci.PredefinedChecklistItem.ChecklistSection != null && aci.PredefinedChecklistItem.ChecklistSection.Checklist != null 
                    ? aci.PredefinedChecklistItem.ChecklistSection.Checklist.Name : null,
                aci.PredefinedChecklistItem.ChecklistSection != null && aci.PredefinedChecklistItem.ChecklistSection.Checklist != null 
                    ? aci.PredefinedChecklistItem.ChecklistSection.Checklist.Code : null,
                // Review information will be populated below
                null,
                CheckListItemStatusEnum.Pending,
                null,
                null,
                null,
                null
            ))
            .ToListAsync(cancellationToken);

        // Map reviews to checklist items
        var checklistItemsWithReviews = checklistItems.Select(item =>
        {
            var review = reviews.FirstOrDefault(r => r.ActivityCheckListItemId == item.ActivityCheckListItemId);
            
            if (review != null)
            {
                return item with
                {
                    ReviewId = review.ActivityCheckListItemReviewId,
                    ReviewStatus = review.Status,
                    Remarks = review.Remarks,
                    ReviewedBy = review.ReviewedBy,
                    ReviewedByName = review.ReviewedByUser?.FullName ?? review.ReviewedByUser?.Email,
                    ReviewedDate = review.ReviewedDate
                };
            }
            
            return item;
        }).ToList();

        var activityName = boxActivity.ActivityMaster?.ActivityName 
            ?? boxActivity.ActivityTemplateActivity?.ActivityName 
            ?? "Unknown Activity";
        
        var activityCode = boxActivity.ActivityMaster?.ActivityCode 
            ?? boxActivity.ActivityTemplateActivity?.ActivityCode 
            ?? "Unknown";

        var result = new GetActivityChecklistByBoxActivityDto(
            boxActivity.BoxActivityId,
            activityName,
            activityCode,
            boxActivity.ProgressPercentage,
            checklistItemsWithReviews
        );

        return Result.Success(result);
    }
}





