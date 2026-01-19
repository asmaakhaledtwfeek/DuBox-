using Dubox.Application.Features.Cost.Commands;
using Dubox.Application.Features.Cost.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CostController> _logger;

    public CostController(
        IMediator mediator,
        IWebHostEnvironment environment,
        ILogger<CostController> logger)
    {
        _mediator = mediator;
        _environment = environment;
        _logger = logger;
    }

    [HttpGet("codes")]
    public async Task<IActionResult> GetCostCodes(
        [FromQuery] string? code,
        [FromQuery] string? costCodeLevel1,
        [FromQuery] string? costCodeLevel2,
        [FromQuery] string? costCodeLevel3,
        [FromQuery] string? level1Description,
        [FromQuery] string? level2Description,
        [FromQuery] string? level3Description,
        [FromQuery] bool? isActive,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetCostCodesQuery
        {
            Code = code,
            CostCodeLevel1 = costCodeLevel1,
            CostCodeLevel2 = costCodeLevel2,
            CostCodeLevel3 = costCodeLevel3,
            Level1Description = level1Description,
            Level2Description = level2Description,
            Level3Description = level3Description,
            IsActive = isActive,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return result.IsSuccess 
            ? Ok(result) 
            : BadRequest(result);
    }

    [HttpPost("codes")]
    public async Task<IActionResult> CreateCostCode([FromBody] CreateCostCodeCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating cost code");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    /// <summary>
    /// Import cost codes from Excel file in wwwroot/data
    /// </summary>
    [HttpPost("import")]
    public async Task<IActionResult> ImportCostCodes(
        [FromQuery] string fileName,
        [FromQuery] bool clearExisting = false)
    {
        try
        {
            var filePath = Path.Combine(_environment.WebRootPath, "data", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File not found: {fileName}. Please ensure the file is in wwwroot/data directory.");
            }

            var command = new ImportCostCodesCommand
            {
                FilePath = filePath,
                ClearExisting = clearExisting
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess 
                ? Ok(new { message = "Import completed", result }) 
                : BadRequest(new { message = "Import failed", error = result.Error });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing cost codes");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    /// <summary>
    /// Seed/Import ALL cost files from wwwroot/data directory
    /// </summary>
    [HttpPost("seed")]
    public async Task<IActionResult> SeedCostData([FromQuery] bool clearExisting = false)
    {
        try
        {
            var dataPath = Path.Combine(_environment.WebRootPath, "data");
            
            if (!Directory.Exists(dataPath))
            {
                return NotFound("Data directory not found. Please create wwwroot/data and add Excel files.");
            }

            var results = new List<object>();

            // Import Cost Codes Master file
            var costCodeFile = Path.Combine(dataPath, "Cost Codes Master - Amana.xlsx");
            if (System.IO.File.Exists(costCodeFile))
            {
                var costCodeCommand = new ImportCostCodesCommand
                {
                    FilePath = costCodeFile,
                    ClearExisting = clearExisting
                };

                var costCodeResult = await _mediator.Send(costCodeCommand);
                results.Add(new
                {
                    file = "Cost Codes Master - Amana.xlsx",
                    success = costCodeResult.IsSuccess,
                    result = costCodeResult
                });

                // Only clear on first import
                clearExisting = false;
            }

            // Import EportHRC file (if structure matches)
            var reportFile = Path.Combine(dataPath, "EportHRC 1.xls");
            if (System.IO.File.Exists(reportFile))
            {
                // Note: This file might have different structure, 
                // adjust import logic as needed
                results.Add(new
                {
                    file = "EportHRC 1.xls",
                    success = false,
                    message = "File found but import logic not yet implemented for this format"
                });
            }

            _logger.LogInformation("Seed operation completed. Processed {Count} files", results.Count);

            return Ok(new
            {
                message = "Seed operation completed",
                timestamp = DateTime.UtcNow,
                filesProcessed = results.Count,
                results
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding cost data");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    /// <summary>
    /// Get HR/Resource costs with optional filtering and pagination
    /// </summary>
    [HttpGet("hr-costs")]
    public async Task<IActionResult> GetHRCosts(
        [FromQuery] string? code,
        [FromQuery] string? name,
        [FromQuery] string? costType,
        [FromQuery] bool? isActive,
        [FromQuery] string? units,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetHRCostsQuery
        {
            Name = name,
            Code = code,
            IsActive = isActive,
            Units = units,
            CostType = costType,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return result.IsSuccess 
            ? Ok(result) 
            : BadRequest(result);
    }

    /// <summary>
    /// Create a new HR cost record
    /// </summary>
    [HttpPost("hr-costs")]
    public async Task<IActionResult> CreateHRCost([FromBody] CreateHRCostCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating HR cost");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    /// <summary>
    /// Import HR costs from Excel file
    /// </summary>
    [HttpPost("import-hr-costs")]
    public async Task<IActionResult> ImportHRCosts(
        [FromQuery] string fileName,
        [FromQuery] bool clearExisting = false)
    {
        try
        {
            var filePath = Path.Combine(_environment.WebRootPath, "data", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File not found: {fileName}");
            }

            var command = new ImportHRCostsCommand
            {
                FilePath = filePath,
                ClearExisting = clearExisting
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess 
                ? Ok(new { message = "Import completed", result }) 
                : BadRequest(new { message = "Import failed", error = result.Error });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing HR costs");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    /// <summary>
    /// Get project costs by project ID with optional filtering and pagination
    /// </summary>
    [HttpGet("project-costs/project/{projectId}")]
    public async Task<IActionResult> GetProjectCostsByProjectId(
        Guid projectId,
        [FromQuery] string? costType,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var query = new GetProjectCostsByProjectIdQuery
            {
                ProjectId = projectId,
                CostType = costType,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project costs");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new project cost for a box
    /// </summary>
    [HttpPost("project-costs")]
    public async Task<IActionResult> CreateProjectCost([FromBody] CreateProjectCostCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project cost");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    /// <summary>
    /// Get distinct cost types from HR costs
    /// </summary>
    [HttpGet("cost-types")]
    public async Task<IActionResult> GetDistinctCostTypes()
    {
        try
        {
            var query = new GetDistinctCostTypesQuery();
            var result = await _mediator.Send(query);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting distinct cost types");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    /// <summary>
    /// Get list of available cost data files
    /// </summary>
    [HttpGet("files")]
    [AllowAnonymous]
    public IActionResult GetAvailableFiles()
    {
        try
        {
            var dataPath = Path.Combine(_environment.WebRootPath, "data");
            
            if (!Directory.Exists(dataPath))
            {
                return Ok(new { files = new string[0], message = "Data directory not found" });
            }

            var files = Directory.GetFiles(dataPath, "*.xls*")
                .Select(f => new
                {
                    name = Path.GetFileName(f),
                    size = new FileInfo(f).Length,
                    lastModified = new FileInfo(f).LastWriteTimeUtc
                })
                .ToList();

            return Ok(new
            {
                dataPath,
                files
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available files");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }
}

