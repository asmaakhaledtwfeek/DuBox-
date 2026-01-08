using Dubox.Application.DTOs;
using Dubox.Application.Features.Teams.Commands;
using Dubox.Application.Features.Teams.Queries;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

public record AddMembersToGroupRequest(List<Guid> TeamMemberIds);

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

    [HttpPost("{teamId}/members")]
    [ProducesResponseType(typeof(Result<TeamMemberDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTeamMember(
        Guid teamId,
        [FromBody] AddTeamMemberCommand command,
        CancellationToken cancellationToken)
    {
        var addMemberCommand = command with { TeamId = teamId };
        var result = await _mediator.Send(addMemberCommand, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("team-groups")]
    [ProducesResponseType(typeof(PaginatedTeamGroupsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllTeamGroups(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? search = null,
        [FromQuery] Guid? teamId = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllTeamGroupsQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            TeamId = teamId,
            IsActive = isActive
        };

        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("team-groups/{id}")]
    [ProducesResponseType(typeof(TeamGroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeamGroupById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamGroupByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("team-groups/{id}/members")]
    [ProducesResponseType(typeof(TeamGroupMembersDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeamGroupMembers(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamGroupMembersQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }


    [HttpPost("team/{teamId}/assign-leader")]
    [ProducesResponseType(typeof(TeamGroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignTeamLeader(
        Guid teamId,
        [FromBody] AssignTeamLeaderCommand request,
        CancellationToken cancellationToken)
    {
        var command = new AssignTeamLeaderCommand(teamId, request.TeamMemberId);
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

    [HttpGet("team-members/{teamId}/inactive")]
    [ProducesResponseType(typeof(TeamMembersDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInactiveTeamMembers(Guid teamId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetInactiveTeamMembersQuery(teamId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all active team members across all teams
    /// </summary>
    [HttpGet("all-active-members")]
    [ProducesResponseType(typeof(List<TeamMemberDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllActiveMembers(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllActiveMembersQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("team-members/{teamId}/reactivate/{teamMemberId}")]
    [ProducesResponseType(typeof(TeamMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReactivateTeamMember(Guid teamId, Guid teamMemberId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReactivateTeamMemberCommand(teamId, teamMemberId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

   
    [HttpGet("{teamId}/available-users")]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvailableUsersForTeam(Guid teamId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAvailableUsersForTeamQuery(teamId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{teamId}/users")]
    public async Task<IActionResult> GetTeamUsers(Guid teamId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamMembersQuery(teamId), cancellationToken);
        
        if (!result.IsSuccess || result.Data == null)
            return BadRequest(result);

        // Transform to simple user list for assignment dropdowns
        var users = result.Data.Members
            .Select(m => new 
            {
                userId = m.UserId,
                userName = m.FullName,
                userEmail = m.Email
            })
            .ToList();

        return Ok(Result.Success(users));
    }

    [HttpDelete("team-members/{teamId}/member/{teamMemberId}")]
    public async Task<IActionResult> RemoveTeamMember(Guid teamId, Guid teamMemberId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveTeamMemberCommand(teamId, teamMemberId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

}

