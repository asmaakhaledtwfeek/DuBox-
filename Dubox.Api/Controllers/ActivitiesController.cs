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

    [HttpGet("masters")]
    public async Task<IActionResult> GetAllActivityMasters(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllActivityMastersQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("masters/stage/{stageNumber}")]
    public async Task<IActionResult> GetActivitiesByStage(int stageNumber, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActivitiesByStageQuery(stageNumber), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxActivityId}")]
    public async Task<IActionResult> GetBoxActivityById(Guid boxActivityId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxActivityByIdQuery(boxActivityId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("box/{boxId}")]
    public async Task<IActionResult> GetBoxActivities(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxActivitiesByBoxQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("update-status/{boxActivityId}")]
    public async Task<IActionResult> UpdateBoxActivityStatus(Guid boxActivityId, [FromBody] UpdateBoxActivityStatusCommand command, CancellationToken cancellationToken)
    {
        if (boxActivityId != command.BoxActivityId)
            return BadRequest("Box Activity ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPut("Assign-team")]
    public async Task<IActionResult> AssignActivityToTeam([FromBody] AssignActivityToTeamCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("issue-to-activity")]
    public async Task<IActionResult> IssueMaterialToActivity([FromBody] IssueMaterialToActivityCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

