using Dubox.Application.Features.BoxMaterials.Commands;
using Dubox.Application.Features.BoxMaterials.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/boxes/{boxId}/materials")]
[Authorize]
public class BoxMaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoxMaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get material checklist for a box
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetBoxMaterials(
        Guid boxId,
        [FromQuery] bool? onlyOverdue = null,
        [FromQuery] bool? onlyPending = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetBoxMaterialsQuery(boxId, onlyOverdue, onlyPending);
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Mark material as arrived or not arrived
    /// </summary>
    [HttpPut("{materialId}/arrived")]
    public async Task<IActionResult> MarkMaterialArrived(
        Guid boxId,
        Guid materialId,
        [FromBody] MarkMaterialArrivedRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new MarkMaterialArrivedCommand(boxId, materialId, request.IsArrived);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

public record MarkMaterialArrivedRequest(bool IsArrived);






