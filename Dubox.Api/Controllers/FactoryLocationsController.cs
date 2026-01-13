using Dubox.Application.Features.FactoryLocations.Commands;
using Dubox.Application.Features.FactoryLocations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FactoryLocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FactoryLocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all factory locations
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllFactoryLocations(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllFactoryLocationsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Create a new factory location
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFactoryLocation([FromBody] CreateFactoryLocationCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

