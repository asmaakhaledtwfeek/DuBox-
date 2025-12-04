using Dubox.Application.Features.QualityIssues.Commands;
using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QualityIssuesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QualityIssuesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{qualityIssueId}")]
        public async Task<IActionResult> GetQualityIssueById(Guid qualityIssueId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetQualityIssueByIdQuery(qualityIssueId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("box/{boxId}")]
        public async Task<IActionResult> GetQualityIssuesByBox(Guid boxId, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetQualityIssuesByBoxIdQuery(boxId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllQualityIssues([FromQuery] GetQualityIssuesQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        
        [HttpPut("{issueId}/status")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(50_000_000)] // 50 MB for multiple images
        public async Task<IActionResult> UpdateQualityIssueStatus(
            Guid issueId,
            [FromForm] QualityIssueStatusEnum Status,
            [FromForm] string? ResolutionDescription,
            [FromForm] List<IFormFile>? Files,
            [FromForm] List<string>? ImageUrls,
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
                validImageUrls = ImageUrls
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .Select(url => url!.Trim())
                    .ToList();
            }

            var command = new UpdateQualityIssueStatusCommand(
                IssueId: issueId,
                Status: Status,
                ResolutionDescription: ResolutionDescription,
                Files: fileBytes,
                ImageUrls: validImageUrls
            );

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
