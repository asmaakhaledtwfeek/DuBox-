using Dubox.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BIMController : ControllerBase
{
    private readonly IDbContext _context;

    public BIMController(IDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all BIM models
    /// </summary>
    [HttpGet("models")]
    public async Task<IActionResult> GetBIMModels()
    {
        var models = await _context.BIMModels
            .OrderByDescending(m => m.CreatedDate)
            .Select(m => new
            {
                m.BIMModelId,
                m.ModelName,
                m.Category,
                m.RevitFamily,
                m.Type,
                m.Instance,
                m.Quantity,
                m.Unit,
                m.PlannedStartDate,
                m.PlannedFinishDate,
                m.ThumbnailPath
            })
            .ToListAsync();

        return Ok(new { success = true, data = models });
    }

    /// <summary>
    /// Get BIM model by ID
    /// </summary>
    [HttpGet("models/{id}")]
    public async Task<IActionResult> GetBIMModel(Guid id)
    {
        var model = await _context.BIMModels.FindAsync(id);

        if (model == null)
        {
            return NotFound(new { success = false, message = "BIM model not found" });
        }

        return Ok(new { success = true, data = model });
    }

    /// <summary>
    /// Create BIM model
    /// </summary>
    [HttpPost("models")]
    public async Task<IActionResult> CreateBIMModel([FromBody] CreateBIMModelRequest request)
    {
        var model = new Domain.Entities.BIMModel
        {
            ModelName = request.ModelName,
            Category = request.Category,
            RevitFamily = request.RevitFamily,
            Type = request.Type,
            Instance = request.Instance,
            Quantity = request.Quantity,
            Unit = request.Unit,
            PlannedStartDate = request.PlannedStartDate,
            PlannedFinishDate = request.PlannedFinishDate,
            ProjectId = request.ProjectId,
            Description = request.Description,
            CreatedDate = DateTime.UtcNow
        };

        _context.BIMModels.Add(model);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, data = model.BIMModelId });
    }
}

public record CreateBIMModelRequest(
    string ModelName,
    string? Category,
    string? RevitFamily,
    string? Type,
    string? Instance,
    decimal? Quantity,
    string? Unit,
    DateTime? PlannedStartDate,
    DateTime? PlannedFinishDate,
    Guid? ProjectId,
    string? Description
);



