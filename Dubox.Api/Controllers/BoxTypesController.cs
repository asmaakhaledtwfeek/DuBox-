using Dubox.Application.Features.BoxTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoxTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoxTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllProjectTypeCategoriesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("by-category/{categoryId}")]
    public async Task<IActionResult> GetBoxTypesByCategory(int categoryId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxTypesByCategoryQuery(categoryId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxTypeId}/subtypes")]
    public async Task<IActionResult> GetBoxSubTypesByBoxType(int boxTypeId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxSubTypesByBoxTypeQuery(boxTypeId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

