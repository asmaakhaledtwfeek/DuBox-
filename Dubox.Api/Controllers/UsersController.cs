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
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(userId), cancellationToken);
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
}

