using Dubox.Application.Features.Permissions.Commands;
using Dubox.Application.Features.Permissions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPermissions(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllPermissionsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpGet("grouped")]
    public async Task<IActionResult> GetPermissionsGrouped(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPermissionsGroupedQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("matrix")]
    public async Task<IActionResult> GetPermissionMatrix(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPermissionMatrixQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetRolePermissions(Guid roleId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRolePermissionsQuery(roleId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserPermissions(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserPermissionsQuery(userId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("role/{roleId}")]
    public async Task<IActionResult> AssignPermissionsToRole(Guid roleId, [FromBody] List<Guid> permissionIds, CancellationToken cancellationToken)
    {
        var command = new AssignPermissionsToRoleCommand(roleId, permissionIds);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

