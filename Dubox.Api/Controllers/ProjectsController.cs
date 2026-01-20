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
    public async Task<IActionResult> GetAllProjects([FromQuery] GetAllProjectsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
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

    [HttpPatch("{projectId}/status")]
    public async Task<IActionResult> UpdateProjectStatus(Guid projectId, [FromBody] UpdateProjectStatusCommand command, CancellationToken cancellationToken)
    {
        if (projectId != command.ProjectId)
            return BadRequest("Project ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{projectId}/compression-start-date")]
    public async Task<IActionResult> UpdateCompressionStartDate(Guid projectId, [FromBody] UpdateCompressionStartDateCommand command, CancellationToken cancellationToken)
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

    [HttpGet("{projectId}/configuration")]
    public async Task<IActionResult> GetProjectConfiguration(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectConfigurationQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{projectId}/configuration")]
    public async Task<IActionResult> SaveProjectConfiguration(Guid projectId, [FromBody] SaveProjectConfigurationCommand command, CancellationToken cancellationToken)
    {
        if (projectId != command.ProjectId)
            return BadRequest("Project ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{projectId}/box-panels/excel")]
    public async Task<IActionResult> DownloadBoxPanelsExcel(Guid projectId, CancellationToken cancellationToken)
    {
        if (projectId == Guid.Empty)
            return BadRequest("Project ID is required");

        // Get project details to construct filename
        var projectResult = await _mediator.Send(new GetProjectByIdQuery(projectId), cancellationToken);
        if (!projectResult.IsSuccess || projectResult.Data == null)
            return BadRequest("Project not found");

        var result = await _mediator.Send(new GenerateBoxPanelsExcelQuery(projectId), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        // Construct filename: ProjectCode-ProjectName-BoxPanels.xlsx
        var fileName = $"{projectResult.Data.ProjectCode}-{projectResult.Data.ProjectName}-BoxPanels.xlsx";

        return File(result.Data!,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    [HttpPost("{projectId}/box-panels/import-excel")]
    [RequestSizeLimit(10_485_760)] // 10 MB
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportBoxPanelsFromExcel(Guid projectId, [FromForm] IFormFile? file, CancellationToken cancellationToken)
    {
        if (projectId == Guid.Empty)
            return BadRequest("Project ID is required");

        // Support file from FormData
        IFormFile? actualFile = file;
        if (actualFile == null && Request.HasFormContentType)
        {
            var form = await Request.ReadFormAsync(cancellationToken);
            actualFile = form.Files["file"];
        }

        if (actualFile == null || actualFile.Length == 0)
            return BadRequest("No file uploaded");

        using var stream = actualFile.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var command = new ImportBoxPanelsFromExcelCommand(projectId, memoryStream, actualFile.FileName);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

