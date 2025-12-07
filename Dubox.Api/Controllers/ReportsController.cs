using Dubox.Application.Features.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

/// <summary>
/// Controller for generating various reports
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get box progress report - Shows progress distribution across buildings
    /// </summary>
    /// <param name="projectId">Optional project ID to filter results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of box progress data grouped by building</returns>
    [HttpGet("box-progress")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBoxProgressReport([FromQuery] Guid? projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBoxProgressReportQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get team productivity report - Shows team performance metrics
    /// </summary>
    /// <param name="projectId">Optional project ID to filter results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of team productivity metrics</returns>
    [HttpGet("team-productivity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamProductivityReport([FromQuery] Guid? projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamProductivityReportQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get report summary - Overall dashboard statistics
    /// </summary>
    /// <param name="projectId">Optional project ID to filter results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Summary statistics for reports dashboard</returns>
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetReportSummary([FromQuery] Guid? projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetReportSummaryQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get missing materials report - Identifies material shortages
    /// </summary>
    /// <param name="projectId">Optional project ID to filter results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of missing materials with shortage details</returns>
    [HttpGet("missing-materials")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMissingMaterialsReport([FromQuery] Guid? projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMissingMaterialsReportQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get phase readiness report - Tracks phase completion and blocking issues
    /// </summary>
    /// <param name="projectId">Optional project ID to filter results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of phases with readiness status</returns>
    [HttpGet("phase-readiness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPhaseReadinessReport([FromQuery] Guid? projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPhaseReadinessReportQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get boxes summary report - Comprehensive report with filtering, pagination, sorting, KPIs, and charts
    /// </summary>
    /// <param name="query">Query parameters for filtering, pagination, and sorting</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated boxes summary report with KPIs and aggregations</returns>
    [HttpGet("boxes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBoxesSummaryReport([FromQuery] GetBoxesSummaryReportQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

