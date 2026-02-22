using Dubox.Application.DTOs;
using Dubox.Application.Features.CustomReports.Commands;
using Dubox.Application.Features.CustomReports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/custom-reports")]
[Authorize]
public class CustomReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ─── Data sources metadata ──────────────────────────────────────────────

    [HttpGet("data-sources")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDataSources(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDataSourcesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // ─── Filter options ─────────────────────────────────────────────────────

    [HttpGet("filter-options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFilterOptions(
        [FromQuery] string dataSource,
        [FromQuery] string field,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dataSource) || string.IsNullOrWhiteSpace(field))
            return BadRequest("dataSource and field are required.");

        var result = await _mediator.Send(new GetFilterOptionsQuery(dataSource, field), cancellationToken);
        return Ok(result);
    }

    // ─── CRUD ───────────────────────────────────────────────────────────────

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCustomReportsQuery(page, pageSize, search), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomReportByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] SaveCustomReportCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result)
            : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] SaveCustomReportCommand command, CancellationToken cancellationToken)
    {
        // Override the id from the route
        var commandWithId = command with { Id = id };
        var result = await _mediator.Send(commandWithId, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteCustomReportCommand(id), cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    // ─── Execution & Preview ────────────────────────────────────────────────

    /// <summary>Run a report using an inline config (live preview, no save required).</summary>
    [HttpPost("preview")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Preview(
        [FromBody] ExecuteCustomReportQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>Run a saved report by its id with pagination.</summary>
    [HttpPost("{id:guid}/execute")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Execute(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var reportResult = await _mediator.Send(new GetCustomReportByIdQuery(id), cancellationToken);
        if (!reportResult.IsSuccess) return NotFound(reportResult);

        var query = new ExecuteCustomReportQuery(reportResult.Data!.Config, page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // ─── Export ─────────────────────────────────────────────────────────────

    /// <summary>Export a saved report to Excel using its saved config.</summary>
    [HttpGet("{id:guid}/export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportById(Guid id, CancellationToken cancellationToken)
    {
        var reportResult = await _mediator.Send(new GetCustomReportByIdQuery(id), cancellationToken);
        if (!reportResult.IsSuccess) return NotFound(reportResult);

        return await ExportConfig(reportResult.Data!.Config, reportResult.Data.Name, cancellationToken);
    }

    /// <summary>Export using an inline config (no save required).</summary>
    [HttpPost("export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExportInline(
        [FromBody] ExportCustomReportQuery query,
        CancellationToken cancellationToken)
    {
        return await ExportConfig(query.Config, "custom_report", cancellationToken);
    }

    private async Task<IActionResult> ExportConfig(CustomReportConfig config, string reportName, CancellationToken ct)
    {
        var result = await _mediator.Send(new ExportCustomReportQuery(config), ct);
        if (!result.IsSuccess) return BadRequest(result);

        var safeName = string.Concat(reportName.Split(Path.GetInvalidFileNameChars()));
        var fileName = $"{safeName}_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
        return File(result.Data!, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
