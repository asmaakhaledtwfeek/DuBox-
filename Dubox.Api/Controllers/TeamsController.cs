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
    public async Task<IActionResult> GetAllTeams(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTeamsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
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
    public async Task<IActionResult> GetTeamMembers(int teamId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamMembersQuery(teamId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

}

