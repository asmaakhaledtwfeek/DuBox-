using Dubox.Application.Features.Roles.Commands;
using Dubox.Application.Features.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllRolesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> UpdateRole(Guid roleId, [FromBody] UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        if (roleId != command.RoleId)
        {
            return BadRequest("Role ID mismatch.");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(Guid roleId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteRoleCommand(roleId), cancellationToken);

        if (result.IsSuccess)
            return Ok(result);

        // Check if it's a constraint/conflict error
        var errorMessage = result.Message ?? string.Empty;
        if (errorMessage.Contains("constraint", StringComparison.OrdinalIgnoreCase) ||
            errorMessage.Contains("foreign key", StringComparison.OrdinalIgnoreCase) ||
            errorMessage.Contains("relationship", StringComparison.OrdinalIgnoreCase))
        {
            return Conflict(result);
        }

        return BadRequest(result);
    }
}

