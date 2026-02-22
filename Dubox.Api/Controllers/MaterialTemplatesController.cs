using Dubox.Application.Features.MaterialTemplates.Commands;
using Dubox.Application.Features.MaterialTemplates.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/material-templates")]
[Authorize]
public class MaterialTemplatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialTemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all material templates
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllTemplates(
        [FromQuery] bool activeOnly = true,
        [FromQuery] string? category = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllMaterialTemplatesQuery(activeOnly, category, searchTerm);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get material template by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMaterialTemplateByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new material template
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTemplate(
        [FromBody] CreateMaterialTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetTemplateById),
                new { id = result.Data },
                result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Update an existing material template
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate(
        Guid id,
        [FromBody] UpdateMaterialTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMaterialTemplateCommand(
            id,
            request.TemplateName,
            request.Description,
            request.Category,
            request.Items);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete (deactivate) a material template
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteMaterialTemplateCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Assign template to project
    /// </summary>
    [HttpPost("{id}/assign-to-project")]
    public async Task<IActionResult> AssignToProject(
        Guid id,
        [FromBody] AssignToProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignTemplateToProjectCommand(request.ProjectId, id, request.OldMaterialTemplateId);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Assign template to box type
    /// </summary>
    [HttpPost("{id}/assign-to-box-type")]
    public async Task<IActionResult> AssignToBoxType(
        Guid id,
        [FromBody] AssignToBoxTypeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignTemplateToBoxTypeCommand(request.ProjectBoxTypeId, id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get templates assigned to a project
    /// </summary>
    [HttpGet("projects/{projectId}")]
    public async Task<IActionResult> GetProjectTemplates(Guid projectId, CancellationToken cancellationToken)
    {
        var query = new GetProjectTemplatesQuery(projectId);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get templates assigned to box types in a project
    /// </summary>
    [HttpGet("projects/{projectId}/box-types")]
    public async Task<IActionResult> GetBoxTypeTemplates(Guid projectId, CancellationToken cancellationToken)
    {
        var query = new GetBoxTypeTemplatesQuery(projectId);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get available materials for template
    /// </summary>
    [HttpGet("available-materials")]
    public async Task<IActionResult> GetAvailableMaterials(
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? category = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAvailableMaterialsForTemplateQuery(searchTerm, category);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Remove template from project
    /// </summary>
    [HttpDelete("projects/{projectId}/templates/{templateId}")]
    public async Task<IActionResult> RemoveFromProject(
        Guid projectId,
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTemplateFromProjectCommand(projectId, templateId);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Remove template from box type
    /// </summary>
    [HttpDelete("box-types/{boxTypeId}/templates/{templateId}")]
    public async Task<IActionResult> RemoveFromBoxType(
        int boxTypeId,
        Guid templateId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTemplateFromBoxTypeCommand(boxTypeId, templateId);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// Request DTOs
public record UpdateMaterialTemplateRequest(
    string TemplateName,
    string? Description,
    string? Category,
    List<MaterialTemplateItemDto> Items);

public record AssignToProjectRequest(Guid ProjectId, Guid? OldMaterialTemplateId = null);

public record AssignToBoxTypeRequest(int ProjectBoxTypeId);

