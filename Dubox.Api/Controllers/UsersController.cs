using Dubox.Application.Features.Users.Commands;
using Dubox.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25, CancellationToken cancellationToken = default)
    {
        var query = new GetAllUsersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(userId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (userId != command.UserId)
            return BadRequest("User ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetUserRoles(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserRolesQuery(userId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{userId}/roles")]
    public async Task<IActionResult> AssignRolesToUser(Guid userId, [FromBody] List<Guid> roleIds, CancellationToken cancellationToken)
    {
        var command = new AssignRolesToUserCommand(userId, roleIds);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{userId}/groups")]
    public async Task<IActionResult> AssignUserToGroups(Guid userId, [FromBody] List<Guid> groupIds, CancellationToken cancellationToken)
    {
        var command = new AssignUserToGroupsCommand(userId, groupIds);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);

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

