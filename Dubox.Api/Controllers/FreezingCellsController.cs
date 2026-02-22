using Dubox.Application.Features.FreezingCells.Commands;
using Dubox.Application.Features.FreezingCells.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FreezingCellsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FreezingCellsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all freezing cells for a specific factory section part
    /// </summary>
    [HttpGet("part/{factorySectionPartId}")]
    public async Task<IActionResult> GetFreezingCellsByPart(Guid factorySectionPartId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFreezingCellsByPartQuery(factorySectionPartId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Create a new freezing cell (freeze a row in a part)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFreezingCell([FromBody] CreateFreezingCellCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete a freezing cell (unfreeze a row)
    /// </summary>
    [HttpDelete("{freezingCellId}")]
    public async Task<IActionResult> DeleteFreezingCell(Guid freezingCellId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteFreezingCellCommand(freezingCellId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
