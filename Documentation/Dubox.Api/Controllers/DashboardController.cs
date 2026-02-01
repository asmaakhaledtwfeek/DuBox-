using Dubox.Application.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get overall dashboard statistics (all projects)
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetDashboardStatistics(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDashboardStatisticsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get dashboard data for all projects
    /// </summary>
    [HttpGet("projects")]
    public async Task<IActionResult> GetAllProjectsDashboard(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllProjectsDashboardQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get dashboard data for a specific project
    /// </summary>
    [HttpGet("projects/{projectId}")]
    public async Task<IActionResult> GetProjectDashboard(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectDashboardQuery(projectId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

