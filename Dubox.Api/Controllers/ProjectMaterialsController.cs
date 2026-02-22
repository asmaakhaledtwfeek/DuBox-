using Dubox.Application.Features.ProjectMaterials.Commands;
using Dubox.Application.Features.ProjectMaterials.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId}/materials")]
[Authorize]
public class ProjectMaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectMaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get materials for a project
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProjectMaterials(
        Guid projectId,
        [FromQuery] bool selectedOnly = true,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProjectMaterialsQuery(projectId, selectedOnly);
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Select materials for a project
    /// DEPRECATED: Projects should use material templates instead of individual material selection
    /// </summary>
    [HttpPost("select")]
    [Obsolete("Projects should use material templates. Use /api/material-templates endpoints instead.")]
    public async Task<IActionResult> SelectMaterials(
        Guid projectId,
        [FromBody] SelectProjectMaterialsRequest request,
        CancellationToken cancellationToken = default)
    {
        // Return error - projects should use material templates
        return BadRequest(new 
        { 
            IsSuccess = false,
            Error = "Direct material selection for projects is no longer supported. Please assign material templates to the project instead. Use /api/material-templates endpoints."
        });
        
        // Old implementation commented out
        /*
        var command = new SelectProjectMaterialsCommand(
            projectId,
            request.MaterialIds ?? new List<Guid>(),
            request.SelectAll);
        
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
        */
    }
}

public record SelectProjectMaterialsRequest(
    List<Guid>? MaterialIds = null,
    bool SelectAll = false
);






