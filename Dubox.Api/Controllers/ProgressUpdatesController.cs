using Dubox.Application.Features.ProgressUpdates.Commands;
using Dubox.Application.Features.ProgressUpdates.Queries;
using Dubox.Domain.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressUpdatesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IImageProcessingService _imageProcessingService;
    public ProgressUpdatesController(IMediator mediator, IImageProcessingService imageProcessingService)
    {
        _mediator = mediator;
        _imageProcessingService = imageProcessingService;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(50_000_000)] // 50 MB for multiple images
    public async Task<IActionResult> CreateProgressUpdate(
    [FromForm] Guid BoxId,
    [FromForm] Guid BoxActivityId,
    [FromForm] decimal ProgressPercentage,
    [FromForm] string? WorkDescription,
    [FromForm] string? IssuesEncountered,
    [FromForm] double? Latitude,
    [FromForm] double? Longitude,
    [FromForm] string? LocationDescription,
    [FromForm] List<IFormFile>? Files,
    [FromForm] List<string>? ImageUrls,
    [FromForm] string UpdateMethod,
    [FromForm] string? DeviceInfo,
    [FromForm] string? WirBay,
    [FromForm] string? WirRow,
    [FromForm] string? WirPosition,
    CancellationToken cancellationToken)
    {
        List<byte[]>? fileBytes = null;
        if (Files != null && Files.Count > 0)
        {
            fileBytes = new List<byte[]>();
            foreach (var file in Files.Where(f => f != null && f.Length > 0))
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms, cancellationToken);
                fileBytes.Add(ms.ToArray());
            }
        }

        List<string>? validImageUrls = null;
        if (ImageUrls != null && ImageUrls.Count > 0)
        {
            validImageUrls = ImageUrls.Where(url => !string.IsNullOrWhiteSpace(url)).ToList();
            if (validImageUrls.Count == 0)
            {
                validImageUrls = null;
            }
        }

        var command = new CreateProgressUpdateCommand(
            BoxId,
            BoxActivityId,
            ProgressPercentage,
            WorkDescription,
            IssuesEncountered,
            Latitude,
            Longitude,
            LocationDescription,
            fileBytes,
            validImageUrls,
            UpdateMethod,
            DeviceInfo,
            WirBay,
            WirRow,
            WirPosition
        );

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }



    [HttpGet("box/{boxId}")]
    public async Task<IActionResult> GetProgressUpdatesByBox(
        Guid boxId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? activityName = null,
        [FromQuery] string? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] Guid? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProgressUpdatesByBoxQuery(boxId)
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            ActivityName = activityName,
            Status = status,
            FromDate = fromDate,
            ToDate = toDate,
            UpdatedBy = updatedBy
        };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }


    [HttpGet("activity/{boxActivityId}")]
    public async Task<IActionResult> GetProgressUpdatesByActivity(Guid boxActivityId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProgressUpdatesByActivityQuery(boxActivityId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{progressUpdateId}")]
    public async Task<IActionResult> GetProgressUpdateById(Guid progressUpdateId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProgressUpdateByIdQuery(progressUpdateId), cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}

