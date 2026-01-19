using Dubox.Application.Features.Schedule.Commands;
using Dubox.Application.Features.Schedule.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all schedule activities
    /// </summary>
    [HttpGet("activities")]
    public async Task<IActionResult> GetScheduleActivities(CancellationToken cancellationToken)
    {
        var query = new GetScheduleActivitiesQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get schedule activity details by ID
    /// </summary>
    [HttpGet("activities/{id}")]
    public async Task<IActionResult> GetScheduleActivityDetails(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetScheduleActivityDetailsQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new schedule activity
    /// </summary>
    [HttpPost("activities")]
    public async Task<IActionResult> CreateScheduleActivity(
        [FromBody] CreateScheduleActivityCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetScheduleActivityDetails),
                new { id = result.Data },
                result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Assign a team to a schedule activity
    /// </summary>
    [HttpPost("activities/{activityId}/assign-team")]
    public async Task<IActionResult> AssignTeam(
        Guid activityId,
        [FromBody] AssignTeamRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignTeamCommand(activityId, request.TeamId, request.Notes);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Assign a material to a schedule activity
    /// </summary>
    [HttpPost("activities/{activityId}/assign-material")]
    public async Task<IActionResult> AssignMaterial(
        Guid activityId,
        [FromBody] AssignMaterialRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignMaterialCommand(
            activityId,
            request.MaterialName,
            request.MaterialCode,
            request.Quantity,
            request.Unit,
            request.Notes);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// Request DTOs for API
public record AssignTeamRequest(Guid TeamId, string? Notes);
public record AssignMaterialRequest(
    string MaterialName,
    string? MaterialCode,
    decimal Quantity,
    string? Unit,
    string? Notes);

