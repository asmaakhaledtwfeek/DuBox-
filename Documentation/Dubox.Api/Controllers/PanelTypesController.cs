using Dubox.Application.Features.PanelTypes.Commands;
using Dubox.Application.Features.PanelTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId}/[controller]")]
[Authorize]
public class PanelTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PanelTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPanelTypesByProject(
        Guid projectId,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetPanelTypesByProjectQuery(projectId, includeInactive), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{panelTypeId}")]
    public async Task<IActionResult> GetPanelTypeById(
        Guid projectId,
        Guid panelTypeId,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetPanelTypeByIdQuery(panelTypeId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePanelType(
        Guid projectId,
        [FromBody] CreatePanelTypeCommand command,
        CancellationToken cancellationToken = default)
    {
        if (projectId != command.ProjectId)
            return BadRequest("Project ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess 
            ? CreatedAtAction(nameof(GetPanelTypeById), new { projectId, panelTypeId = result.Data!.PanelTypeId }, result) 
            : BadRequest(result);
    }

    [HttpPut("{panelTypeId}")]
    public async Task<IActionResult> UpdatePanelType(
        Guid projectId,
        Guid panelTypeId,
        [FromBody] UpdatePanelTypeCommand command,
        CancellationToken cancellationToken = default)
    {
        if (panelTypeId != command.PanelTypeId)
            return BadRequest("Panel Type ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{panelTypeId}")]
    public async Task<IActionResult> DeletePanelType(
        Guid projectId,
        Guid panelTypeId,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new DeletePanelTypeCommand(panelTypeId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

