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
    /// Get all progress updates for a specific box
    /// </summary>
    [HttpGet("box/{boxId}")]
    public async Task<IActionResult> GetProgressUpdatesByBox(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProgressUpdatesByBoxQuery(boxId), cancellationToken);
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

