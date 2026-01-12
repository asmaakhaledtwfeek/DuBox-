using Dubox.Application.Features.BoxDrawings.Commands;
using Dubox.Application.Features.BoxDrawings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoxDrawingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoxDrawingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(52_428_800)] // 50 MB
    public async Task<IActionResult> UploadBoxDrawing(
        [FromForm] Guid BoxId,
        [FromForm] string? DrawingUrl,
        [FromForm] IFormFile? File,
        CancellationToken cancellationToken)
    {
        var command = new UploadBoxDrawingCommand(
          BoxId: BoxId,
          DrawingUrl: DrawingUrl,
          File: File, 
          FileName: File?.FileName
      );

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

   
    [HttpGet("box/{boxId}")]
    public async Task<IActionResult> GetBoxDrawings(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxDrawingsQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{drawingId}/file")]
    public async Task<IActionResult> DownloadBoxDrawing(Guid drawingId, CancellationToken cancellationToken)
    {
        var drawing = await _mediator.Send(new GetBoxDrawingByIdQuery(drawingId), cancellationToken);
        
        if (!drawing.IsSuccess || drawing.Data == null)
        {
            return NotFound("Drawing not found");
        }

        var drawingData = drawing.Data;

        // If it's a URL type, redirect to the URL
        if (drawingData.FileType == "url" && !string.IsNullOrEmpty(drawingData.DrawingUrl))
            return Redirect(drawingData.DrawingUrl);

        return NotFound("File data not found");
    }

    [HttpDelete("{drawingId}")]
    public async Task<IActionResult> DeleteBoxDrawing(Guid drawingId, CancellationToken cancellationToken)
    {
     
        return Ok(new { message = "Delete box drawing endpoint - to be implemented" });
    }
}

