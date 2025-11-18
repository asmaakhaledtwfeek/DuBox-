using Dubox.Application.Features.Activities.Queries;
using Dubox.Application.Features.AuditLogs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuditLogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditLogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("masters")]
        public async Task<IActionResult> GetAuditLogs([FromQuery] GetAuditLogsQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllActivityMastersQuery(), cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}