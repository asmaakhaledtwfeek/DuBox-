using Dubox.Application.Features.WIRCheckpoints.Commands;
using Dubox.Application.Features.WIRCheckpoints.Queries;
using Dubox.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Dubox.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WIRCheckPointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WIRCheckPointsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateWIRCheckPoints([FromBody] CreateWIRCheckpointCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("predefined-checklist-items/{checkpointId}")]
        public async Task<IActionResult> GetPredefinedChecklistItemsByWIRCode(Guid checkpointId, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetPredefinedChecklistItemsByWIRCodeQuery(checkpointId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{wirId}/checklist-items")]
        public async Task<IActionResult> AddChecklistItems(Guid wirId, [FromBody] AddChecklistItemsCommand command, CancellationToken cancellationToken)
        {
            if (wirId != command.WIRId)
                return BadRequest("WIR Check point ID mismatch");
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("checklist-items/{checklistItemId}")]
        public async Task<IActionResult> UpdateChecklistItem(Guid checklistItemId, [FromBody] UpdateChecklistItemCommand command, CancellationToken cancellationToken)
        {
            if (checklistItemId != command.ChecklistItemId)
                return BadRequest("Checklist item ID mismatch");
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("checklist-items/{checklistItemId}")]
        public async Task<IActionResult> DeleteChecklistItem(Guid checklistItemId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteChecklistItemCommand(checklistItemId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{wirId}/quality-issue")]
        [Consumes("multipart/form-data", "application/json")]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> AddQualityIssue(
            Guid wirId,
            CancellationToken cancellationToken)
        {
            IssueTypeEnum issueType;
            SeverityEnum severity;
            string issueDescription;
            string? assignedTo = null;
            DateTime? dueDate = null;
            List<string>? imageUrls = null;
            List<byte[]>? fileBytes = null;

            // Check if request is form-data or JSON
            if (Request.HasFormContentType)
            {
                // Handle multipart/form-data
                var form = await Request.ReadFormAsync(cancellationToken);

                if (!Enum.TryParse<IssueTypeEnum>(form["IssueType"].ToString(), out issueType))
                    return BadRequest("Invalid IssueType");

                if (!Enum.TryParse<SeverityEnum>(form["Severity"].ToString(), out severity))
                    return BadRequest("Invalid Severity");

                issueDescription = form["IssueDescription"].ToString();
                if (string.IsNullOrWhiteSpace(issueDescription))
                    return BadRequest("IssueDescription is required");

                assignedTo = form["AssignedTo"].ToString();
                if (string.IsNullOrWhiteSpace(assignedTo))
                    assignedTo = null;

                if (DateTime.TryParse(form["DueDate"].ToString(), out var parsedDueDate))
                    dueDate = parsedDueDate;

                // Get ImageUrls from form
                if (form.ContainsKey("ImageUrls"))
                {
                    imageUrls = form["ImageUrls"].Where(url => !string.IsNullOrWhiteSpace(url)).ToList();
                    if (imageUrls.Count == 0)
                        imageUrls = null;
                }

                // Convert uploaded files to byte[]
                var files = form.Files.Where(f => f.Name == "Files" && f.Length > 0).ToList();
                if (files.Count > 0)
                {
                    fileBytes = new List<byte[]>();
                    foreach (var file in files)
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms, cancellationToken);
                        fileBytes.Add(ms.ToArray());
                    }
                }
            }
            else
            {
                // Handle JSON
                AddQualityIssueCommand? jsonCommand = null;
                try
                {
                    using var reader = new StreamReader(Request.Body);
                    var body = await reader.ReadToEndAsync();
                    jsonCommand = JsonSerializer.Deserialize<AddQualityIssueCommand>(body, new JsonSerializerOptions
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

                if (wirId != jsonCommand.WIRId)
                    return BadRequest("WIR checkpoint ID mismatch");

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

            var command = new AddQualityIssueCommand(
                WIRId: wirId,
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



        [HttpPut("{wirId}/review")]
        [Consumes("multipart/form-data", "application/json")]
        [RequestSizeLimit(50_000_000)] // 50 MB for multiple images
        public async Task<IActionResult> ReviewWIRCheckPoint(
            Guid wirId,
            [FromForm] WIRCheckpointStatusEnum Status,
            [FromForm] string? Comment,
            [FromForm] string? InspectorRole,
            [FromForm] List<IFormFile>? Files,
            [FromForm] List<string>? ImageUrls,
            [FromForm] string? ItemsJson, // JSON string for complex object
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

            // Parse Items from JSON string
            List<ChecklistItemForReview>? items = null;
            if (!string.IsNullOrWhiteSpace(ItemsJson))
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    // Use JsonStringEnumConverter to handle string enum values from frontend
                    // Frontend sends "Pending", "Pass", "Fail" as strings, backend enum has numeric values
                    options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

                    items = JsonSerializer.Deserialize<List<ChecklistItemForReview>>(ItemsJson, options);
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        message = "Invalid Items JSON format",
                        error = ex.Message,
                        details = ex.InnerException?.Message,
                        itemsJson = ItemsJson?.Substring(0, Math.Min(500, ItemsJson.Length)) // First 500 chars for debugging
                    });
                }
            }

            if (items == null || !items.Any())
            {
                return BadRequest("Items are required");
            }

            var command = new ReviewWIRCheckPointCommand(
                WIRId: wirId,
                Status: Status,
                Comment: Comment,
                InspectorRole: InspectorRole,
                Files: fileBytes,
                ImageUrls: validImageUrls,
                Items: items
            );

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{wirCheckPointId}")]
        public async Task<IActionResult> GetWIRcheckPointById(Guid wirCheckPointId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetWIRCheckpointByIdQuery(wirCheckPointId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("box/{boxId}")]
        public async Task<IActionResult> GetWIRcheckPointsByBox(Guid boxId, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetWIRCheckpointsByBoxIdQuery(boxId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWIRcheckPoints([FromQuery] GetWIRCheckpointsQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Auto-generate all 6 WIRs (with predefined checklist items) for a box
        /// </summary>
        [HttpPost("generate-for-box/{boxId}")]
        public async Task<IActionResult> GenerateWIRsForBox(Guid boxId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Dubox.Application.Features.WIRCheckpoints.Commands.GenerateWIRsForBoxCommand(boxId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get all WIRs for a box with their checklist items grouped by category
        /// </summary>
        [HttpGet("box/{boxId}/with-checklist")]
        public async Task<IActionResult> GetWIRsByBoxWithChecklist(Guid boxId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Dubox.Application.Features.WIRCheckpoints.Queries.GetWIRsByBoxWithChecklistQuery(boxId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
