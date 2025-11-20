using Dubox.Application.Features.WIRCheckpoints.Commands;
using Dubox.Application.Features.WIRCheckpoints.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("{wirId}/checklist-items")]
        public async Task<IActionResult> AddChecklistItems(Guid wirId, [FromBody] AddChecklistItemsCommand command, CancellationToken cancellationToken)
        {
            if (wirId != command.WIRId)
                return BadRequest("WIR Check point ID mismatch");
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost("{wirId}/quality-issues")]
        public async Task<IActionResult> AddQualityIssues(Guid wirId, [FromBody] AddQualityIssuesCommand command, CancellationToken cancellationToken)
        {
            if (wirId != command.WIRId)
                return BadRequest("WIR Check point ID mismatch");
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut("{wirId}/review")]
        public async Task<IActionResult> ReviewWIRCheckPoint(Guid wirId, [FromBody] ReviewWIRCheckPointCommand command, CancellationToken cancellationToken)
        {
            if (wirId != command.WIRId)
                return BadRequest("WIR Check point ID mismatch");
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
    }
}
