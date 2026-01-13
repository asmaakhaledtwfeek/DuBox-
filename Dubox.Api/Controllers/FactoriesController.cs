using Dubox.Application.Features.Factories.Commands;
using Dubox.Application.Features.Factories.Queries;
using Dubox.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FactoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FactoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all factories
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllFactories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllFactoriesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get factory by ID
    /// </summary>
    [HttpGet("{factoryId}")]
    public async Task<IActionResult> GetFactoryById(Guid factoryId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFactoryByIdQuery(factoryId), cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get factories by location
    /// </summary>
    [HttpGet("location/{location}")]
    public async Task<IActionResult> GetFactoriesByLocation(int location, CancellationToken cancellationToken)
    {
        // Validate location enum value
        if (!Enum.IsDefined(typeof(ProjectLocationEnum), location))
        {
            return BadRequest(new { message = "Invalid location value. Valid values are: 1 (KSA), 2 (UAE)" });
        }

        var locationEnum = (ProjectLocationEnum)location;
        var result = await _mediator.Send(new GetFactoriesByLocationQuery(locationEnum), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Create a new factory
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFactory([FromBody] CreateFactoryCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

