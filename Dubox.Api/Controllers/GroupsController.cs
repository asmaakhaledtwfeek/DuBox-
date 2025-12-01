using Dubox.Application.Features.Groups.Commands;
using Dubox.Application.Features.Groups.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGroups(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllGroupsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{groupId}")]
    public async Task<IActionResult> UpdateGroup(Guid groupId, [FromBody] UpdateGroupCommand command, CancellationToken cancellationToken)
    {
        if (groupId != command.GroupId)
        {
            return BadRequest("Group ID mismatch.");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{groupId}")]
    public async Task<IActionResult> DeleteGroup(Guid groupId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteGroupCommand(groupId), cancellationToken);

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

    [HttpPost("{groupId}/roles")]
    public async Task<IActionResult> AssignRolesToGroup(Guid groupId, [FromBody] List<Guid> roleIds, CancellationToken cancellationToken)
    {
        var command = new AssignRolesToGroupCommand(groupId, roleIds);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

