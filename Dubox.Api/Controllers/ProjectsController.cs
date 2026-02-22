using Dubox.Api.Models;
using Dubox.Application.DTOs;
using Dubox.Application.Features.Panels.Commands;
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

    [HttpPatch("{projectId}/box-types/{boxTypeId}")]
    public async Task<IActionResult> UpdateProjectBoxType(Guid projectId, int boxTypeId, [FromBody] UpdateProjectBoxTypeCommand command, CancellationToken cancellationToken)
    {
        if (projectId != command.ProjectId)
            return BadRequest("Project ID mismatch");
        if (boxTypeId != command.BoxTypeId)
            return BadRequest("Box Type ID mismatch");

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

    [HttpPost("{projectId}/upload-images")]
    [RequestSizeLimit(15_728_640)] // 15 MB (5MB per image * 3)
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProjectImages(
        Guid projectId,
        [FromForm] UploadProjectImagesRequest request,
        CancellationToken cancellationToken)
    {
        if (projectId == Guid.Empty)
            return BadRequest("Project ID is required");

        // Validate that at least one image is provided
        if (request.ContractorImage == null && request.SubContractorImage == null && request.ClientImage == null)
            return BadRequest("At least one image must be provided");

        // Validate file types
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp",".jfif" };
        
        if (request.ContractorImage != null)
        {
            var ext = Path.GetExtension(request.ContractorImage.FileName).ToLowerInvariant();
            if (!validExtensions.Contains(ext))
                return BadRequest("Contractor image must be a valid image file (jpg, jpeg, png, gif, webp)");
        }

        if (request.SubContractorImage != null)
        {
            var ext = Path.GetExtension(request.SubContractorImage.FileName).ToLowerInvariant();
            if (!validExtensions.Contains(ext))
                return BadRequest("Sub-contractor image must be a valid image file (jpg, jpeg, png, gif, webp)");
        }

        if (request.ClientImage != null)
        {
            var ext = Path.GetExtension(request.ClientImage.FileName).ToLowerInvariant();
            if (!validExtensions.Contains(ext))
                return BadRequest("Client image must be a valid image file (jpg, jpeg, png, gif, webp)");
        }

        var command = new UploadProjectImagesCommand(
            projectId,
            request.ContractorImage,
            request.SubContractorImage,
            request.ClientImage
        );

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Extract panel data from PDF using AI
    /// Returns data for user review before saving
    /// </summary>
    [HttpPost("{projectId}/panels/extract-from-pdf")]
    [RequestSizeLimit(10_485_760)] // 10 MB
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ExtractPanelsFromPdf(
        Guid projectId,
        [FromForm] ExtractPanelsFromPdfRequest request,
        CancellationToken cancellationToken)
    {
        if (projectId == Guid.Empty)
            return BadRequest("Project ID is required");

        if (request.File == null || request.File.Length == 0)
            return BadRequest("No PDF file uploaded");

        // Validate main file type
        var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
        if (extension != ".pdf")
            return BadRequest("Only PDF files are supported");

        using var stream = request.File.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        // Process second file if provided
        MemoryStream? boxTagsMemoryStream = null;
        string? boxTagsFileName = null;
        
        if (request.BoxTagsFile != null && request.BoxTagsFile.Length > 0)
        {
            var boxTagsExtension = Path.GetExtension(request.BoxTagsFile.FileName).ToLowerInvariant();
            if (boxTagsExtension != ".pdf")
                return BadRequest("Box tags file must be a PDF");

            using var boxTagsStream = request.BoxTagsFile.OpenReadStream();
            boxTagsMemoryStream = new MemoryStream();
            await boxTagsStream.CopyToAsync(boxTagsMemoryStream, cancellationToken);
            boxTagsMemoryStream.Position = 0;
            boxTagsFileName = request.BoxTagsFile.FileName;
        }

        var command = new ExtractPanelsFromPdfCommand(
            projectId,
            request.BoxId,
            memoryStream,
            request.File.FileName,
            boxTagsMemoryStream,
            boxTagsFileName
        );

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Confirm and save reviewed panel extraction data
    /// </summary>
    [HttpPost("{projectId}/panels/confirm-extraction")]
    public async Task<IActionResult> ConfirmPanelExtraction(
        Guid projectId,
        [FromBody] ConfirmPanelExtractionRequest request,
        CancellationToken cancellationToken)
    {
        if (projectId == Guid.Empty)
            return BadRequest("Project ID is required");

        if (request == null)
            return BadRequest("Request body is required");

        var boxTagMappings = request.BoxTagMappings
            .Select(m => new BoxTagMappingDto(m.BoxTag, m.BoxId, m.NewBoxName))
            .ToList();

        var command = new ConfirmPanelExtractionCommand(
            projectId,
            request.BoxId,
            request.PanelTypes,
            request.BoxPanels,
            boxTagMappings
        );

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

public class UploadProjectImagesRequest
{
    public IFormFile? ContractorImage { get; set; }
    public IFormFile? SubContractorImage { get; set; }
    public IFormFile? ClientImage { get; set; }
}

public class ConfirmPanelExtractionRequest
{
    public Guid? BoxId { get; set; }
    public List<ReviewPanelTypeDto> PanelTypes { get; set; } = new();
    public List<ReviewBoxPanelDto> BoxPanels { get; set; } = new();
    public List<BoxTagMapping> BoxTagMappings { get; set; } = new();
}

public class BoxTagMapping
{
    public string BoxTag { get; set; } = string.Empty;
    public Guid? BoxId { get; set; } // null if creating new box
    public string? NewBoxName { get; set; } // only if creating new box
}
