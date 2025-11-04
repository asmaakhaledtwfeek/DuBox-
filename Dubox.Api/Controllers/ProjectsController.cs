using Dubox.Application.Features.Projects.Commands;
using Dubox.Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllProjectsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetProjectById(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetProjectById), new { projectId = result.Data!.ProjectId }, result) : BadRequest(result);
    }

    [HttpPut("{projectId}")]
    public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        if (projectId != command.ProjectId)
            return BadRequest("Project ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteProjectCommand(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

