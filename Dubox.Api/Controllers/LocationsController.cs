using Dubox.Application.Features.FactoryLocations.Commands;
using Dubox.Application.Features.FactoryLocations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLocations(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllFactoryLocationsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{locationId}")]
    public async Task<IActionResult> GetLocationById(Guid locationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLocationByIdQuery(locationId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] CreateFactoryLocationCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetLocationById), new { locationId = result.Data!.LocationId }, result) : BadRequest(result);
    }

    [HttpGet("check-exists")]
    public async Task<IActionResult> CheckLocationExists([FromQuery] string locationCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(locationCode))
            return BadRequest("Location code is required");

        var result = await _mediator.Send(new CheckLocationExistsQuery(locationCode), cancellationToken);
        return result.IsSuccess ? Ok(new { exists = result.Data }) : BadRequest(result);
    }

    [HttpPost("move-box")]
    public async Task<IActionResult> MoveBoxToLocation([FromBody] MoveBoxToLocationCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("boxes/{boxId}/history")]
    public async Task<IActionResult> GetBoxLocationHistory(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxLocationHistoryQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{locationId}/boxes")]
    public async Task<IActionResult> GetBoxesByLocation(Guid locationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxesByLocationQuery(locationId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

