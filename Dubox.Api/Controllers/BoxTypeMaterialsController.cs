using Dubox.Application.Features.BoxTypeMaterials.Commands;
using Dubox.Application.Features.BoxTypeMaterials.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/box-type-materials")]
[Authorize]
public class BoxTypeMaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoxTypeMaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all materials for a specific box type
    /// </summary>
    [HttpGet("box-type/{projectBoxTypeId}")]
    public async Task<IActionResult> GetBoxTypeMaterials(int projectBoxTypeId)
    {
        var query = new GetBoxTypeMaterialsQuery(projectBoxTypeId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
            return Ok(result.Data);

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Get all box type materials for a project (across all box types)
    /// Optionally filter by building and/or level
    /// </summary>
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetProjectBoxTypeMaterials(
        Guid projectId,
        [FromQuery] string? buildingNumber = null,
        [FromQuery] string? floor = null)
    {
        var query = new GetProjectBoxTypeMaterialsQuery(projectId, buildingNumber, floor);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
            return Ok(result.Data);

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Mark a box type material as arrived
    /// </summary>
    [HttpPost("{boxTypeMaterialId}/mark-arrived")]
    public async Task<IActionResult> MarkAsArrived(
        Guid boxTypeMaterialId, 
        [FromBody] MarkMaterialArrivedToBoxTypesRequest request)
    {
        var command = new MarkBoxTypeMaterialArrivedCommand(
            boxTypeMaterialId, 
            request.DeliveryProgress, 
            request.DeliveredQuantity,
            request.Notes);
        
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(new { message = "Material marked as arrived successfully" });

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Mark a box type material as pending (undo arrival)
    /// </summary>
    [HttpPost("{boxTypeMaterialId}/mark-pending")]
    public async Task<IActionResult> MarkAsPending(Guid boxTypeMaterialId)
    {
        var command = new MarkBoxTypeMaterialPendingCommand(boxTypeMaterialId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(new { message = "Material marked as pending successfully" });

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Synchronize BoxTypeMaterial records from existing template assignments
    /// This endpoint populates BoxTypeMaterial records for projects that had templates assigned before this feature was implemented
    /// </summary>
    [HttpPost("project/{projectId}/sync")]
    public async Task<IActionResult> SyncBoxTypeMaterials(Guid projectId)
    {
        var command = new SyncBoxTypeMaterialsCommand(projectId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(new { 
                message = $"Successfully synchronized {result.Data} box type material(s)",
                count = result.Data 
            });

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Remove a material assignment from a specific box type
    /// Allows users to remove materials that were auto-assigned from project-level selections
    /// </summary>
    [HttpDelete("{boxTypeMaterialId}")]
    public async Task<IActionResult> RemoveBoxTypeMaterial(Guid boxTypeMaterialId)
    {
        var command = new RemoveBoxTypeMaterialCommand(boxTypeMaterialId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(new { message = "Material removed from box type successfully" });

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Update the quantity per box for a specific box type material
    /// </summary>
    [HttpPut("{boxTypeMaterialId}/quantity")]
    public async Task<IActionResult> UpdateQuantityPerBox(
        Guid boxTypeMaterialId, 
        [FromBody] UpdateQuantityPerBoxRequest request)
    {
        var command = new UpdateBoxTypeMaterialQuantityCommand(
            boxTypeMaterialId, 
            request.QuantityPerBox);
        
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(new { message = "Quantity per box updated successfully" });

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Generate an Excel file with box type materials for editing and re-import
    /// </summary>
    [HttpGet("box-type/{projectBoxTypeId}/export")]
    public async Task<IActionResult> ExportBoxTypeMaterials(int projectBoxTypeId)
    {
        var query = new GenerateBoxTypeMaterialsExcelQuery(projectBoxTypeId);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return File(result.Data!,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"BoxTypeMaterials_BoxType{projectBoxTypeId}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }

    /// <summary>
    /// Import box type materials from Excel file (updates DeliveredQuantity and QuantityPerBox)
    /// This endpoint is at the box type level, not project level
    /// </summary>
    [HttpPost("box-type/{projectBoxTypeId}/import")]
    [RequestSizeLimit(10_485_760)] // 10 MB
    public async Task<IActionResult> ImportBoxTypeMaterials(
        int projectBoxTypeId, 
        [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var command = new ImportBoxTypeMaterialsFromExcelCommand(
            projectBoxTypeId, 
            memoryStream, 
            file.FileName);
        
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new 
            { 
                message = $"Import completed. {result.Data!.SuccessCount} material(s) updated successfully.",
                successCount = result.Data.SuccessCount,
                failureCount = result.Data.FailureCount,
                errors = result.Data.Errors,
                warnings = result.Data.Warnings
            });
        }

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Generate an Excel file with all box type materials for the project (all box types)
    /// </summary>
    [HttpGet("project/{projectId}/export")]
    public async Task<IActionResult> ExportProjectMaterials(Guid projectId)
    {
        var query = new GenerateProjectMaterialsExcelQuery(projectId);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return File(result.Data!,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"ProjectMaterials_{projectId}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }

    /// <summary>
    /// Import all box type materials for a project from Excel file (updates multiple box types)
    /// </summary>
    [HttpPost("project/{projectId}/import")]
    [RequestSizeLimit(10_485_760)] // 10 MB
    public async Task<IActionResult> ImportProjectMaterials(
        Guid projectId, 
        [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var command = new ImportProjectMaterialsFromExcelCommand(
            projectId, 
            memoryStream, 
            file.FileName);
        
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new 
            { 
                message = $"Import completed. {result.Data!.SuccessCount} material(s) updated successfully across all box types.",
                successCount = result.Data.SuccessCount,
                failureCount = result.Data.FailureCount,
                errors = result.Data.Errors,
                warnings = result.Data.Warnings
            });
        }

        return BadRequest(result.Message);
    }
}

// Request DTOs
public record MarkMaterialArrivedToBoxTypesRequest(
    int DeliveryProgress = 100,
    decimal? ArrivedQuantity = null,
    decimal? DeliveredQuantity = null,
    string? Notes = null
);

public record UpdateQuantityPerBoxRequest(
    int QuantityPerBox
);

