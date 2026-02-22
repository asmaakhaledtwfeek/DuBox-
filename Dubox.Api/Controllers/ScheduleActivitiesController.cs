using Dubox.Application.Features.Schedule.Commands;
using Dubox.Application.Features.Schedule.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/schedule-activities")]
[Authorize]
public class ScheduleActivitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleActivitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all schedule activities
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllScheduleActivities(
        [FromQuery] Guid? projectId = null,
        CancellationToken cancellationToken = default)
    {
        // You'll need to create this query
        // var query = new GetAllScheduleActivitiesQuery(projectId);
        // var result = await _mediator.Send(query, cancellationToken);
        // return result.IsSuccess ? Ok(result) : BadRequest(result);
        
        return Ok(new { message = "Endpoint ready - implement GetAllScheduleActivitiesQuery" });
    }

    /// <summary>
    /// Get schedule activity by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetScheduleActivityById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetScheduleActivityDetailsQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create schedule activity from Activity Master
    /// </summary>
    [HttpPost("from-master")]
    public async Task<IActionResult> CreateFromMaster(
        [FromBody] CreateScheduleActivityFromMasterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetScheduleActivityById),
                new { id = result.Data },
                result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Create schedule activities from template
    /// </summary>
    [HttpPost("from-template")]
    public async Task<IActionResult> CreateFromTemplate(
        [FromBody] CreateScheduleActivitiesFromTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Create a schedule activity manually
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateScheduleActivity(
        [FromBody] CreateScheduleActivityCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetScheduleActivityById),
                new { id = result.Data },
                result);
        }

        return BadRequest(result);
    }
}
