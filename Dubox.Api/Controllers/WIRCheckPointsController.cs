using Dubox.Application.Features.WIRCheckpoints.Commands;
using Dubox.Application.Features.WIRCheckpoints.Queries;
using Dubox.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        [Consumes("multipart/form-data", "application/json")]
        [RequestSizeLimit(50_000_000)] // 50 MB for multiple images
        public async Task<IActionResult> CreateWIRCheckPoints(
            CancellationToken cancellationToken)
        {
            CreateWIRCheckpointCommand command;
            List<IFormFile>? files = null;
            List<string>? imageUrls = null;
            List<string>? fileNames = null;

            if (Request.HasFormContentType)
            {
                var form = await Request.ReadFormAsync(cancellationToken);

                // Parse required fields
                if (!Guid.TryParse(form["BoxActivityId"].ToString(), out var boxActivityId))
                {
                    return BadRequest("BoxActivityId is required and must be a valid GUID");
                }

                var wirNumber = form["WIRNumber"].ToString();
                if (string.IsNullOrWhiteSpace(wirNumber))
                {
                    return BadRequest("WIRNumber is required");
                }

                var wirName = form["WIRName"].ToString();
                var wirDescription = form["WIRDescription"].ToString();
                var attachmentPath = form["AttachmentPath"].ToString();
                var comments = form["Comments"].ToString();

                var formFiles = form.Files.Where(f => f.Name == "Files" && f.Length > 0).ToList();
                if (formFiles.Count > 0)
                {
                    files = formFiles;
                    fileNames = formFiles.Select(f => f.FileName).ToList();
                }

                var imageUrlsFromForm = form["ImageUrls"];
                if (imageUrlsFromForm.Count > 0)
                {
                    imageUrls = imageUrlsFromForm
                        .Where(url => !string.IsNullOrWhiteSpace(url))
                        .Select(url => url!.Trim())
                        .ToList();

                    if (imageUrls.Count == 0)
                    {
                        imageUrls = null;
                    }
                }
                command = new CreateWIRCheckpointCommand(
                          BoxActivityId: boxActivityId,
                          WIRNumber: wirNumber,
                          WIRName: string.IsNullOrWhiteSpace(wirName) ? null : wirName,
                          WIRDescription: string.IsNullOrWhiteSpace(wirDescription) ? null : wirDescription,
                          AttachmentPath: string.IsNullOrWhiteSpace(attachmentPath) ? null : attachmentPath,
                          Comments: string.IsNullOrWhiteSpace(comments) ? null : comments,
                          Files: files, 
                         ImageUrls: imageUrls,
                         FileNames: fileNames
                     );
            }
            else
            {
                // Handle JSON (backward compatibility)
                CreateWIRCheckpointCommand? jsonCommand = null;
                try
                {
                    using var reader = new StreamReader(Request.Body);
                    var body = await reader.ReadToEndAsync();
                    jsonCommand = JsonSerializer.Deserialize<CreateWIRCheckpointCommand>(body, new JsonSerializerOptions
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

                command = jsonCommand with
                {
                    Files = null,
                    ImageUrls = null
                };
            }

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("predefined-checklist-items/{checkpointId}")]
        public async Task<IActionResult> GetPredefinedChecklistItemsByWIRCheckpointId(Guid checkpointId, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetPredefinedChecklistItemsByWIRCheckpointIdQuery(checkpointId), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("predefined-checklist-items/wir/{wIRCode}")]
        public async Task<IActionResult> GetPredefinedChecklistItemsByWIRCheckpointId(string wIRCode, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetPredefinedChecklistItemsByWIRCodeQuery(wIRCode), cancellationToken);
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
            Guid? assignedTo = null;
            Guid? assignedToUserId = null;
            Guid? ccUserId = null;
            DateTime? dueDate = null;
            List<string>? imageUrls = null;
            List<IFormFile>? files = null; 
            List<string>? fileNames = null;

            if (Request.HasFormContentType)
            {
                var form = await Request.ReadFormAsync(cancellationToken);

                if (!Enum.TryParse<IssueTypeEnum>(form["IssueType"].ToString(), out issueType))
                    return BadRequest("Invalid IssueType");

                if (!Enum.TryParse<SeverityEnum>(form["Severity"].ToString(), out severity))
                    return BadRequest("Invalid Severity");

                issueDescription = form["IssueDescription"].ToString();
                if (string.IsNullOrWhiteSpace(issueDescription))
                    return BadRequest("IssueDescription is required");

                var assignedToValue = form["AssignedTo"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(assignedToValue))
                {
                    if (!Guid.TryParse(assignedToValue, out var assignedToTemp))
                        return BadRequest("Invalid AssignedTo");

                    assignedTo = assignedToTemp;
                }

                if (Guid.TryParse(form["AssignedToUserId"].FirstOrDefault(), out var assignedToUserTemp))
                    assignedToUserId = assignedToUserTemp;

                if (Guid.TryParse(form["CCUserId"].FirstOrDefault(), out var ccUserTemp))
                    ccUserId = ccUserTemp;

                if (DateTime.TryParse(form["DueDate"].ToString(), out var parsedDueDate))
                    dueDate = parsedDueDate;

                var imageUrlsFromForm = form["ImageUrls"];
                if (imageUrlsFromForm.Count > 0)
                {
                    imageUrls = imageUrlsFromForm
                        .Where(url => !string.IsNullOrWhiteSpace(url))
                        .Select(url => url!.Trim())
                        .ToList();
                }

                var formFiles = form.Files.Where(f => f.Name == "Files" && f.Length > 0).ToList();
                if (formFiles.Count > 0)
                {
                    files = formFiles;
                    fileNames = formFiles.Select(f => f.FileName).ToList();
                }
            }
            else
            {
                AddQualityIssueCommand? jsonCommand = null;
                try
                {
                    using var reader = new StreamReader(Request.Body);
                    var body = await reader.ReadToEndAsync();
                    jsonCommand = JsonSerializer.Deserialize<AddQualityIssueCommand>(body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
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
                assignedToUserId = jsonCommand.AssignedToUserId;
                ccUserId = jsonCommand.CCUserId;
                dueDate = jsonCommand.DueDate;
                imageUrls = jsonCommand.ImageUrls;
                files = jsonCommand.Files; 
                fileNames = jsonCommand.FileNames;
            }

            List<string>? validImageUrls = null;
            if (imageUrls != null && imageUrls.Count > 0)
            {
                validImageUrls = imageUrls
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .ToList();

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
                AssignedToUserId: assignedToUserId,
                CCUserId: ccUserId,
                DueDate: dueDate,
                ImageUrls: validImageUrls,
                Files: files, 
                FileNames: fileNames
            );
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess && result.Message?.Contains("Database error", StringComparison.OrdinalIgnoreCase) == true)
                return Conflict(result);

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
                    .ToList();

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
                    options.Converters.Add(new JsonStringEnumConverter());

                    items = JsonSerializer.Deserialize<List<ChecklistItemForReview>>(ItemsJson, options);
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        message = "Invalid Items JSON format",
                        error = ex.Message,
                        details = ex.InnerException?.Message,
                        itemsJson = ItemsJson?.Substring(0, Math.Min(500, ItemsJson.Length))
                    });
                }
            }

            if (items == null || !items.Any())
                return BadRequest("Items are required");


            var command = new ReviewWIRCheckPointCommand(
                WIRId: wirId,
                Status: Status,
                Comment: Comment,
                InspectorRole: InspectorRole,
                Files: Files,
                ImageUrls: validImageUrls,
                Items: items,
                FileNames: fileNames
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

       
        //[HttpPost("generate-for-box/{boxId}")]
        //public async Task<IActionResult> GenerateWIRsForBox(Guid boxId, CancellationToken cancellationToken)
        //{
        //    var result = await _mediator.Send(new Dubox.Application.Features.WIRCheckpoints.Commands.GenerateWIRsForBoxCommand(boxId), cancellationToken);
        //    return result.IsSuccess ? Ok(result) : BadRequest(result);
        //}

        
        //[HttpGet("box/{boxId}/with-checklist")]
        //public async Task<IActionResult> GetWIRsByBoxWithChecklist(Guid boxId, CancellationToken cancellationToken)
        //{
        //    var result = await _mediator.Send(new Dubox.Application.Features.WIRCheckpoints.Queries.GetWIRsByBoxWithChecklistQuery(boxId), cancellationToken);
        //    return result.IsSuccess ? Ok(result) : BadRequest(result);
        //}
    }
}
