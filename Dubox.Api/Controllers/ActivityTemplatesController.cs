using Dubox.Application.Features.ActivityTemplates.Commands;
using Dubox.Application.Features.ActivityTemplates.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/activity-templates")]
[Authorize]
public class ActivityTemplatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActivityTemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all activity templates
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllTemplates(
        [FromQuery] bool activeOnly = true,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllActivityTemplatesQuery(activeOnly, searchTerm);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get activity template by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetActivityTemplateByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new activity template
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTemplate(
        [FromBody] CreateActivityTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetTemplateById),
                new { id = result.Data },
                result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Create activity template from Activity Master
    /// </summary>
    [HttpPost("from-master")]
    public async Task<IActionResult> CreateTemplateFromMaster(
        [FromBody] CreateActivityTemplateFromMasterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetTemplateById),
                new { id = result.Data },
                result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Update an existing activity template
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate(
        Guid id,
        [FromBody] UpdateActivityTemplateCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.ActivityTemplateId)
        {
            return BadRequest("Template ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete an activity template
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteActivityTemplateCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Add activity to template
    /// </summary>
    [HttpPost("add-activity")]
    public async Task<IActionResult> AddActivityToTemplate(
        [FromBody] AddActivityToTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
