using Dubox.Application.Features.QualityIssues.Commands;
using Dubox.Application.Features.QualityIssues.Queries;
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
        public async Task<IActionResult> UpdateQualityIssueStatus(Guid issueId, [FromBody] UpdateQualityIssueStatusCommand command, CancellationToken cancellationToken)
        {
            if (issueId != command.IssueId)
                return BadRequest("Issue ID mismatch");
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
