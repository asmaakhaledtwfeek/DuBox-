using Dubox.Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dubox.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/projects/weather")]
    public class ProjectWeatherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectWeatherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get today's weather report for a specific project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Weather report for the project</returns>
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectWeatherReport(Guid projectId)
        {
            var query = new GetProjectWeatherReportQuery(projectId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get today's weather reports for all active projects
        /// </summary>
        /// <returns>List of weather reports for all active projects</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProjectsWeatherReport()
        {
            var query = new GetAllProjectsWeatherReportQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
