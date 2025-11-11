using Dubox.Application.Features.Departments.Commands;
using Dubox.Application.Features.Departments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllDepartmentsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{DepartmentId}")]
    public async Task<IActionResult> GetDepartmentById(Guid departmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery(departmentId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetDepartmentById), new { DepartmentId = result.Data!.DepartmentId }, result) : BadRequest(result);
    }

    [HttpPut("{departmentId}")]
    public async Task<IActionResult> UpdateDepartment(Guid departmentId, [FromBody] UpdateDepartmentCommand command, CancellationToken cancellationToken)
    {
        if (departmentId != command.DepartmentId)
            return BadRequest("Project ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{departmentId}")]
    public async Task<IActionResult> DeleteDepartment(Guid departmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteDepartmentCommand(departmentId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
