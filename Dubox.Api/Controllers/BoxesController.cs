using Dubox.Application.Features.Boxes.Commands;
using Dubox.Application.Features.Boxes.Queries;
using Dubox.Application.Features.BoxPanels.Commands;
using Dubox.Application.Features.Projects.Queries;
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
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetBoxesByFactoryQuery(factoryId, includeDispatched, page, pageSize), cancellationToken);
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
        [FromQuery] Guid? changedBy = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetBoxLogsQuery(
            boxId,
            pageNumber,
            pageSize,
            searchTerm,
            action,
            fromDate,
            toDate,
            changedBy
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

    /// <summary>

    [HttpPost("{boxId}/duplicate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DuplicateBox(
        Guid boxId,
        [FromBody] DuplicateBoxRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DuplicateBoxCommand(
            boxId
        );

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
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

    [HttpPatch("{boxId}/delivery-info")]
    public async Task<IActionResult> UpdateBoxDeliveryInfo(Guid boxId, [FromBody] UpdateBoxDeliveryInfoCommand command, CancellationToken cancellationToken)
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

        // Get project details to construct filename
        var projectResult = await _mediator.Send(new GetProjectByIdQuery(projectId), cancellationToken);
        if (!projectResult.IsSuccess || projectResult.Data == null)
            return BadRequest("Project not found");

        var result = await _mediator.Send(new GenerateBoxesTemplateQuery(projectId), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        // Construct filename: ProjectCode-ProjectName-BoxesTemplates.xlsx
        var fileName = $"{projectResult.Data.ProjectCode}-{projectResult.Data.ProjectName}-BoxesTemplates.xlsx";

        return File(result.Data!,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
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

    [HttpPut("panels/{boxPanelId}/status")]
    public async Task<IActionResult> UpdateBoxPanelStatus(Guid boxPanelId, [FromBody] UpdateBoxPanelStatusCommand command, CancellationToken cancellationToken)
    {
        if (boxPanelId != command.BoxPanelId)
            return BadRequest("Box Panel ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // Panel Scanning & Tracking
    [HttpPost("panels/scan")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ScanPanelBarcode(
        [FromBody] ScanPanelBarcodeCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // Panel Approval - First Approval
    [HttpPost("panels/{boxPanelId}/first-approval")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApprovePanelFirstApproval(
        Guid boxPanelId,
        [FromBody] ApprovePanelFirstApprovalCommand command,
        CancellationToken cancellationToken = default)
    {
        if (boxPanelId != command.BoxPanelId)
            return BadRequest("Panel ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // Panel Approval - Second Approval
    [HttpPost("panels/{boxPanelId}/second-approval")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApprovePanelSecondApproval(
        Guid boxPanelId,
        [FromBody] ApprovePanelSecondApprovalCommand command,
        CancellationToken cancellationToken = default)
    {
        if (boxPanelId != command.BoxPanelId)
            return BadRequest("Panel ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

public record DuplicateBoxRequest(
    bool IncludeActivities = true,
    bool IncludeDrawings = false
);

