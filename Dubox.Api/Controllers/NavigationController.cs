using Dubox.Application.Features.Navigation.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NavigationController : ControllerBase
{
    private readonly IMediator _mediator;

    public NavigationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("menu")]
    public async Task<IActionResult> GetMenuItems(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetNavigationMenuItemsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

