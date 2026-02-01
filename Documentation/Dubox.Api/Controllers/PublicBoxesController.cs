using Dubox.Application.Features.Boxes.Queries;
using Dubox.Application.Features.BoxDrawings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/public/boxes")]
[AllowAnonymous]
public class PublicBoxesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PublicBoxesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{boxId}")]
    public async Task<IActionResult> GetPublicBoxById(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPublicBoxByIdQuery(boxId), cancellationToken);
        
        if (!result.IsSuccess)
        {
            return result.Message?.Contains("not found") == true
                ? NotFound(result)
                : BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("{boxId}/summary")]
    public async Task<IActionResult> GetPublicBoxSummary(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxSummaryQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxId}/attachments")]
    public async Task<IActionResult> GetPublicBoxAttachments(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxAttachmentsQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxId}/drawings")]
    public async Task<IActionResult> GetPublicBoxDrawings(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxDrawingsQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

