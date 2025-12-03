using Dubox.Application.Features.ProgressUpdates.Commands;
using Dubox.Application.Features.ProgressUpdates.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressUpdatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressUpdatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a progress update (used by mobile scanning and web interface)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateProgressUpdate([FromBody] CreateProgressUpdateCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all progress updates for a specific box with pagination and search
    /// </summary>
    [HttpGet("box/{boxId}")]
    public async Task<IActionResult> GetProgressUpdatesByBox(
        Guid boxId, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? activityName = null,
        [FromQuery] string? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] Guid? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProgressUpdatesByBoxQuery(boxId)
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            ActivityName = activityName,
            Status = status,
            FromDate = fromDate,
            ToDate = toDate,
            UpdatedBy = updatedBy
        };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all progress updates for a specific box activity
    /// </summary>
    [HttpGet("activity/{boxActivityId}")]
    public async Task<IActionResult> GetProgressUpdatesByActivity(Guid boxActivityId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProgressUpdatesByActivityQuery(boxActivityId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

