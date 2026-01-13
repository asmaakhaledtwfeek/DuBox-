using Dubox.Application.Features.Boxes.Commands;
using Dubox.Application.Features.Boxes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoxesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoxesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBoxes(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllBoxesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetBoxesByProject(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxesByProjectQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("factory/{factoryId}")]
    public async Task<IActionResult> GetBoxesByFactory(
        Guid factoryId, 
        [FromQuery] bool includeDispatched = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetBoxesByFactoryQuery(factoryId, includeDispatched), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("project/{projectId}/box-type-stats")]
    public async Task<IActionResult> GetBoxTypeStatsByProject(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxTypeStatsByProjectQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxId}/logs")]
    public async Task<IActionResult> GetBoxLogs(
        Guid boxId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? action = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetBoxLogsQuery(
            boxId,
            pageNumber,
            pageSize,
            searchTerm,
            action,
            fromDate,
            toDate
        ), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxId}")]
    public async Task<IActionResult> GetBoxById(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxByIdQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBox([FromBody] CreateBoxCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetBoxById), new { boxId = result.Data!.BoxId }, result) : BadRequest(result);
    }


    [HttpPut("{boxId}")]
    public async Task<IActionResult> UpdateBox(Guid boxId, [FromBody] UpdateBoxCommand command, CancellationToken cancellationToken)
    {
        if (boxId != command.BoxId)
            return BadRequest("Box ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{boxId}/status")]
    public async Task<IActionResult> UpdateBoxStatus(Guid boxId, [FromBody] UpdateBoxStatusCommand command, CancellationToken cancellationToken)
    {
        if (boxId != command.BoxId)
            return BadRequest("Box ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{boxId}")]
    public async Task<IActionResult> DeleteBox(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteBoxCommand(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("template")]
    public async Task<IActionResult> DownloadTemplate([FromQuery] Guid projectId, CancellationToken cancellationToken)
    {
        if (projectId == Guid.Empty)
            return BadRequest("Project ID is required");

        var result = await _mediator.Send(new GenerateBoxesTemplateQuery(projectId), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return File(result.Data!,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "BoxesImportTemplate.xlsx");
    }


    [HttpPost("import-excel")]
    [RequestSizeLimit(10_485_760)] // 10 MB
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportFromExcel([FromQuery] Guid? projectId, [FromForm] IFormFile? file, CancellationToken cancellationToken)
    {
        // Support projectId from both query string and FormData for flexibility
        Guid actualProjectId;
        if (projectId.HasValue)
        {
            actualProjectId = projectId.Value;
        }
        else if (Request.HasFormContentType)
        {
            var form = await Request.ReadFormAsync(cancellationToken);
            if (!Guid.TryParse(form["projectId"].ToString(), out actualProjectId))
            {
                return BadRequest("ProjectId is required and must be a valid GUID");
            }
        }
        else
        {
            return BadRequest("ProjectId is required");
        }

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
        var command = new ImportBoxesFromExcelCommand(actualProjectId, memoryStream, actualFile.FileName);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpGet("generate-qrcode/{boxId}")]
    public async Task<IActionResult> GenerateBoxQRCode(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GenerateBoxQRCodeByIdQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Generate QR code with public URL for a box
    /// Returns both the QR code image and the public URL
    /// </summary>
    [HttpGet("{boxId}/qrcode-with-url")]
    public async Task<IActionResult> GetBoxQRCodeWithUrl(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxQRCodeWithUrlQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPut("define-box-material{boxId}")]
    public async Task<IActionResult> DefineBoxMaterialRequired(Guid boxId, [FromBody] DefineBoxMaterialRequirementCommand command, CancellationToken cancellationToken)
    {
        if (boxId != command.BoxId)
            return BadRequest("Box ID mismatch");
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPut("allocate-box-material")]
    public async Task<IActionResult> AllocateBoxMaterial([FromBody] AllocateBoxMaterialCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxId}/drawing")]
    public async Task<IActionResult> GetBoxDrawing(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxDrawingQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{boxId}/attachments")]
    public async Task<IActionResult> GetBoxAttachments(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxAttachmentsQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

   
}

