using Dubox.Application.Features.Boxes.Commands;
using Dubox.Application.Features.Boxes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoxesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoxesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBoxes(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllBoxesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxId}")]
    public async Task<IActionResult> GetBoxById(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxByIdQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetBoxesByProject(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxesByProjectQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("qr/{qrCodeString}")]
    public async Task<IActionResult> GetBoxByQRCode(string qrCodeString, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxByQRCodeQuery(qrCodeString), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBox([FromBody] CreateBoxCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetBoxById), new { boxId = result.Data!.BoxId }, result) : BadRequest(result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportBoxes([FromBody] ImportBoxesCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{boxId}")]
    public async Task<IActionResult> UpdateBox(Guid boxId, [FromBody] UpdateBoxCommand command, CancellationToken cancellationToken)
    {
        if (boxId != command.BoxId)
            return BadRequest("Box ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{boxId}")]
    public async Task<IActionResult> DeleteBox(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteBoxCommand(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

