using Dubox.Application.Features.ActivityCheckListItems.Commands;
using Dubox.Application.Features.ActivityCheckListItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/activity-checklist-items")]
[Authorize]
public class ActivityCheckListItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActivityCheckListItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all checklist items for a specific ActivityMaster
    /// </summary>
    [HttpGet("by-activity-master/{activityMasterId}")]
    public async Task<IActionResult> GetChecklistItemsByActivityMaster(
        Guid activityMasterId,
        CancellationToken cancellationToken)
    {
        var query = new GetActivityCheckListItemsByActivityMasterIdQuery(activityMasterId);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all checklist items for a specific ActivityTemplateActivity
    /// </summary>
    [HttpGet("by-activity-template-activity/{activityTemplateActivityId}")]
    public async Task<IActionResult> GetChecklistItemsByActivityTemplateActivity(
        Guid activityTemplateActivityId,
        CancellationToken cancellationToken)
    {
        var query = new GetActivityCheckListItemsByActivityTemplateActivityIdQuery(activityTemplateActivityId);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Create a new activity checklist item
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateChecklistItem(
        [FromBody] CreateActivityCheckListItemCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Created(string.Empty, result) : BadRequest(result);
    }

    /// <summary>
    /// Bulk create multiple activity checklist items
    /// </summary>
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreateChecklistItems(
        [FromBody] BulkCreateActivityCheckListItemsCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Update an existing activity checklist item
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChecklistItem(
        Guid id,
        [FromBody] UpdateActivityCheckListItemCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.ActivityCheckListItemId)
        {
            return BadRequest("ChecklistItem ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete an activity checklist item
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChecklistItem(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteActivityCheckListItemCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get checklist items for a specific BoxActivity with review status
    /// </summary>
    [HttpGet("by-box-activity/{boxActivityId}")]
    public async Task<IActionResult> GetChecklistByBoxActivity(
        Guid boxActivityId,
        CancellationToken cancellationToken)
    {
        var query = new GetActivityChecklistByBoxActivityIdQuery(boxActivityId);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Submit reviews for activity checklist items
    /// Can only be done when activity progress is 100%
    /// </summary>
    [HttpPost("review")]
    public async Task<IActionResult> SubmitChecklistReview(
        [FromBody] SubmitActivityChecklistReviewCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get summary of activity checklist reviews (counts by status)
    /// </summary>
    [HttpGet("reviews/summary")]
    public async Task<IActionResult> GetActivityReviewsSummary(CancellationToken cancellationToken)
    {
        var query = new GetActivityReviewsSummaryQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get paginated list of activity reviews with filters
    /// </summary>
    [HttpGet("reviews")]
    public async Task<IActionResult> GetActivityReviews(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] Guid? projectId = null,
        [FromQuery] Guid? boxId = null,
        [FromQuery] string? buildingNumber = null,
        [FromQuery] string? floor = null,
        [FromQuery] int? boxTypeId = null,
        [FromQuery] string? reviewStatus = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetActivityReviewsQuery(page, pageSize, projectId, boxId, buildingNumber, floor, boxTypeId, reviewStatus, searchTerm);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
