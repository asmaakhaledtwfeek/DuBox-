using Dubox.Application.DTOs;
using Dubox.Application.Features.Teams.Commands;
using Dubox.Application.Features.Teams.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeamsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeamsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllTeams(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? search = null,
        [FromQuery] string? department = null,
        [FromQuery] string? trade = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllTeamsQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            Department = department,
            Trade = trade,
            IsActive = isActive
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get team by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeamById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("team-groups")]
    public async Task<IActionResult> GetAllTeamGroups(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTeamGroupsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("team-groups")]
    public async Task<IActionResult> CreateTeamGroup([FromBody] CreateTeamGroupCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(Guid id, [FromBody] UpdateTeamCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { TeamId = id }, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost("assign-members")]
    public async Task<IActionResult> AssignTeamMembers([FromBody] AssignedTeamMembersCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost("complate-member-profile")]
    public async Task<IActionResult> ComplateMemberProfile([FromBody] ComplateTeamMemberProfileCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("team-members/{teamId}")]
    public async Task<IActionResult> GetTeamMembers(Guid teamId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamMembersQuery(teamId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("team-members/{teamId}/member/{teamMemberId}")]
    public async Task<IActionResult> RemoveTeamMember(Guid teamId, Guid teamMemberId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveTeamMemberCommand(teamId, teamMemberId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

}

