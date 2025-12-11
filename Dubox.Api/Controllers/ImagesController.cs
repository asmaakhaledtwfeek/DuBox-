using Dubox.Application.DTOs;
using Dubox.Application.Features.Images.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("{category}/{imageId}")]
    public async Task<IActionResult> GetImage(ImageCategory category, Guid imageId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetImageQuery(category, imageId), cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("{category}/{imageId}/file")]
    public async Task<IActionResult> GetImageFile(ImageCategory category, Guid imageId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetImageQuery(category, imageId), cancellationToken);

        if (!result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }

        var imageData = result.Data;

        // Parse base64 data
        var base64Data = imageData.ImageData;
        if (base64Data.Contains(","))
        {
            base64Data = base64Data.Split(',')[1];
        }

        try
        {
            var bytes = Convert.FromBase64String(base64Data);
            var contentType = GetContentType(imageData.ImageType, imageData.OriginalName);
            return File(bytes, contentType, imageData.OriginalName);
        }
        catch
        {
            return BadRequest("Invalid image data");
        }
    }

    private static string GetContentType(string imageType, string? originalName)
    {
        // Try to determine content type from imageType field
        if (!string.IsNullOrEmpty(imageType))
        {
            if (imageType.StartsWith("image/"))
                return imageType;

            return imageType.ToLowerInvariant() switch
            {
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "gif" => "image/gif",
                "webp" => "image/webp",
                "bmp" => "image/bmp",
                "svg" => "image/svg+xml",
                _ => "image/jpeg" // default
            };
        }

        // Try from file extension
        if (!string.IsNullOrEmpty(originalName))
        {
            var ext = Path.GetExtension(originalName).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                ".svg" => "image/svg+xml",
                _ => "image/jpeg"
            };
        }

        return "image/jpeg";
    }
}

