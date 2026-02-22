using Dubox.Application.Features.Checklists.Commands;
using Dubox.Application.Features.Checklists.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChecklistsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChecklistsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateChecklist([FromBody] CreateChecklistCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllChecklists(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllChecklistsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("by-wir-code/{wirCode}")]
    public async Task<IActionResult> GetChecklistsByWIRCode(string wirCode, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetChecklistsByWIRCodeQuery(wirCode), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("sections")]
    public async Task<IActionResult> CreateSection([FromBody] CreateChecklistSectionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("items")]
    public async Task<IActionResult> CreateItem([FromBody] CreateChecklistItemCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("clone")]
    public async Task<IActionResult> CloneChecklistItems([FromBody] CloneChecklistItemsCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
