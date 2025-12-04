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
    [RequestSizeLimit(10_485_760)] // 10 MB
    public async Task<IActionResult> CreateProgressUpdate(
    [FromForm] Guid BoxId,
    [FromForm] Guid BoxActivityId,
    [FromForm] decimal ProgressPercentage,
    [FromForm] string? WorkDescription,
    [FromForm] string? IssuesEncountered,
    [FromForm] double? Latitude,
    [FromForm] double? Longitude,
    [FromForm] string? LocationDescription,
    [FromForm] IFormFile? File,
    [FromForm] string? ImageUrl,
    [FromForm] string UpdateMethod,
    [FromForm] string? DeviceInfo,
    CancellationToken cancellationToken)
    {
        var imageBytes = await _imageProcessingService
            .GetImageBytesAsync(File, ImageUrl, cancellationToken);

        var command = new CreateProgressUpdateCommand(
            BoxId,
            BoxActivityId,
            ProgressPercentage,
            WorkDescription,
            IssuesEncountered,
            Latitude,
            Longitude,
            LocationDescription,
            imageBytes,
            UpdateMethod,
            DeviceInfo
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
}

