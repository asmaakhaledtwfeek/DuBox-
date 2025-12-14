using Dubox.Application.Features.QualityIssues.Commands;
using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Dubox.Api.Controllers
{
    [Route("api/qualityissues")]
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

        /// <summary>
        /// Create a quality issue directly for a box (without WIR checkpoint)
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data", "application/json")]
        [RequestSizeLimit(50_000_000)] // 50 MB for multiple images
        public async Task<IActionResult> CreateQualityIssue(
            CancellationToken cancellationToken)
        {
            Guid boxId;
            IssueTypeEnum issueType;
            SeverityEnum severity;
            string issueDescription;
            string? assignedTo = null;
            DateTime? dueDate = null;
            List<string>? imageUrls = null;
            List<byte[]>? fileBytes = null;

            // Check if request is multipart/form-data or JSON
            if (Request.HasFormContentType)
            {
                // Handle multipart/form-data
                var form = await Request.ReadFormAsync(cancellationToken);

                if (!Guid.TryParse(form["BoxId"].ToString(), out boxId))
                    return BadRequest("Invalid or missing BoxId");

                if (!Enum.TryParse<IssueTypeEnum>(form["IssueType"].ToString(), out issueType))
                    return BadRequest("Invalid or missing IssueType");

                if (!Enum.TryParse<SeverityEnum>(form["Severity"].ToString(), out severity))
                    return BadRequest("Invalid or missing Severity");

                issueDescription = form["IssueDescription"].ToString();
                if (string.IsNullOrWhiteSpace(issueDescription))
                    return BadRequest("IssueDescription is required");

                assignedTo = form["AssignedTo"].ToString();
                if (string.IsNullOrWhiteSpace(assignedTo))
                    assignedTo = null;

                if (DateTime.TryParse(form["DueDate"].ToString(), out var parsedDueDate))
                    dueDate = parsedDueDate;

                // Handle image URLs
                var imageUrlsFromForm = form["ImageUrls"];
                if (imageUrlsFromForm.Count > 0)
                {
                    imageUrls = imageUrlsFromForm
                        .Where(url => !string.IsNullOrWhiteSpace(url))
                        .Select(url => url!.Trim())
                        .ToList();
                }

                // Handle file uploads
                var files = form.Files;
                if (files.Count > 0)
                {
                    fileBytes = new List<byte[]>();
                    foreach (var file in files.Where(f => f != null && f.Length > 0))
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms, cancellationToken);
                        fileBytes.Add(ms.ToArray());
                    }
                }
            }
            else
            {
                // Handle JSON request
                string requestBody;
                using (var reader = new StreamReader(Request.Body))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(requestBody))
                    return BadRequest("Request body is required");

                CreateQualityIssueJsonRequest? jsonCommand;
                try
                {
                    jsonCommand = JsonSerializer.Deserialize<CreateQualityIssueJsonRequest>(requestBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch
                {
                    return BadRequest("Invalid JSON format");
                }

                if (jsonCommand == null)
                    return BadRequest("Request body is required");

                boxId = jsonCommand.BoxId;
                issueType = jsonCommand.IssueType;
                severity = jsonCommand.Severity;
                issueDescription = jsonCommand.IssueDescription;
                assignedTo = jsonCommand.AssignedTo;
                dueDate = jsonCommand.DueDate;
                imageUrls = jsonCommand.ImageUrls;
                fileBytes = jsonCommand.Files;
            }

            // Validate and clean ImageUrls
            List<string>? validImageUrls = null;
            if (imageUrls != null && imageUrls.Count > 0)
            {
                validImageUrls = imageUrls.Where(url => !string.IsNullOrWhiteSpace(url)).ToList();
                if (validImageUrls.Count == 0)
                {
                    validImageUrls = null;
                }
            }

            var command = new CreateQualityIssueCommand(
                BoxId: boxId,
                IssueType: issueType,
                Severity: severity,
                IssueDescription: issueDescription,
                AssignedTo: assignedTo,
                DueDate: dueDate,
                ImageUrls: validImageUrls,
                Files: fileBytes
            );

            var result = await _mediator.Send(command, cancellationToken);

            // Check for specific database conflict errors and return 409
            if (!result.IsSuccess && result.Message?.Contains("Database error", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Conflict(result);
            }

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

    /// <summary>
    /// JSON request model for creating quality issue
    /// </summary>
    public class CreateQualityIssueJsonRequest
    {
        public Guid BoxId { get; set; }
        public IssueTypeEnum IssueType { get; set; }
        public SeverityEnum Severity { get; set; }
        public string IssueDescription { get; set; } = string.Empty;
        public string? AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<byte[]>? Files { get; set; }
    }
}
