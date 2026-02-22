using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Dubox.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public class SubmitActivityChecklistReviewCommandHandler 
    : IRequestHandler<SubmitActivityChecklistReviewCommand, Result<string>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SubmitActivityChecklistReviewCommandHandler(
        IDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<string>> Handle(
        SubmitActivityChecklistReviewCommand request, 
        CancellationToken cancellationToken)
    {
        // Get current user ID and username
        var userId = _currentUserService.UserId;
        var userGuid = string.IsNullOrEmpty(userId) ? (Guid?)null : Guid.Parse(userId);
        var username = _currentUserService.Username;

        // Verify box activity exists
        var boxActivity = await _context.BoxActivities
            .FirstOrDefaultAsync(ba => ba.BoxActivityId == request.BoxActivityId, cancellationToken);

        if (boxActivity == null)
        {
            return Result.Failure<string>("Box activity not found.");
        }

        // Validate that activity progress is 100%
        if (boxActivity.ProgressPercentage < 100)
        {
            return Result.Failure<string>(
                $"Activity checklist can only be reviewed after progress reaches 100%. Current progress: {boxActivity.ProgressPercentage}%");
        }

        // Get existing reviews for this box activity
        var existingReviews = await _context.Set<ActivityCheckListItemReview>()
            .Where(r => r.BoxActivityId == request.BoxActivityId)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;

        foreach (var reviewDto in request.Reviews)
        {
            // Verify the checklist item exists
            var checklistItem = await _context.ActivityCheckListItems
                .FirstOrDefaultAsync(aci => aci.ActivityCheckListItemId == reviewDto.ActivityCheckListItemId, cancellationToken);

            if (checklistItem == null)
            {
                return Result.Failure<string>($"Checklist item {reviewDto.ActivityCheckListItemId} not found.");
            }

            // Check if review already exists
            var existingReview = existingReviews
                .FirstOrDefault(r => r.ActivityCheckListItemId == reviewDto.ActivityCheckListItemId);

            if (existingReview != null)
            {
                // Update existing review
                existingReview.Status = reviewDto.Status;
                existingReview.Remarks = reviewDto.Remarks;
                existingReview.ReviewedBy = userGuid;
                existingReview.ReviewedDate = now;
                existingReview.ModifiedDate = now;
                existingReview.ModifiedBy = username;
            }
            else
            {
                // Create new review
                var newReview = new ActivityCheckListItemReview
                {
                    ActivityCheckListItemReviewId = Guid.NewGuid(),
                    BoxActivityId = request.BoxActivityId,
                    ActivityCheckListItemId = reviewDto.ActivityCheckListItemId,
                    Status = reviewDto.Status,
                    Remarks = reviewDto.Remarks,
                    ReviewedBy = userGuid,
                    ReviewedDate = now,
                    CreatedDate = now,
                    CreatedBy = username,
                    ModifiedDate = now,
                    ModifiedBy = username
                };

                _context.Set<ActivityCheckListItemReview>().Add(newReview);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success<string>("Activity checklist review submitted successfully.");
    }
}

