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
        // Validate that at least one input is provided
        if (string.IsNullOrWhiteSpace(DrawingUrl) && (File == null || File.Length == 0))
        {
            return BadRequest("Either a drawing URL or a file must be provided.");
        }

        byte[]? fileBytes = null;
        string? fileName = null;

        // Process file if provided
        if (File != null && File.Length > 0)
        {
            // Validate file extension
            var extension = Path.GetExtension(File.FileName).ToLowerInvariant();
            if (extension != ".pdf" && extension != ".dwg")
            {
                return BadRequest("Only PDF and DWG files are allowed.");
            }

            // Validate file size (50 MB max)
            if (File.Length > 52_428_800)
            {
                return BadRequest("File size cannot exceed 50 MB.");
            }

            fileName = File.FileName;

            // Convert file to byte array
            using var memoryStream = new MemoryStream();
            await File.CopyToAsync(memoryStream, cancellationToken);
            fileBytes = memoryStream.ToArray();
        }

        var command = new UploadBoxDrawingCommand(
            BoxId: BoxId,
            DrawingUrl: DrawingUrl,
            File: fileBytes,
            FileName: fileName
        );

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all drawings for a specific box
    /// </summary>
    [HttpGet("box/{boxId}")]
    public async Task<IActionResult> GetBoxDrawings(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxDrawingsQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Download a box drawing file
    /// </summary>
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
        {
            return Redirect(drawingData.DrawingUrl);
        }

        // If it's a file type, return the file data
        if (!string.IsNullOrEmpty(drawingData.FileData))
        {
            // Remove data URL prefix if present (e.g., "data:application/pdf;base64,")
            var base64Data = drawingData.FileData;
            if (base64Data.Contains(","))
            {
                var parts = base64Data.Split(',');
                if (parts.Length == 2 && parts[0].StartsWith("data:"))
                {
                    base64Data = parts[1]; // Get the actual Base64 part
                }
            }
            
            var fileBytes = Convert.FromBase64String(base64Data);
            var extension = drawingData.FileExtension?.ToLowerInvariant() ?? "";
            var contentType = extension == ".pdf" ? "application/pdf" : 
                            extension == ".dwg" ? "application/acad" : 
                            "application/octet-stream";
            
            var fileName = drawingData.OriginalFileName ?? $"drawing{extension}";
            
            return File(fileBytes, contentType, fileName);
        }

        return NotFound("File data not found");
    }

    /// <summary>
    /// Delete a box drawing
    /// </summary>
    [HttpDelete("{drawingId}")]
    public async Task<IActionResult> DeleteBoxDrawing(Guid drawingId, CancellationToken cancellationToken)
    {
        // This would require a command handler - for now, returning a placeholder
        // You can implement DeleteBoxDrawingCommand and Handler if needed
        return Ok(new { message = "Delete box drawing endpoint - to be implemented" });
    }
}

