using Dubox.Application.Features.QualityIssues.Commands;
using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        [HttpPost]
        [Consumes("multipart/form-data", "application/json")]
        [RequestSizeLimit(50_000_000)] 
        public async Task<IActionResult> CreateQualityIssue(
            CancellationToken cancellationToken)
        {
            Guid boxId;
            IssueTypeEnum issueType;
            SeverityEnum severity;
            string issueDescription;
            Guid? assignedTo = null;
            DateTime? dueDate = null;
            List<string>? imageUrls = null;
            List<IFormFile>? files = null;
            List<string>? fileNames = null;
            if (Request.HasFormContentType)
            {
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
                
                // AssignedTo is optional - only parse if provided
                var assignedToValue = form["AssignedTo"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(assignedToValue) && Guid.TryParse(assignedToValue, out var assignedToTemp))
                {
                    assignedTo = assignedToTemp;
                }

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
                var formFiles = form.Files;
                if (formFiles.Count > 0)
                {
                    files = formFiles.Where(f => f != null && f.Length > 0).ToList();
                    fileNames = files.Select(f => f.FileName).ToList();
                }
            }
            else
            {
                return BadRequest("Only multipart/form-data is supported for file uploads");
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
                Files: files,
               FileNames: fileNames
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

            List<string>? fileNames = null;
            if (Files != null && Files.Count > 0)
            {
                fileNames = Files
                    .Where(f => f != null && f.Length > 0)
                    .Select(f => f.FileName)
                    .ToList();
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
                Files: Files,
                ImageUrls: validImageUrls,
                FileNames: fileNames
            );

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{issueId}/assign")]
        public async Task<IActionResult> AssignQualityIssueToTeam(
            Guid issueId,
            [FromBody] AssignQualityIssueToTeamRequest request,
            CancellationToken cancellationToken)
        {
            if (issueId != request.IssueId)
                return BadRequest("Issue ID mismatch");

            var command = new AssignQualityIssueToTeamCommand(
                IssueId: issueId,
                TeamId: request.TeamId,
                TeamMemberId:request.TeamMemberId
            );

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }

    public class AssignQualityIssueToTeamRequest
    {
        public Guid IssueId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? TeamMemberId{ get; set; }
    }

    public class CreateQualityIssueJsonRequest
    {
        public Guid BoxId { get; set; }
        public IssueTypeEnum IssueType { get; set; }
        public SeverityEnum Severity { get; set; }
        public string IssueDescription { get; set; } = string.Empty;
        public Guid? AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<byte[]>? Files { get; set; }
    }
}
