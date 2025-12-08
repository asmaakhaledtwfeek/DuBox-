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

    /// <summary>
    /// Get all permissions
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllPermissions(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllPermissionsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get permissions grouped by category and module
    /// </summary>
    [HttpGet("grouped")]
    public async Task<IActionResult> GetPermissionsGrouped(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPermissionsGroupedQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get permission matrix for all roles
    /// </summary>
    [HttpGet("matrix")]
    public async Task<IActionResult> GetPermissionMatrix(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPermissionMatrixQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get permissions for a specific role
    /// </summary>
    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetRolePermissions(Guid roleId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRolePermissionsQuery(roleId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all permissions for a user (from direct roles and group roles)
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserPermissions(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserPermissionsQuery(userId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Assign permissions to a role
    /// </summary>
    [HttpPost("role/{roleId}")]
    public async Task<IActionResult> AssignPermissionsToRole(Guid roleId, [FromBody] List<Guid> permissionIds, CancellationToken cancellationToken)
    {
        var command = new AssignPermissionsToRoleCommand(roleId, permissionIds);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

