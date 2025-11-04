using Dubox.Application.Features.Activities.Commands;
using Dubox.Application.Features.Activities.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActivitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all activity master templates (seeded data)
    /// </summary>
    [HttpGet("masters")]
    public async Task<IActionResult> GetAllActivityMasters(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllActivityMastersQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get activity masters by stage number (1-6)
    /// </summary>
    [HttpGet("masters/stage/{stageNumber}")]
    public async Task<IActionResult> GetActivitiesByStage(int stageNumber, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActivitiesByStageQuery(stageNumber), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all activities for a specific box
    /// </summary>
    [HttpGet("box/{boxId}")]
    public async Task<IActionResult> GetBoxActivities(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxActivitiesByBoxQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Update box activity progress and status
    /// </summary>
    [HttpPut("{boxActivityId}")]
    public async Task<IActionResult> UpdateBoxActivity(Guid boxActivityId, [FromBody] UpdateBoxActivityCommand command, CancellationToken cancellationToken)
    {
        if (boxActivityId != command.BoxActivityId)
            return BadRequest("Box Activity ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

