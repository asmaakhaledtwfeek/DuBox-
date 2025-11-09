using Dubox.Application.Features.WIRRecords.Commands;
using Dubox.Application.Features.WIRRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WIRRecordsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WIRRecordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all WIR records
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllWIRRecords(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllWIRRecordsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get WIR record by ID
    /// </summary>
    [HttpGet("{wirRecordId}")]
    public async Task<IActionResult> GetWIRRecordById(Guid wirRecordId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWIRRecordByIdQuery(wirRecordId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all WIR records for a specific box
    /// </summary>
    [HttpGet("box/{boxId}")]
    public async Task<IActionResult> GetWIRRecordsByBox(Guid boxId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWIRRecordsByBoxQuery(boxId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Create a new WIR record (Work Inspection Request)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateWIRRecord([FromBody] CreateWIRRecordCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetWIRRecordById), new { wirRecordId = result.Data!.WIRRecordId }, result) : BadRequest(result);
    }

    /// <summary>
    /// Approve a WIR record (QC Inspector)
    /// </summary>
    [HttpPost("{wirRecordId}/approve")]
    public async Task<IActionResult> ApproveWIRRecord(Guid wirRecordId, [FromBody] ApproveWIRRecordCommand command, CancellationToken cancellationToken)
    {
        if (wirRecordId != command.WIRRecordId)
            return BadRequest("WIR Record ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Reject a WIR record (QC Inspector)
    /// </summary>
    [HttpPost("{wirRecordId}/reject")]
    public async Task<IActionResult> RejectWIRRecord(Guid wirRecordId, [FromBody] RejectWIRRecordCommand command, CancellationToken cancellationToken)
    {
        if (wirRecordId != command.WIRRecordId)
            return BadRequest("WIR Record ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

