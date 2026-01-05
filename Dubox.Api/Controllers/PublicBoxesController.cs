using Dubox.Application.Features.Boxes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

/// <summary>
/// Public controller for box information accessed via QR code scan
/// No authentication required
/// </summary>
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

    /// <summary>
    /// Get public box details by ID (no authentication required)
    /// Used when scanning QR codes
    /// </summary>
    /// <param name="boxId">The box ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Public box details</returns>
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
}

